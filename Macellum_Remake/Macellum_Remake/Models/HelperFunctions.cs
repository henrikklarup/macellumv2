using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Macellum_Remake.Models
{
    public class Helper
    {

        public List<Arter> PerformSwap(List<Arter> allArters, string replaceName, int id)
        {
            var firstOrDefault = allArters.FirstOrDefault(s => s.Name.Normalize().Trim().ToLower() == replaceName.Normalize().Trim().ToLower());
            if (firstOrDefault != null)
            {
                var x = firstOrDefault.Id;
                allArters.Swap(allArters.FindIndex(s => s.Id == x), id);
            }
            return allArters;
        }

        public static string HashPassword(string clearData)
        {
            var encoding = new UnicodeEncoding();
            HashAlgorithm hash = new SHA512Managed();
            const string saltValue = "a4adfd6966852bf2bb364a9958cff9de8f746371c261d72ff08ffef7a7947796";
            var saltValueSize = saltValue.Length;

            if (clearData != null)
            {
                // Convert the salt string and the password string to a single
                // array of bytes. Note that the password string is Unicode and
                // therefore may or may not have a zero in every other byte.

                var binarySaltValue = new byte[saltValueSize];

                binarySaltValue[0] = byte.Parse(saltValue.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[1] = byte.Parse(saltValue.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[2] = byte.Parse(saltValue.Substring(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);
                binarySaltValue[3] = byte.Parse(saltValue.Substring(6, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat);

                var valueToHash = new byte[saltValueSize + encoding.GetByteCount(clearData)];
                var binaryPassword = encoding.GetBytes(clearData);

                // Copy the salt value and the password to the hash buffer.

                binarySaltValue.CopyTo(valueToHash, 0);
                binaryPassword.CopyTo(valueToHash, saltValueSize);

                var hashValue = hash.ComputeHash(valueToHash);

                // The hashed password is the salt plus the hash value (as a string).

                var hashedPassword = saltValue;

// ReSharper disable LoopCanBeConvertedToQuery
                foreach (var hexdigit in hashValue)
// ReSharper restore LoopCanBeConvertedToQuery
                {
                    hashedPassword += hexdigit.ToString("X2", CultureInfo.InvariantCulture.NumberFormat);
                }

                // Return the hashed password as a string.

                return hashedPassword;
            }

            return null;
        }
    }


    static class ListExtensions
    {
        public static void Swap<T>(
            this IList<T> list,
            int firstIndex,
            int secondIndex
        )
        {
            Contract.Requires(list != null);
            Contract.Requires(firstIndex >= 0 && firstIndex < list.Count);
            Contract.Requires(secondIndex >= 0 && secondIndex < list.Count);
            if (firstIndex == secondIndex)
            {
                return;
            }
            T temp = list[firstIndex];
            list[firstIndex] = list[secondIndex];
            list[secondIndex] = temp;
        }
    }
}