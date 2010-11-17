// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;
using System.Diagnostics;
using System.Text;
using NovellX.Ldap;

namespace NovellX.Security.Nmas
{
	public class NmasLdap
	{
		private readonly LdapConnection _ldapConnection;

		public NmasLdap(LdapConnection ldapConnection)
		{
			if (ldapConnection == null)
				throw new ArgumentNullException("ldapConnection");
			_ldapConnection = ldapConnection;
		}

		public string GetPassword(string getPasswordUserDN)
		{
			const int getPwdSize = 512;
			var getPwd = new StringBuilder(512);
			var getPwdLen = getPwdSize;
			int err;
			unsafe {
				err = NativeMethods.NmasLdap.get_password(_ldapConnection.Handle, getPasswordUserDN, new IntPtr(&getPwdLen), getPwd);
			}

			if (err == NovellX.Ldap.NativeMethods.Ldap.SUCCESS) {
				//Debug.WriteLine("nmasldap_get_password: {0}", getPwd);
			}
			else {
				Debug.WriteLine("ERROR {0} nmasldap_get_password", err);
			}
			return getPwd.ToString(0, getPwdLen);
		}
	}
}
