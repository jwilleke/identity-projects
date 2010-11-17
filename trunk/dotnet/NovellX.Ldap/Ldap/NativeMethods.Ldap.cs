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
		// ReSharper disable InconsistentNaming

		/// <summary>
		/// Contains native methods from the ldapsdk.dll library.
		/// </summary>
		public static class Ldap
		{
			
			public static readonly int VERSION3 = 3;
			public static readonly int SUCCESS = 0x00;

			/// <summary>
			/// Sets the value of session-wide parameters.
			/// </summary>
			/// <param name="ld">(IN) Points to the session handle. If this is NULL, the function accesses the global defaults.</param>
			/// <param name="option">(IN) Specifies the name of the option which is being set </param>
			/// <param name="invalue">(IN) Points to the value to which the specified option is set.</param>
			/// <returns>
			/// 0x00 LDAP_SUCCESS
			/// -1 Failure
			/// </returns>
			/// <remarks>
			/// The ldap_init function returns the value for the ld parameter. If you use the ldap_set_option function 
			/// before calling ldap_init and use NULL for the ld parameter, the values are set globally and copied to 
			/// all LDAP session handles you create afterwards. If the ldap_set_option function is called after the 
			/// ldap_init function, one of the following occurs:
			///  If the ld parameter is NULL, the values are set globally but do not affect the values in currently 
			/// created LDAP session handles.
			///  If the ld parameter is set to the value returned by the ldap_init function, the values are set for 
			/// only that LDAP session handle.
			/// 
			/// The following examples illustrate how to globally set two of the options.
			/// <code>
			/// /* Don’t chase referrals */
			/// rc = ldap_set_option( NULL, LDAP_OPT_REFERRALS, LDAP_OPT_OFF );  
			/// /* Set LDAP version 3 */
			/// int version = LDAP_VERSION3;
			/// rc = ldap_set_option( NULL, LDAP_OPT_PROTOCOL_VERSION, &version );
			/// </code>
			/// 
			/// <code>
			/// #include &lt;ldap.h&gt;
			/// int ldap_set_option (
			///     LDAP               *ld,
			///     int                 option,
			///     LDAP_CONST void    *invalue);
			/// </code>
			/// </remarks>
			[DllImport("ldapsdk.dll", EntryPoint = "ldap_set_option")]
			public static extern int set_option(IntPtr ld, LdapOptions.Option option, IntPtr invalue);

			/// <summary>
			/// A wrapper that allows passing a managed int as the value.
			/// </summary>
			public static int set_option(IntPtr ld, LdapOptions.Option option, int invalue)
			{
				unsafe
				{
					var pValue = new IntPtr(&invalue);
					int ret = set_option(IntPtr.Zero, option, pValue);
					return ret;
				}
			}

			/// <summary>
			/// A wrapper that allows passing in a managed array as a value.
			/// </summary>

			public static int set_option(IntPtr ld, LdapOptions.Option option, int[] value)
			{
				unsafe
				{
					fixed (int* pValue = &value[0])
					{
						int ret = set_option(IntPtr.Zero, option, new IntPtr(pValue));
						return ret;
					}
				}
			}

			/// <summary>Initializes an LDAP session associated with an LDAP server and returns a pointer to a session handle.</summary>
			/// <param name="host">(IN) Contains the names of the available hosts, each separated by a space, or a list of IP 
			/// addresses (in dot format) of the hosts, each separated by a space. If a port number is included 
			/// with the name or the address, it is separated from them with a colon (:). </param>
			/// <param name="port">(IN) Contains the TCP port number to connect to. If a port number is included with the host 
			/// parameter, this parameter is ignored.</param>
			/// <returns>
			/// >0 Success; session handle
			/// NULL Unsuccessful
			/// </returns>
			/// <remarks>
			/// If you connect to an LDAP v2 server, you must call an LDAP bind operation before performing any 
			/// operations. If you connect to an LDAP v3 server, some operations can be performed before calling a 
			/// bind operation.
			/// The ldap_init function does not actually communicate with the LDAP server. Communication 
			/// begins when the application binds or does some other operation.
			/// The LDAP libraries first contact the first server listed in the host parameter. If they are unable to 
			/// communicate with that server, they try the next server and then the next.
			/// 
			/// The session handle returned contains opaque data identifying the session. To get or set handle 
			/// information, use ldap_set_option and ldap_get_option. For a list of the handle options, see 
			/// Section 6.10, “Session Preference Options,” on page 427.
			/// IMPORTANT: The ldap_init function allocates memory for the LDAP structure. This memory 
			/// must be freed by calling ldap_unbind or ldap_unbind_s even when an LDAP bind function is not 
			/// called or the LDAP bind function fails.
			/// 
			/// 
			/// <code>
			/// #include <ldap.h>
			/// LDAP *ldap_init (
			///     const char   *host,
			///     int           port);
			/// </code>        
			/// </remarks>
			[DllImport("ldapsdk.dll", EntryPoint = "ldap_init")]
			public static extern IntPtr init(string host, int port);

			/// <summary>
			/// Synchronously authenticates the specified client to the LDAP server using a distinguished name and password.
			/// </summary>
			/// <param name="ld">(IN) Points to the handle for the LDAP session.</param>
			/// <param name="dn">(IN) Points to the distinguished name of the entry who is authenticating. For an anonymous authentication, set this parameter to NULL</param>
			/// <param name="passwd">(IN) Points to the client's password. For an anonymous authentication, set this parameter to NULL.</param>
			/// <returns>
			/// 0x00 LDAP_SUCCESS
			/// Non-zero Failure. For a complete list, see “LDAP Return Codes”.
			/// 0x54 LDAP_DECODING_ERROR
			/// 0x59 LDAP_PARAM_ERROR
			/// 0x5A LDAP_NO_MEMORY
			/// 0x5C LDAP_NOT_SUPPORTED
			/// </returns>
			/// <remarks>
			/// By default, eDirectory does not accept clear text passwords. Make sure that the parameter for 
			/// encrypted passwords is set to allow unencrypted passwords.
			/// An anonymous bind to an eDirectory directory allows clients to access whatever the [Public] user 
			/// has been granted access to. By default, this is just enough to allow the user to find an eDirectory 
			/// server, match a distinguished name, and authenticate.
			/// The LDAP_OPT_NETWORK_TIMEOUT option (set by calling ldap_set_option (page 277)) 
			/// enables you to set a timeout for the initial connection to a server. If no timeout is set, timeout 
			/// depends upon the underlying socket timeout setting of the operating system.
			/// Using the connection timeout, you can also specify multiple hosts separated by spaces in a bind call, 
			/// then use a timeout to determine how long your application will wait for an initial response before 
			/// attempting a connection to the next host in the list.
			/// Passing NULL for the ld parameter of ldap_set_option sets this timeout as the default connection 
			/// timeout for subsequent session handles created with ldap_init (page 179) or ldapssl_init (page 308). 
			/// To clear the timeout pass NULL for the invalue parameter of ldap_set_option.
			/// A connection timeout will cause an LDAP_SERVER_DOWN error (81) "Can't contact LDAP 
			/// server".
			/// For sample code, see bind.c (http://developer.novell.com/ndk/doc/samplecode/cldap_sample/
			/// index.htm).
			/// 
			/// <code>
			/// #include &lt;ldap.h&gt;
			/// int ldap_simple_bind_s (
			///    LDAP         *ld,
			///    const char   *dn,
			///    const char   *passwd);
			/// </code>
			/// </remarks>
			[DllImport("ldapsdk.dll", EntryPoint = "ldap_simple_bind_s")]
			public static extern int simple_bind_s(IntPtr ld, string dn, string passwd);

			/// <summary>
			/// Unbinds from the directory, closes the connection, and frees resources associated with the session. 
			/// Functionally, there are no differences between ldap_unbind and ldap_unbind_s.
			/// </summary>
			/// <param name="ld">(IN) Points to the handle of the LDAP session that is to be unbound.</param>
			/// <returns>
			/// 0x00 LDAP_SUCCESS
			/// Non-zero Failure. See “LDAP Return Codes”.
			/// </returns>
			/// <remarks>
			/// After the call to ldap_unbind[_s], the session handle (ld) is invalid. 
			/// Note that there are no funtional differences between the four unbind functions.
			/// <code>
			/// #include &lt;ldap.h&gt;
			/// int ldap_unbind[_s] (
			/// LDAP *ld);
			/// </code>
			/// </remarks>
			[DllImport("ldapsdk.dll", EntryPoint = "ldap_unbind")]
			public static extern int unbind(IntPtr ld);

			/// <summary>
			/// Converts a numeric LDAP error code into a character string.
			/// </summary>
			/// <param name="err">(IN) Specifies an LDAP error code returned by an LDAP function.</param>
			/// <returns>>0: Pointer to a zero-terminated character string.</returns>
			/// <remarks>
			/// The ldap_err2string function converts LDAP error codes returned by the following functions:
			///  * ldap_parse_result
			///  * ldap_parse_sasl_bind_result
			///  * ldap_parse_extended_result
			///  * synchronous LDAP operation functions
			/// 
			/// The LDAP error code is converted to a zero-terminated character string which describes the error.
			/// The return value points to a string contained in static data. Be aware of the following:
			///  * It should be used or copied before another call to ldap_err2string is made.
			///  * The pointer should not be used to modify the original string.
			///  * The string should not be freed by the application program. 
			///  * The returned string is UTF-8 encoded if the API succeeds.
			/// If the API succeeds, errno is set to 0. Else, the returned string will be in local codepage.
			/// 
			/// If the retuned string is UTF-8 encoded then it has to be converted into the local codepage before you 
			/// can print it. Otherwise, the returned pointer can be used directly in a printf statement as displayed in 
			/// the following example:
			/// <code>
			/// err=ldap_search(...);
			/// if (err)   
			/// {    
			///		char *s;    
			///		s= ldap_err2string(err);    
			///		if (errno==0) // returned string is utf8 encoded        
			///		{     
			///			//convert to local codepage and print it        
			///		} 
			///		else // returned string is not utf8 encoded, it is in local codepage          
			///			printf("Search error: %s\n",s);    
			///	}
			/// </code>
			/// For information on converting utf8 to local code page, refer to the utf8bind.c sample code
			/// 
			/// <code>
			/// #include &lt;ldap.h&gt;
			/// char *ldap_err2string (
			/// int   err);
			/// </code>
			/// </remarks>
			[DllImport("ldapsdk.dll", EntryPoint = "ldap_err2string")]
			public static extern string err2string(int err);
		}
		// ReSharper restore InconsistentNaming
	}
}