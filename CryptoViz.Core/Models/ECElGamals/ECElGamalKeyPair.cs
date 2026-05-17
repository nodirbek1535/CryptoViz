using System.Numerics;

namespace CryptoViz.Core.Models.ECElGamals
{
    public class ECElGamalKeyPair
    {
        // y^2 = x^3 + Ax + B mod P
        public BigInteger P { get; set; }
        public BigInteger A { get; set; }
        public BigInteger B { get; set; }
        
        public ECPoint G { get; set; }          // Boshlang'ich nuqta
        public BigInteger PrivateKey { get; set; } // d
        public ECPoint PublicKey { get; set; }  // Q = d * G
    }
}
