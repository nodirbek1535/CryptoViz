using System;
using System.Numerics;
using CryptoViz.Core.Models.ECElGamals;

namespace CryptoViz.Core.Brokers.Maths
{
    public class ECMathBroker : IECMathBroker
    {
        public ECPoint AddPoints(ECPoint p, ECPoint q, BigInteger a, BigInteger modulus)
        {
            if (p.IsInfinity) return q;
            if (q.IsInfinity) return p;

            BigInteger lambda;

            if (p.X == q.X && p.Y == q.Y)
            {
                if (p.Y == 0) return new ECPoint { IsInfinity = true };

                // Ikkilantirish: lambda = (3x^2 + a) * (2y)^-1 mod p
                BigInteger numerator = (3 * p.X * p.X + a) % modulus;
                BigInteger denominator = (2 * p.Y) % modulus;
                if (denominator < 0) denominator += modulus;

                lambda = (numerator * ModInverse(denominator, modulus)) % modulus;
            }
            else
            {
                if (p.X == q.X) return new ECPoint { IsInfinity = true };

                // Qo'shish: lambda = (y2 - y1) * (x2 - x1)^-1 mod p
                BigInteger numerator = (q.Y - p.Y) % modulus;
                if (numerator < 0) numerator += modulus;
                
                BigInteger denominator = (q.X - p.X) % modulus;
                if (denominator < 0) denominator += modulus;

                lambda = (numerator * ModInverse(denominator, modulus)) % modulus;
            }

            if (lambda < 0) lambda += modulus;

            BigInteger xr = (lambda * lambda - p.X - q.X) % modulus;
            if (xr < 0) xr += modulus;

            BigInteger yr = (lambda * (p.X - xr) - p.Y) % modulus;
            if (yr < 0) yr += modulus;

            return new ECPoint { X = xr, Y = yr, IsInfinity = false };
        }

        public ECPoint MultiplyPoint(ECPoint p, BigInteger k, BigInteger a, BigInteger modulus)
        {
            ECPoint r = new ECPoint { IsInfinity = true };
            ECPoint q = p;

            while (k > 0)
            {
                if ((k & 1) == 1)
                {
                    r = AddPoints(r, q, a, modulus);
                }
                q = AddPoints(q, q, a, modulus);
                k >>= 1;
            }

            return r;
        }

        private BigInteger ModInverse(BigInteger value, BigInteger modulus)
        {
            return BigInteger.ModPow(value, modulus - 2, modulus);
        }
    }
}
