using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using System.Xml;

namespace Floss.Api.Auth
{
    /// <summary>
    /// Utility methods for Cryptography
    /// </summary>
    public class Crypto
    {

        /// <summary>
        /// Verify signature in the XML document
        /// </summary>
        /// <param name="xd">XML document</param>
        /// <param name="verifySignatureOnly">If <c>true</c> then only signature is verified; otherwise both signature and signing certificate are verified.</param>
        /// <param name="signatureNs">XML namespace for signature ("http://www.w3.org/2000/09/xmldsig#"). By default it is empty.</param>
        /// <param name="certValidator">Optional signing cetificate validator. If specified it must return true if certificate is considered valid.
        /// This method is independent from <paramref name="verifySignatureOnly"/>, i.e. both checks are applied if specified.
        /// </param>
        /// <returns>bool</returns>
        public static bool VerifyXmlSignature(XmlDocument xd, bool verifySignatureOnly, Func<X509Certificate2, bool> certValidator = null)
        {
            if (xd == null) throw new ArgumentNullException("xd");
            const string signatureNs = "ds";
            SignedXml signedXml = new SignedXml(xd);
            var nsManager = new XmlNamespaceManager(xd.NameTable);
            nsManager.AddNamespace(signatureNs, "http://www.w3.org/2000/09/xmldsig#");
            string pfx = "//" + signatureNs + ":";
            // find signature node
            var node = xd.SelectSingleNode(pfx + "Signature", nsManager);
            if (node == null) return false; // there is no signature
            // find signiong certificate node
            var certElement = node.SelectSingleNode(pfx + "X509Certificate", nsManager);

            var cert = new X509Certificate2(Convert.FromBase64String(certElement.InnerText));
            if (certValidator != null)
            {
                if (!certValidator(cert))
                    return false;
            }
            signedXml.LoadXml((XmlElement)node);
            return signedXml.CheckSignature(cert, verifySignatureOnly);
        }

    }
}
