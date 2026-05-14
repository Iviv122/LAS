using System.Numerics;
using System.Text;
namespace LAS.Lib
{

    public static class Base62
    {
        private const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static BigInteger Decode(string encoded)
        {
            if (string.IsNullOrEmpty(encoded))
                return 0;

            var bigInteger = System.Numerics.BigInteger.Zero;

            foreach (char c in encoded)
            {
                int value = Alphabet.IndexOf(c);
                if (value < 0)
                    throw new ArgumentException($"Invalid Base62 character: {c}");

                bigInteger = bigInteger * 62 + value;
            }

            return bigInteger;
        }

        public static string Encode(long bigInteger)
        {
            if (bigInteger == 0)
                return Alphabet[0].ToString();

            var result = new StringBuilder();

            while (bigInteger > 0)
            {
                long rem = bigInteger % 62;
                bigInteger/=62;
                result.Insert(0, Alphabet[(int)rem]);
            }

            return result.ToString();
        }
    }

}