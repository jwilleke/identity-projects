// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;
using System.IO;

namespace NovellX.Ldap
{
	public class LdapConnection : IDisposable
	{
		private static readonly object SyncRoot = new object();
		private static volatile int _isSslClientInitialized;

		private LdapConnection(IntPtr connectionHandle, bool isSslClientInitialized)
		{
			this.Handle = connectionHandle;
			lock (SyncRoot) {
				if (isSslClientInitialized) // NOTE: We only have InitSsl now, but non-SSL could be added easily, hence the check here.
					_isSslClientInitialized++;
			}
		}

		/// <summary>
		/// The underlying connection handle/pointer used by the native API.
		/// </summary>
		internal IntPtr Handle
		{
			get; 
			private set; 
		}

		/// <summary>
		/// Initializes and SSL connection to the specified host on the specified port.
		/// </summary>
		public static LdapConnection InitSsl(string ldapHost, int port, FileInfo certificateDerFile)
		{
			// initialize the ssl library
			if (certificateDerFile != null) {
				var err = NativeMethods.LdapSsl.client_init(certificateDerFile.FullName, IntPtr.Zero);
				if (err != NativeMethods.Ldap.SUCCESS) {
					var msg = Ldap.NativeMethods.Ldap.err2string(err);
					throw new LdapException("ldapssl_client_init failed with message: '" + msg + "'.", err);
				}
			} // else might be null because they specified the certificate file previously via ldapssl_add_trusted_cert

			const int isSecure = 1;
			IntPtr ld;
			if ((ld = NativeMethods.LdapSsl.init(ldapHost, 636, isSecure)) == IntPtr.Zero) {
				throw new LdapException("ldapssl_init failed");
			}
			return new LdapConnection(ld, true);
		}

		void IDisposable.Dispose()
		{
			Close();
		}

		/// <summary>
		/// Closes the connection.
		/// </summary>
		public void Close()
		{
			//note: It is LdapSsl.init that still requires unbind to be called here. See docs for LdapSsl.init
			NativeMethods.Ldap.unbind(Handle);
			lock (SyncRoot) {
				if (--_isSslClientInitialized == 0) {
					NativeMethods.LdapSsl.client_deinit();
				}
			}
		}

		/// <summary>
		/// Equivelent to the C library ldap_simple_bind_s
		/// </summary>
		public void SimpleBind(string loginDn, string password)
		{
			var err = NativeMethods.Ldap.simple_bind_s(Handle, loginDn, password);
			if (err != NativeMethods.Ldap.SUCCESS) {
				throw new LdapException("ldap_simple_bind_s failed", err);
			}
		}
	}
}
