using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Text;

namespace Jmaxxz.X509
{
	public class CertificateStore : IDisposable
	{
		private X509Store _store;

		public IEnumerable<X509Certificate2> Certificates {
			get { return _store.Certificates.Cast<X509Certificate2>(); }
		}

		public CertificateStore (OpenFlags flags, StoreName name, StoreLocation location)
		{
			_store = new X509Store(name, location);
			_store.Open(flags);
		}

		public X509Certificate2 FindByFingerprint (byte[] fingerprint)
		{
			return _store.Certificates.Find(X509FindType.FindByThumbprint, ToHex(fingerprint), false).Cast<X509Certificate2>().FirstOrDefault();
		}

		public IEnumerable<X509Certificate2> FindByIssuer (string issuer, bool validOnly)
		{
			return _store.Certificates.Find(X509FindType.FindByIssuerName, issuer, validOnly).Cast<X509Certificate2>();
		}

		private string ToHex (byte[] data)
		{
			var sb = new StringBuilder();
			foreach (var b in data) {
				sb.Append(b.ToString("x2"));
			}
			return sb.ToString();
		}

		public void Dispose ()
		{
			_store.Close();
		}
	}
}

