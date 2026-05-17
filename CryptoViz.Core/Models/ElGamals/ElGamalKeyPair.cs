using System.Numerics;

namespace CryptoViz.Core.Models.ElGamals
{
    public class ElGamalKeyPair
    {
        public BigInteger P { get; set; }
        public BigInteger G { get; set; }
        public BigInteger PrivateKey { get; set; } // x
        public BigInteger PublicKey { get; set; }  // h
    }
}
