using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppSheetProject
{
    public static class Utils
    {
        public static bool IsValidPhoneNo(string phoneNumber)
        {
            // This is just a simple validation to make sure that the phone number has 10 digits and consider
            // delimiters as optional
            // Below will consider (, ), - and space as optional and will return true if there are 9 digits.
            Match match = Regex.Match(phoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$");

            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string ReadSetting(string serviceUrlKey)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[serviceUrlKey] ;
                return result;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new InvalidOperationException("Error reading app settings for serviceUrlKey", ex);
            }
        }

        public static Task ForEachAsync<T>(this IEnumerable<T> source, int dop, Func<T, Task> body)
        {
            return Task.WhenAll(
                from partition in Partitioner.Create(source).GetPartitions(dop)
                select Task.Run(async delegate
                {
                    using (partition)
                        while (partition.MoveNext())
                            await body(partition.Current);
                }));
        }
    }
}
