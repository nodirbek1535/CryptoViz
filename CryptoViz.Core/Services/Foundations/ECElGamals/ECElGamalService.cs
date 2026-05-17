using System.Numerics;
using CryptoViz.Core.Brokers.Maths;
using CryptoViz.Core.Models.ECElGamals;

namespace CryptoViz.Core.Services.Foundations.ECElGamals
{
    public partial class ECElGamalService : IECElGamalService
    {
        private readonly IECMathBroker ecMathBroker;
        private readonly IMathBroker mathBroker;

        public ECElGamalService(IECMathBroker ecMathBroker, IMathBroker mathBroker)
        {
            this.ecMathBroker = ecMathBroker;
            this.mathBroker = mathBroker;
        }

        public ECElGamalKeyPair GenerateKeys(BigInteger p, BigInteger a, BigInteger b, ECPoint g, BigInteger? customD = null)
        {
            if (!this.mathBroker.IsPrime(p))
            {
                throw new ArgumentException($"Kiritilgan P ({p}) tub son emas! EC-ElGamal xavfsizligi uchun P albatta tub son bo'lishi shart.");
            }

            // d ni tanlaymiz (yashirin kalit) - qo'lda yoki tasodifiy
            BigInteger d = customD ?? this.mathBroker.GenerateRandomBigInteger(2, p - 1);
            
            // Q = d * G (ochiq kalit)
            ECPoint q = this.ecMathBroker.MultiplyPoint(g, d, a, p);

            return new ECElGamalKeyPair
            {
                P = p,
                A = a,
                B = b,
                G = g,
                PrivateKey = d,
                PublicKey = q
            };
        }

        public ECElGamalCiphertext Encrypt(ECPoint messagePoint, ECElGamalKeyPair keyPair, BigInteger? customK = null)
        {
            // k tasodifiy yoki qo'lda kiritilgan son
            BigInteger k = customK ?? this.mathBroker.GenerateRandomBigInteger(2, keyPair.P - 1);

            // C1 = k * G
            ECPoint c1 = this.ecMathBroker.MultiplyPoint(keyPair.G, k, keyPair.A, keyPair.P);

            // C2 = M + k * Q
            ECPoint kQ = this.ecMathBroker.MultiplyPoint(keyPair.PublicKey, k, keyPair.A, keyPair.P);
            ECPoint c2 = this.ecMathBroker.AddPoints(messagePoint, kQ, keyPair.A, keyPair.P);

            return new ECElGamalCiphertext
            {
                C1 = c1,
                C2 = c2
            };
        }

        public ECPoint Decrypt(ECElGamalCiphertext ciphertext, ECElGamalKeyPair keyPair)
        {
            // d * C1
            ECPoint dc1 = this.ecMathBroker.MultiplyPoint(ciphertext.C1, keyPair.PrivateKey, keyPair.A, keyPair.P);

            // -dc1 ni topamiz (y koordinatasini manfiy qilamiz: P - y mod P)
            ECPoint minusDc1 = new ECPoint 
            { 
                X = dc1.X, 
                Y = (keyPair.P - dc1.Y) % keyPair.P, 
                IsInfinity = dc1.IsInfinity 
            };

            // M = C2 - d * C1
            ECPoint messagePoint = this.ecMathBroker.AddPoints(ciphertext.C2, minusDc1, keyPair.A, keyPair.P);

            return messagePoint;
        }
    }
}
