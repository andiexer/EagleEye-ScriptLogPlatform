using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.Utils
{
    public class TransactionUtil
    {
        private const string TransactionPrefix = "TID";
        private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new Random();

        public static string CreateTransactionId()
        {
            var unixTimeStamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString();
            var randomString = new string(Enumerable.Repeat(Chars, 5).Select(s => s[Random.Next(s.Length)]).ToArray());
            return $"{TransactionPrefix}-{unixTimeStamp}-{randomString}";

        }
    }
}
