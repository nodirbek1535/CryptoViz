using System.Numerics;
using CryptoViz.Core.Brokers.Maths;
using CryptoViz.Core.Models.ElGamals;

namespace CryptoViz.Core.Services.Foundations.ElGamals
{
    public partial class ElGamalService : IElGamalService
    {
        private readonly IMathBroker mathBroker;

        public ElGamalService(IMathBroker mathBroker)
        {
            this.mathBroker = mathBroker;
        }

        public ElGamalKeyPair GenerateKeys(BigInteger p, BigInteger g, BigInteger? customX = null)
        {
            if (!this.mathBroker.IsPrime(p))
            {
                throw new ArgumentException($"Kiritilgan P ({p}) tub son emas! ElGamal xavfsizligi uchun P albatta tub son bo'lishi shart.");
            }

            // x ni foydalanuvchi kiritgan bo'lsa o'shani olamiz, aks holda tasodifiy
            BigInteger x = customX ?? this.mathBroker.GenerateRandomBigInteger(2, p - 1);
            
            // h = g^x mod p (ochiq kalit)
            BigInteger h = this.mathBroker.ModPow(g, x, p);

            return new ElGamalKeyPair
            {
                P = p,
                G = g,
                PrivateKey = x,
                PublicKey = h
            };
        }

        public ElGamalCiphertext Encrypt(BigInteger message, ElGamalKeyPair keyPair, BigInteger? customK = null)
        {
            if (message >= keyPair.P)
            {
                throw new ArgumentException($"Ochiq matn (M={message}) moduldan (P={keyPair.P}) kichik bo'lishi shart!");
            }

            // k ni foydalanuvchi kiritgan bo'lsa o'shani olamiz, aks holda tasodifiy
            BigInteger k = customK ?? this.mathBroker.GenerateRandomBigInteger(2, keyPair.P - 1);
            
            // c1 = g^k mod p
            BigInteger c1 = this.mathBroker.ModPow(keyPair.G, k, keyPair.P);
            
            // c2 = m * h^k mod p
            BigInteger hK = this.mathBroker.ModPow(keyPair.PublicKey, k, keyPair.P);
            BigInteger c2 = (message * hK) % keyPair.P;

            return new ElGamalCiphertext
            {
                C1 = c1,
                C2 = c2
            };
        }

        public BigInteger Decrypt(ElGamalCiphertext ciphertext, ElGamalKeyPair keyPair)
        {
            // s = c1^x mod p
            BigInteger s = this.mathBroker.ModPow(ciphertext.C1, keyPair.PrivateKey, keyPair.P);
            
            // s^-1 mod p
            BigInteger sInverse = this.mathBroker.ModInverse(s, keyPair.P);
            
            // m = c2 * s^-1 mod p
            BigInteger message = (ciphertext.C2 * sInverse) % keyPair.P;

            return message;
        }
    }
}
