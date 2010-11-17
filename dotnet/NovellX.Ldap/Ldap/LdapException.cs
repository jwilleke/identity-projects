// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;

namespace NovellX.Ldap
{
	/// <summary>
	/// An exception raised by the LDAP Libraries.
	/// </summary>
	internal class LdapException : Exception
	{
		private const int ErrorCodeDefault = int.MaxValue;
		private const string MessageDefault = "";

		public LdapException(string ldapErrorMessage) 
			: this(ldapErrorMessage, ErrorCodeDefault)
		{
		}

		public LdapException(int ldapErrorCode) 
			: this(MessageDefault, ldapErrorCode)
		{
		}


		public LdapException(string ldapErrorMessage, int ldapErrorCode)
			:base(ldapErrorMessage)
		{
			this.ErrorCode = ldapErrorCode;
		}

		internal int ErrorCode
		{
			get; 
			private set;
		}

	}
}