using System.Numerics;
using CryptoViz.Core.Models.ECElGamals;

namespace CryptoViz.Core.Brokers.Maths
{
    public interface IECMathBroker
    {
        ECPoint AddPoints(ECPoint p, ECPoint q, BigInteger a, BigInteger modulus);
        ECPoint MultiplyPoint(ECPoint p, BigInteger k, BigInteger a, BigInteger modulus);
    }
}
