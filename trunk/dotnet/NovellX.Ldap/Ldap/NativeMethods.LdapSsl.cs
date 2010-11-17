// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;
using System.Runtime.InteropServices;

namespace NovellX.Ldap
{
	public static partial class NativeMethods
	{
		/// <summary>
		/// Contains native methods from the ldapssl.dll library.
		/// </summary>
		public static class LdapSsl
		{
			/// <summary>
			/// Creates an LDAP session handle that is SSL enabled.
			/// </summary>
			/// <param name="host">
			/// (IN) Contains the names of the available hosts, each separated by a space, or a list of IP 
			/// addresses (in dot format) of the hosts, each separated by a space. If a port number is included 
			/// with the name or the address, it is separated from them with a colon (:).</param>
			/// <param name="port">
			/// (IN) Contains the TCP port number to connect to, which for an SSL connection is the SSL port 
			/// number of the LDAP server. If a port number is included with the host parameter, this 
			/// parameter is ignored.
			/// </param>
			/// <param name="secure">
			/// (IN) Specifies whether the connection is established over SSL.
			/// * Zero: do not establish the connection over SSL (which makes this function essentially the same as the ldap_init function)
			/// * Non-zero: establish the connection over SSL
			/// </param>
			/// <returns>
			/// * >0: Success; session handle
			/// * NULL: Unsuccessful
			/// </returns>
			/// <remarks>
			/// If you connect to an LDAP v2 server, you must call an LDAP bind operation before performing any 
			/// operations. If you connect to an LDAP v3 server, some operations can be performed before calling a 
			/// bind operation.
			/// Before calling this function, you must first call the ldapssl_client_init function which initializes the 
			/// SSL library. 
			/// Calling the ldapssl_init function is equivalent to calling the ldap_init function followed by the 
			/// ldapssl_install_routines function.
			/// The ldapssl_init function does not actually communicate with the LDAP server. Communication 
			/// begins when the application binds or does some other operation.
			/// The LDAP libraries first contact the first server listed in the host parameter. If they are unable to 
			/// communicate with that server, they try the next server and then the next.
			/// The session handle returned contains opaque data identifying the session. To get or set handle 
			/// information, use ldap_set_option and ldap_get_option. For a list of the handle options, see 
			/// Section 6.10, “Session Preference Options,” on page 427.
			/// For sample code, see sslbind.c (http://developer.novell.com/ndk/doc/samplecode/cldap_sample/
			/// index.htm).
			/// 
			/// IMPORTANT: The ldap_init function allocates memory for the LDAP structure. This memory 
			/// must be freed by calling ldap_unbind or ldap_unbind_s even when an LDAP bind function is not 
			/// called or the LDAP bind function fails.
			/// 
			/// <code>
			/// #include &lt;ldap_ssl.h&gt;
			/// LDAP * ldapssl_init (
			///		const char   *host,
			///		int           port,
			///		int           secure);
			/// </code>
			/// </remarks>
			[DllImport("ldapssl.dll", EntryPoint = "ldapssl_init")]
			public static extern IntPtr init(string host, int port, int secure);

			/// <summary>
			/// Initializes the SSL (Secure Socket Layer) library.
			/// </summary>
			/// <param name="certFile">(IN) Points to the trusted root certificate file, a fully-qualified file path and the file must contain a DER encoded certificate.</param>
			/// <param name="reserved">(IN) Not currently used. Pass a NULL.</param>
			/// <returns>
			/// 0: Success
			/// -1: Failure
			/// </returns>
			/// <remarks>
			/// The LDAP SSL library provides SSL server authentication. In order to verify the server, the library 
			/// needs to be configured with a trusted root certificate.
			/// The certFile parameter is the fully qualified path of a file containing a trusted root certificate DER 
			/// encoded.
			/// It is also possible to pass NULL in the certFile parameter and use ldapssl_add_trusted_cert to add 
			/// trusted root certificates to the LDAP SSL library. The API ldapssl_add_trusted_cert accepts DER 
			/// and B64 (PEM) encoded certificates.
			/// If the SSL handshake fails, the LDAP library returns an LDAP_SERVER_DOWN error. The 
			/// handshake can fail because the server is down or because SSL has not been set up correctly on the 
			/// client or LDAP server.
			/// When you are finished with the SSL library, you should call the ldapssl_client_deinit function.
			/// For sample code, see sslbind.c (http://developer.novell.com/ndk/doc/samplecode/cldap_sample/
			/// index.htm).
			/// 
			/// <code>
			/// #include &lt;ldap_ssl.h&gt;
			/// int ldapssl_client_init (
			///		const char   *certFile,
			///		void         *reserved);
			///	</code>
			/// </remarks>
			[DllImport("ldapssl.dll", EntryPoint = "ldapssl_client_init")]
			public static extern int client_init(string certFile, IntPtr reserved);

			/// <summary>
			/// Deinitializes the SSL library.
			/// </summary>
			/// <returns>
			/// 0x00: LDAP_SUCCESS
			/// Non-zero: Failure. See “LDAP Return Codes”
			/// </returns>
			/// <remarks>
			/// This function must be called after you are finished using the SSL library. Before calling this 
			/// function, all SSL LDAP session handles must be closed using the ldap_unbind function.
			/// For sample code, see sslbind.c (http://developer.novell.com/ndk/doc/samplecode/cldap_sample/
			/// index.htm).
			/// 
			/// <code>
			/// #include &lt;ldap_ssl.h&gt;
			/// int ldapssl_client_deinit (
			///		void);
			/// </code>
			/// </remarks>
			[DllImport("ldapssl.dll", EntryPoint = "ldapssl_client_deinit")]
			public static extern int client_deinit();
		}
	}
}