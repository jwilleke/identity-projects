// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;
using System.IO;
using NovellX.Ldap;
using NovellX.Security.Nmas;

namespace NmasGetPassword
{
	class Program
	{
		private const string Usage =
			"\n Usage:   NmasGetPassword <host name> <port number> <login dn> <password> <derCertFile> <getPasswordForUserDN>" +
			"\n Example: NmasGetPassword Acme.com 389 cn=admin,o=Acme secret c:\\certs\\mycert.der cn=admin,o=AcmeUser2\n";

		static int Main(string[] args)
		{
			if (args.Length != 6)
			{
				Console.WriteLine(Usage);
				return -1;
			}
			var ldapHost = args[0];
			var ldapPort = int.Parse(args[1]);
			var loginDn = args[2];
			var password = args[3];
			var derFile = args[4];
			var getPasswordUserDn = args[5];

			return GetPassword(ldapHost, ldapPort, loginDn, password, derFile, getPasswordUserDn);
		}

		public static int GetPassword(string ldapHost, int ldapPort, string loginDN, string password, string derFile, string getPasswordUserDN)
		{
			// establish LDAP connection
			LdapOptions.SetOption(LdapOptions.Option.PROTOCOL_VERSION, LdapOptions.Version3);

			// Initialize the ldap connection for ssl
			using (var ldapConnection = LdapConnection.InitSsl(ldapHost, 636, new FileInfo(derFile)))
			{
				ldapConnection.SimpleBind(loginDN, password);
				// get current password
				string pwd = new NmasLdap(ldapConnection).GetPassword(getPasswordUserDN);
				Console.WriteLine("nmasldap_get_password: {0}", pwd);
				Console.ReadLine();
			}
			return 0;
		}

		#region Examples of using Native API Directly
		/* 
		 * 
		 * Just for example sake, here is how to do the same with the NativeAPI instead of the clean class wrappers.
		 * 
		 * 
		public static int GetPasswordNativeAPI(string ldapHost, int ldapPort, string loginDN, string password, string derFile, string getPasswordUserDN)
		{
			int err;
			// establish LDAP connection
			NativeMethods.Ldap.set_option(IntPtr.Zero, LdapOptions.Option.PROTOCOL_VERSION, NativeMethods.Ldap.VERSION3);

			// initialize the ssl library
			err = NativeMethods.LdapSsl.client_init(derFile, IntPtr.Zero);
			if (err != NativeMethods.Ldap.SUCCESS)
			{
				Console.WriteLine("ldapssl_client_init failed {0}", err);
				return err;
			}

			// Initialize the ldap connection for ssl
			IntPtr ld;
			if ((ld = NativeMethods.LdapSsl.init(ldapHost, 636, 1)) == IntPtr.Zero)
			{
				err = NovellX.Security.Nmas.NativeMethods.NmasLdap.E_SERVER_NOT_FOUND;
				Console.WriteLine("(LSMLDAP)- ldapssl_init failed");
			}

			if (err == NativeMethods.Ldap.SUCCESS)
			{
				// Perform Simple BIND
				err = NativeMethods.Ldap.simple_bind_s(ld, loginDN, password);
				if (err != NativeMethods.Ldap.SUCCESS)
				{
					Console.WriteLine("ERROR {0} ldap_simple_bind_s", err);
				}
				else
				{
					Console.WriteLine("simple_bind_s: Succeeded");
					// get current password
					const int getPwdSize = 512;
					var getPwd = new StringBuilder(512);
					var getPwdLen = getPwdSize;
					unsafe
					{
						err = NovellX.Security.Nmas.NativeMethods.NmasLdap.get_password(ld, getPasswordUserDN, new IntPtr(&getPwdLen), getPwd);
					}

					if (err == NativeMethods.Ldap.SUCCESS)
					{
						Console.WriteLine("nmasldap_get_password: {0}", getPwd);
					}
					else
					{
						Console.WriteLine("ERROR {0} nmasldap_get_password", err);
					}
					// unbind
					NativeMethods.Ldap.unbind(ld);
				}
			}

			NativeMethods.LdapSsl.client_deinit();

			Console.ReadLine();
			return 0;
		}
		*/

		/*
		 * 
		 * For example sake, how to do a simple bind without SSL via the Native API
		 * 
		 * 
		public static int BindWithoutSSLNative(string ldapHost, int ldapPort, string loginDN, string password)
		{
			// Set LDAP version to 3 and set connection timeout. 
			NativeMethods.Ldap.set_option(IntPtr.Zero, LdapOptions.Option.PROTOCOL_VERSION, NativeMethods.Ldap.VERSION3);
			int[] timeout = { 10, 0 };  // 10 second connection/search timeout
			NativeMethods.Ldap.set_option(IntPtr.Zero, LdapOptions.Option.NETWORK_TIMEOUT, timeout);

			/* Initialize the LDAP session ♥1♥
			Console.WriteLine("host: {0}", ldapHost);
			Console.WriteLine("port: {0}", ldapPort);

			IntPtr ld;
			if ((ld = NativeMethods.Ldap.init(ldapHost, ldapPort)) == null)
			{
				Console.WriteLine("LDAP session initialization failed");
				return 1;
			}
			Console.WriteLine("LDAP session initialized");

			/* Bind to the server ♥1♥
			Console.WriteLine("loginDN: {0}", loginDN);
			Console.WriteLine("password: {0}", password);

			int rc = NativeMethods.Ldap.simple_bind_s(ld, loginDN, password);

			if (rc != NativeMethods.Ldap.SUCCESS)
			{
				Console.WriteLine("ldap_simple_bind_s: {0}", NativeMethods.Ldap.err2string(rc));
				NativeMethods.Ldap.unbind(ld);
				return (1);
			}
			Console.WriteLine("Bind successful");

			NativeMethods.Ldap.unbind(ld);
			return 0;
		}
		*/
		#endregion
	}
}

