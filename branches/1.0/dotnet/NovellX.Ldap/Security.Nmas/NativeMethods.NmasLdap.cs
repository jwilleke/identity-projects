// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;
using System.Runtime.InteropServices;

namespace NovellX.Security.Nmas
{
	internal static partial class NativeMethods
	{
		/// <summary>
		/// Contains native methods from the NMAS native libraries.
		/// </summary>
		internal static class NmasLdap
		{
			public static readonly int E_BASE = (-1600);
			public static readonly int E_SERVER_NOT_FOUND = (E_BASE - 64)     /* -1664 0xFFFFF980 */;

			/// <summary>
			/// Reads the password.
			/// </summary>
			/// <param name="ld">(IN) The LDAP session handle. </param>
			/// <param name="objectDN">
			/// (IN) Identifies the object that holds the login data. An LDAP DN. 
			/// C: UTF-8 string
			/// </param>
			/// <param name="pwdLen">
			/// (OUT) Password length in bytes.
			/// C: size_t int</param>
			/// <param name="pwd">
			/// (OUT) Specifies the password.
			/// C: UTF-8 string
			/// </param>
			/// <returns>
			/// Returns NMAS_E_SUCCESS (0) if successful or a non-zero NMAS Error Code if not successful.
			/// </returns>
			/// <remarks>
			/// <code>
			/// #include &lt;ldap.h&gt;
			/// #include &lt;ntypes.h&gt;
			/// #include &lt;nmasext.h&gt;
			/// int nmasldap_get_password
			/// (
			///      LDAP     *ld,
			///      char     *objectDN,
			///      size_t   *pwdLen,   
			///      char     *pwd 
			/// );
			/// </code>
			/// </remarks>
			[DllImport("nmasExt.dll", EntryPoint = "nmasldap_get_password", CallingConvention = CallingConvention.Cdecl)]
			public static extern int get_password(IntPtr ld, [MarshalAs(UnmanagedType.LPStr)] string objectDN, IntPtr pwdLen, [MarshalAs(UnmanagedType.LPStr)] System.Text.StringBuilder pwd);
		}
	}
}