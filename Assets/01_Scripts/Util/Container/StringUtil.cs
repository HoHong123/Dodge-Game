using System;
using System.Text;
using System.Collections.Generic;

namespace Util.Container {
    public static class StringUtil {
        public static Dictionary<string, string> ParseQueryString(string query) {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();

            query = query.TrimStart('?');
            string[] pairs = query.Split('&');
            foreach (string pair in pairs) {
                string[] keyValue = pair.Split('=');
                if (keyValue.Length == 2) {
                    string key = Uri.UnescapeDataString(keyValue[0]);
                    string value = Uri.UnescapeDataString(keyValue[1]);
                    queryParams[key] = value;
                }
            }

            return queryParams;
        }

        public static string EncodeStringToBase64(string plainText) {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            string base64EncodedData = Convert.ToBase64String(plainTextBytes);
            return base64EncodedData;
        }

        public static string DecodeBase64ToString(string base64EncodedData) {
            byte[] decodedBytes = Convert.FromBase64String(base64EncodedData);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);
            return decodedString;
        }

        public static string FormatNumber<T>(T number) where T : struct, IFormattable {
            return number.ToString("N0", null);
        }

        public static string NumToAlpha<T>(T number, bool useRound = false, bool useKoreanUnit = false) where T : struct, IConvertible {
            double num = Convert.ToDouble(number);

            double Format(double value) {
                double temp = value * 10;
                return useRound ? Math.Round(temp) / 10.0 : Math.Floor(temp) / 10.0;
            }

            if (num >= 1_000_000_000_000)
                return $"{Format(num / 1_000_000_000_000.0):0.0}" + (useKoreanUnit ? "조" : "T");
            else if (num >= 1_000_000_000)
                return $"{Format(num / 1_000_000_000.0):0.0}" + (useKoreanUnit ? "십억" : "B");
            else if (num >= 1_000_000)
                return $"{Format(num / 1_000_000.0):0.0}" + (useKoreanUnit ? "백만" : "M");
            else if (num >= 10_000)
                return $"{Format(num / 1_000.0):0.0}" + (useKoreanUnit ? "천" : "K");

            return num.ToString("0");
        }
    }
}