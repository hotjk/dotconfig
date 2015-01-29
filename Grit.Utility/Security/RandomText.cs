using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grit.Utility.Security
{
    public static class RandomText
    {
        public const string CASE_LETTERS_NUMBERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        public const string LETTERS_NUMBERS = "abcdefghijklmnopqrstuvwxyz0123456789";
        public const string LETTERS = "abcdefghijklmnopqrstuvwxyz";
        public const string NUMBERS = "0123456789";
        public const string FRIENDLY = "qwertypadfhjkcbnm34678";

        public static string Generate(int count, string from = RandomText.FRIENDLY)
        {
            var output = new StringBuilder(10);

            for (int i = 0; i < count; i++)
            {
                var randomIndex = RandomNumber.Next(from.Length - 1);
                output.Append(from[randomIndex]);
            }

            return output.ToString();
        }

        public static string GenerateSalt(int count)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(Generate(count, CASE_LETTERS_NUMBERS)));
        }
    }
}
