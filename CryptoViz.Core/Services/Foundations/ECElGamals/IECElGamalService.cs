using System.Numerics;
using CryptoViz.Core.Models.ECElGamals;

namespace CryptoViz.Core.Services.Foundations.ECElGamals
{
    public interface IECElGamalService
    {
        ECElGamalKeyPair GenerateKeys(BigInteger p, BigInteger a, BigInteger b, ECPoint g, BigInteger? customD = null);
        ECElGamalCiphertext Encrypt(ECPoint messagePoint, ECElGamalKeyPair keyPair, BigInteger? customK = null);
        ECPoint Decrypt(ECElGamalCiphertext ciphertext, ECElGamalKeyPair keyPair);
    }
}
