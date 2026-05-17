using System.Numerics;

namespace CryptoViz.Core.Brokers.Maths
{
    public interface IMathBroker
    {
        BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus);
        BigInteger ModInverse(BigInteger value, BigInteger modulus);
        BigInteger GenerateRandomBigInteger(BigInteger min, BigInteger max);
        bool IsPrime(BigInteger number);
    }
}
