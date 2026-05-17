using System.Numerics;

namespace CryptoViz.Core.Models.ECElGamals
{
    public class ECPoint
    {
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }
        public bool IsInfinity { get; set; }
    }
}
