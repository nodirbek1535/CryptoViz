using System;
using System.Numerics;
using System.Security.Cryptography;

namespace CryptoViz.Core.Brokers.Maths
{
    public class MathBroker : IMathBroker
    {
        public BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
        {
            return BigInteger.ModPow(value, exponent, modulus);
        }

        public BigInteger ModInverse(BigInteger value, BigInteger modulus)
        {
            // Fermaning kichik teoremasidan foydalanib teskari elementni topamiz
            // a^(p-2) mod p = a^(-1) mod p
            return BigInteger.ModPow(value, modulus - 2, modulus);
        }

        public BigInteger GenerateRandomBigInteger(BigInteger min, BigInteger max)
        {
            if (min >= max)
                throw new ArgumentException("Min max dan kichik bo'lishi kerak.");
                
            BigInteger range = max - min;
            byte[] bytes = range.ToByteArray();
            BigInteger randomValue;
            
            using (var rng = RandomNumberGenerator.Create())
            {
                do
                {
                    rng.GetBytes(bytes);
                    bytes[bytes.Length - 1] &= (byte)0x7F; // musbat ekanligini kafolatlash
                    randomValue = new BigInteger(bytes);
                } while (randomValue >= range);
            }
            
            return randomValue + min;
        }

        public bool IsPrime(BigInteger number)
        {
            if (number <= 1) return false;
            if (number == 2 || number == 3) return true;
            if (number % 2 == 0) return false;

            return MillerRabinTest(number, 5);
        }

        private bool MillerRabinTest(BigInteger n, int k)
        {
            BigInteger d = n - 1;
            int s = 0;
            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            for (int i = 0; i < k; i++)
            {
                BigInteger a = GenerateRandomBigInteger(2, n - 1);
                BigInteger x = BigInteger.ModPow(a, d, n);

                if (x == 1 || x == n - 1)
                    continue;

                bool composite = true;
                for (int r = 1; r < s; r++)
                {
                    x = BigInteger.ModPow(x, 2, n);
                    if (x == n - 1)
                    {
                        composite = false;
                        break;
                    }
                }

                if (composite)
                    return false;
            }
            return true;
        }
    }
}
