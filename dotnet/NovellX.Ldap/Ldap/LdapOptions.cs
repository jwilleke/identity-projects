// Authors:
// Scott Willeke (scott@willeke.com)
//
// Copyright (c) 2010 Scott Willeke (scott@willeke.com)
// Licensed under the MIT license: http://www.opensource.org/licenses/mit-license.php
using System;

namespace NovellX.Ldap
{
	/// <summary>
	/// Used to set options for the LDAP library.
	/// </summary>
	public static class LdapOptions
	{
		public static readonly int Version3 = 3;

		public static void SetOption(Option ldapOption, int intValue)
		{
			var retVal = NativeMethods.Ldap.set_option(IntPtr.Zero, ldapOption, intValue);
			if (NativeMethods.Ldap.SUCCESS != retVal)
				throw new LdapException(retVal);
		}

		#region Option Enum
		/// <summary>
		/// Options used with <see cref="NovellX.Ldap.NativeMethods.Ldap.set_option" /> or <see cref="NovellX.Ldap.LdapOptions"/>.
		/// </summary>
		public enum Option : uint
		{
			/*
			 * LDAP_OPTions defined by draft-ldapext-ldap-c-api-02
			 * 0x0000 - 0x0fff reserved for api options
			 * 0x1000 - 0x3fff reserved for api extended options
			 * 0x4000 - 0x7fff reserved for private and experimental options
			 */
			API_INFO = 0x0000,
			DESC = 0x0001 /* deprecated */,
			DEREF = 0x0002,
			SIZELIMIT = 0x0003,
			TIMELIMIT = 0x0004,
			/* 0x05 - 0x07 not defined by current draft */
			REFERRALS = 0x0008,
			RESTART = 0x0009,
			/* 0x0a - 0x10 not defined by current draft */
			PROTOCOL_VERSION = 0x0011,
			SERVER_CONTROLS = 0x0012,
			CLIENT_CONTROLS = 0x0013,
			/* 0x14 not defined by current draft */
			API_FEATURE_INFO = 0x0015,

			/* 0x16 - 0x2f not defined by current draft */
			HOST_NAME = 0x0030,
			RESULT_CODE = 0x0031	/* Updated for C API draft 5 */,
			ERROR_NUMBER = 0x0031,
			ERROR_STRING = 0x0032,
			MATCHED_DN = 0x0033,
			/* 0x34 - 0x0fff not defined by current draft */


			/* private and experimental options */
			/* OpenLDAP specific options */
			DEBUG_LEVEL = 0x5001	/* debug level */,
			TIMEOUT = 0x5002	/* default timeout */,
			REFHOPLIMIT = 0x5003	/* ref hop limit */,
			NETWORK_TIMEOUT = 0x5005  /* socket level timeout */,
			URI = 0x5006,
			REFERRAL_LIST = 0x5007  /* Referrals from LDAP Result msg */,

			/* IO functions options */
			IO_FUNCS = 0x7001,
			PEER_NAME = 0x7002,
			CURRENT_NAME = 0x7003,

			/*thread safe */
			SESSION_REFCNT = 0x8001,

			/* CIPHER related */
			TLS_CIPHER_LEVEL = 0x9001,

			/* Values for settting the CIPHER */
			CIPHER_LOW = 0x01,
			CIPHER_MEDIUM = 0x02,
			CIPHER_HIGH = 0x03,
			CIPHER_EXPORT = 0x04,

			/* on/off values */
			OPT_ON = 1,
			OPT_OFF = 0,

			/* OpenLDAP TLS options */
			X_TLS = 0x6000,
			X_TLS_HARD = 1
		}
		#endregion
	}
}