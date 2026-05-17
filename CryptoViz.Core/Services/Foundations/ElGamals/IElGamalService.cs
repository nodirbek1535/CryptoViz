using System.Numerics;
using CryptoViz.Core.Models.ElGamals;

namespace CryptoViz.Core.Services.Foundations.ElGamals
{
    public interface IElGamalService
    {
        ElGamalKeyPair GenerateKeys(BigInteger p, BigInteger g, BigInteger? customX = null);
        ElGamalCiphertext Encrypt(BigInteger message, ElGamalKeyPair keyPair, BigInteger? customK = null);
        BigInteger Decrypt(ElGamalCiphertext ciphertext, ElGamalKeyPair keyPair);
    }
}
