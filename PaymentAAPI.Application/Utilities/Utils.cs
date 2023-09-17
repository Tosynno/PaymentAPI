using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAPI.Application.Utilities
{
    public static class Utils
    {
        public static string GenerateMarchantNumber()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            var randomNumber = rand.Next(100000, 999999);
            int generateCode = randomNumber;

            var codeFormat = "0905" + generateCode.ToString();

            return codeFormat;

        }
        public static string GenerateTranId()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            var randomNumber = rand.Next(100, 999);
            int generateCode = randomNumber;

            var codeFormat = "MT" + generateCode.ToString();

            return codeFormat;

        }
        public static string GeneratePart_tran_srl_num()
        {
            Random rand = new Random();
            var randomNumber = rand.Next(100, 999);
            int generateCode = randomNumber;

            var codeFormat = generateCode.ToString();

            return codeFormat;

        }
        
        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.PadLeft(str.Length - postFix.Length);
                }
            }

            return str;
        }
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }
    }
}
