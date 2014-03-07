using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace BLUE.Mail.DAO
{
    internal static class Utilities
    {
        private static CultureInfo _enUsCulture = CultureInfo.GetCultureInfo("en-US");

        internal static void CopyStream(Stream a, Stream b, int maxLength, int bufferSize = 8192)
        {
            int read;
            var buffer = new byte[bufferSize];
            while (maxLength > 0)
            {
                read = Math.Min(bufferSize, maxLength);
                read = a.Read(buffer, 0, read);
                if (read == 0) return;
                maxLength -= read;
                b.Write(buffer, 0, read);
            }
        }

        public static NameValueCollection ParseImapHeader(string data)
        {
            var values = new NameValueCollection();
            string name = null;
            int nump = 0;
            var temp = new StringBuilder();
            if (data != null)
                foreach (var c in data)
                {
                    if (c == ' ')
                    {
                        if (name == null)
                        {
                            name = temp.ToString();
                            temp.Remove(0, temp.Length);

                        }
                        else if (nump == 0)
                        {
                            values[name] = temp.ToString();
                            name = null;
                            temp.Remove(0, temp.Length);
                        }
                        else
                            temp.Append(c);
                    }
                    else if (c == '(')
                    {
                        if (nump > 0)
                            temp.Append(c);
                        nump++;
                    }
                    else if (c == ')')
                    {
                        nump--;
                        if (nump > 0)
                            temp.Append(c);
                    }
                    else
                        temp.Append(c);
                }

            if (name != null)
                values[name] = temp.ToString();

            return values;
        }

        internal static byte[] Read(Stream stream, int len)
        {
            var data = new byte[len];
            int read, pos = 0;
            while (pos < len && (read = stream.Read(data, pos, len - pos)) > 0)
            {
                pos += read;
            }
            return data;
        }

        internal static string ReadLine(Stream stream, ref int maxLength, Encoding encoding, char? termChar)
        {
            if (stream.CanTimeout)
                stream.ReadTimeout = 10000;

            var maxLengthSpecified = maxLength > 0;
            int i;
            byte b = 0, b0;
            var read = false;
            using (var mem = new MemoryStream())
            {
                while (true)
                {
                    b0 = b;
                    i = stream.ReadByte();
                    if (i == -1) break;
                    else read = true;

                    b = (byte)i;
                    if (maxLengthSpecified) maxLength--;

                    if (maxLengthSpecified && mem.Length == 1 && b == termChar && b0 == termChar)
                    {
                        maxLength++;
                        continue;
                    }

                    if (b == 10 || b == 13)
                    {
                        if (mem.Length == 0 && b == 10)
                        {
                            continue;
                        }
                        else break;
                    }

                    mem.WriteByte(b);
                    if (maxLengthSpecified && maxLength == 0)
                        break;
                }

                if (mem.Length == 0 && !read) return null;
                return encoding.GetString(mem.ToArray());
            }
        }

        internal static string ReadToEnd(Stream stream, int maxLength, Encoding encoding)
        {
            if (stream.CanTimeout)
                stream.ReadTimeout = 10000;

            int read = 1;
            byte[] buffer = new byte[8192];
            using (var mem = new MemoryStream())
            {
                do
                {
                    var length = maxLength == 0 ? buffer.Length : Math.Min(maxLength - (int)mem.Length, buffer.Length);
                    read = stream.Read(buffer, 0, length);
                    mem.Write(buffer, 0, read);
                    if (maxLength > 0 && mem.Length == maxLength) break;
                } while (read > 0);

                return encoding.GetString(mem.ToArray());
            }
        }

        internal static void TryDispose<T>(ref T obj) where T : class, IDisposable
        {
            try
            {
                if (obj != null)
                    obj.Dispose();
            }
            catch (Exception)
            {
            }

            obj = null;
        }

        internal static string NotEmpty(string input, params string[] others)
        {
            if (!string.IsNullOrEmpty(input))
                return input;

            foreach (var item in others)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    return item;
                }
            }
            return string.Empty;
        }

        internal static List<T> ToList<T>(IEnumerable<T> source)
        {
            List<T> list = new List<T>();

            list.AddRange(source);

            return list;
        }

        internal static T FirstOrDefault<T>(IEnumerable<T> list)
        {
            foreach (T item in list)
                return item;

            return default(T);
        }

        internal static T LastOrDefault<T>(IEnumerable<T> list)
        {
            T last = default(T);

            foreach (T item in list)
                last = item;

            return last;
        }

        internal static int ToInt(string input)
        {
            int result;
            if (int.TryParse(input, out result))
            {
                return result;
            }
            else
            {
                return 0;
            }
        }

        internal static IEnumerable<T> Cast<T, TSource>(IEnumerable<TSource> source)
        {
            foreach (var item in source)
                yield return (T)Convert.ChangeType(item, typeof(T));
        }

        internal static DateTime? ToNullDate(string input)
        {
            DateTime result;
            input = NormalizeDate(input);
            if (DateTime.TryParse(input, _enUsCulture, DateTimeStyles.None, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private static Regex rxTimeZoneName = new Regex(@"\s+\([a-z]+\)$", RegexOptions.Compiled | RegexOptions.IgnoreCase); //Mon, 28 Feb 2005 19:26:34 -0500 (EST)
        private static Regex rxTimeZoneColon = new Regex(@"\s+(\+|\-)(\d{1,2})\D(\d{2})$", RegexOptions.Compiled | RegexOptions.IgnoreCase); //Mon, 28 Feb 2005 19:26:34 -0500 (EST)
        private static Regex rxTimeZoneMinutes = new Regex(@"([\+\-]?\d{1,2})(\d{2})$", RegexOptions.Compiled); //search can be strict because the format has already been normalized
        private static Regex rxNegativeHours = new Regex(@"(?<=\s)\-(?=\d{1,2}\:)", RegexOptions.Compiled);

        public static string NormalizeDate(string value)
        {
            value = rxTimeZoneName.Replace(value, string.Empty);
            value = rxTimeZoneColon.Replace(value, match => " " + match.Groups[1].Value + match.Groups[2].Value.PadLeft(2, '0') + match.Groups[3].Value);
            value = rxNegativeHours.Replace(value, string.Empty);
            var minutes = rxTimeZoneMinutes.Match(value);
            if (Utilities.ToInt(minutes.Groups[2].Value) > 60)
            { //even if there's no match, the value = 0
                value = value.Substring(0, minutes.Index) + minutes.Groups[1].Value + "00";
            }
            return value;
        }

        internal static string GetRFC2060Date(DateTime date)
        {
            return date.ToString("dd-MMM-yyyy hh:mm:ss zz", _enUsCulture);
        }

        internal static bool IsNullOrWhiteSpace(string str)
        {
            if (String.IsNullOrEmpty(str) || String.IsNullOrEmpty(str.Trim()))
                return true;

            return false;
        }

        internal static string QuoteString(string value)
        {
            return "\"" + value
                .Replace("\\", "\\\\")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n")
                .Replace("\"", "\\\"") + "\"";
        }

        internal static bool StartsWithWhiteSpace(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;
            var chr = line[0];
            return chr == ' ' || chr == '\t' || chr == '\n' || chr == '\r';
        }

        public static string DecodeQuotedPrintable(string value, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.Default;
            }

            if (value.IndexOf('_') > -1 && value.IndexOf(' ') == -1)
                value = value.Replace('_', ' ');

            var data = System.Text.Encoding.ASCII.GetBytes(value);
            var eq = Convert.ToByte('=');
            var n = 0;

            for (int i = 0; i < data.Length; i++)
            {
                var b = data[i];

                if ((b == eq) && ((i + 2) < data.Length))
                {
                    byte b1 = data[i + 1], b2 = data[i + 2];
                    if (b1 == 10 || b1 == 13)
                    {
                        i++;
                        if (b2 == 10 || b2 == 13)
                        {
                            i++;
                        }
                        continue;
                    }

                    if (byte.TryParse(value.Substring(i + 1, 2), NumberStyles.HexNumber, null, out b))
                    {
                        data[n] = (byte)b;
                        n++;
                        i += 2;
                    }
                    else
                    {
                        data[i] = eq;
                        n++;
                    }

                }
                else
                {
                    data[n] = b;
                    n++;
                }
            }

            value = encoding.GetString(data, 0, n);
            return value;
        }

        internal static string DecodeBase64(string data, Encoding encoding = null)
        {
            if (!IsValidBase64String(ref data))
            {
                return data;
            }
            var bytes = Convert.FromBase64String(data);
            return (encoding ?? System.Text.Encoding.Default).GetString(bytes);
        }

        internal static string DecodeWords(string encodedWords, Encoding @default = null)
        {
            if (string.IsNullOrEmpty(encodedWords))
                return string.Empty;

            string decodedWords = encodedWords;

            // Notice that RFC2231 redefines the BNF to
            // encoded-word := "=?" charset ["*" language] "?" encoded-text "?="
            // but no usage of this BNF have been spotted yet. It is here to
            // ease debugging if such a case is discovered.

            // This is the regex that should fit the BNF
            // RFC Says that NO WHITESPACE is allowed in this encoding, but there are examples
            // where whitespace is there, and therefore this regex allows for such.
            const string strRegEx = @"\=\?(?<Charset>\S+?)\?(?<Encoding>\w)\?(?<Content>.+?)\?\=";
            // \w	Matches any word character including underscore. Equivalent to "[A-Za-z0-9_]".
            // \S	Matches any nonwhite space character. Equivalent to "[^ \f\n\r\t\v]".
            // +?   non-gready equivalent to +
            // (?<NAME>REGEX) is a named group with name NAME and regular expression REGEX

            var matches = Regex.Matches(encodedWords, strRegEx);
            foreach (Match match in matches)
            {
                // If this match was not a success, we should not use it
                if (!match.Success)
                    continue;

                string fullMatchValue = match.Value;

                string encodedText = match.Groups["Content"].Value;
                string encoding = match.Groups["Encoding"].Value;
                string charset = match.Groups["Charset"].Value;

                // Get the encoding which corrosponds to the character set
                Encoding charsetEncoding = ParseCharsetToEncoding(charset, @default);

                // Store decoded text here when done
                string decodedText;

                // Encoding may also be written in lowercase
                switch (encoding.ToUpperInvariant())
                {
                    // RFC:
                    // The "B" encoding is identical to the "BASE64" 
                    // encoding defined by RFC 2045.
                    // http://tools.ietf.org/html/rfc2045#section-6.8
                    case "B":
                        decodedText = DecodeBase64(encodedText, charsetEncoding);
                        break;

                    // RFC:
                    // The "Q" encoding is similar to the "Quoted-Printable" content-
                    // transfer-encoding defined in RFC 2045.
                    // There are more details to this. Please check
                    // http://tools.ietf.org/html/rfc2047#section-4.2
                    // 
                    case "Q":
                        decodedText = DecodeQuotedPrintable(encodedText, charsetEncoding);
                        break;

                    default:
                        throw new ArgumentException("The encoding " + encoding + " was not recognized");
                }

                // Repalce our encoded value with our decoded value
                decodedWords = decodedWords.Replace(fullMatchValue, decodedText);
            }

            return decodedWords;
        }

        /// <summary>
        /// Parse a character set into an encoding.
        /// </summary>
        /// <param name="characterSet">The character set to parse</param>
        /// <param name="@default">The character set to default to if it can't be parsed</param>
        /// <returns>An encoding which corresponds to the character set</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="characterSet"/> is <see langword="null"/></exception>
        public static Encoding ParseCharsetToEncoding(string characterSet, Encoding @default)
        {
            if (string.IsNullOrEmpty(characterSet))
                return @default ?? Encoding.Default;

            string charSetUpper = characterSet.ToUpperInvariant();
            if (charSetUpper.Contains("WINDOWS") || charSetUpper.Contains("CP"))
            {
                // It seems the character set contains an codepage value, which we should use to parse the encoding
                charSetUpper = charSetUpper.Replace("CP", ""); // Remove cp
                charSetUpper = charSetUpper.Replace("WINDOWS", ""); // Remove windows
                charSetUpper = charSetUpper.Replace("-", ""); // Remove - which could be used as cp-1554

                // Now we hope the only thing left in the characterSet is numbers.
                int codepageNumber = int.Parse(charSetUpper, System.Globalization.CultureInfo.InvariantCulture);

                foreach (var encodingInfo in Encoding.GetEncodings())
                {
                    if (encodingInfo.CodePage == codepageNumber)
                        return encodingInfo.GetEncoding() ?? @default ?? Encoding.Default;
                }

                return @default ?? Encoding.Default;
            }

            // It seems there is no codepage value in the characterSet. It must be a named encoding
            foreach (var encodingInfo in Encoding.GetEncodings())
            {
                if (Utilities.Is(encodingInfo.Name, characterSet))
                    return encodingInfo.GetEncoding() ?? @default ?? Encoding.Default;
            }

            return @default ?? Encoding.Default;
        }

        #region IsValidBase64
        private const char Base64Padding = '=';
        private static readonly List<char> Base64Characters = new List<char>
        { 
		    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 
		    'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 
		    'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 
		    'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '/'
		};

        internal static bool IsValidBase64String(ref string param, bool strictPadding = false)
        {
            if (param == null)
            {
                // null string is not Base64 
                return false;
            }

            // replace optional CR and LF characters
            param = param.Replace("\r", String.Empty).Replace("\n", String.Empty);

            var lengthWPadding = param.Length;
            var missingPaddingLength = lengthWPadding % 4;
            if (missingPaddingLength != 0)
            {
                // Base64 string length should be multiple of 4
                if (strictPadding)
                {
                    return false;
                }
                else
                {
                    //add the minimum necessary padding
                    if (missingPaddingLength > 2)
                        missingPaddingLength = missingPaddingLength % 2;
                    param += new string(Base64Padding, missingPaddingLength);
                    lengthWPadding += missingPaddingLength;
                    System.Diagnostics.Debug.Assert(lengthWPadding % 4 == 0);
                }
            }

            if (lengthWPadding == 0)
            {
                // Base64 string should not be empty
                return false;
            }

            // replace pad chacters
            var paramWOPadding = param.TrimEnd(Base64Padding);
            var lengthWOPadding = paramWOPadding.Length;

            if ((lengthWPadding - lengthWOPadding) > 2)
            {
                // there should be no more than 2 pad characters
                return false;
            }

            foreach (char c in paramWOPadding)
            {
                if (!Base64Characters.Contains(c))
                {
                    // string contains non-Base64 character
                    return false;
                }
            }

            // nothing invalid found
            return true;
        }
        #endregion

        internal static TValue Get<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            if (dictionary == null)
                return defaultValue;
            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }

        internal static void Set<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
                lock (dictionary)
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, value);
                        return;
                    }

            dictionary[key] = value;
        }


        internal static void Fire<T>(EventHandler<T> events, object sender, T args) where T : EventArgs
        {
            if (events == null)
                return;

            events(sender, args);
        }

        internal static MailAddress ToEmailAddress(string input)
        {
            try
            {
                return new MailAddress(input);
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static bool Is(string input, string other)
        {
            return string.Equals(input, other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Removes all Diacritics from a string. For example: é -> e
        /// </summary>
        public static String RemoveDiacritics(String value)
        {
            var stringBuilder = new StringBuilder();

            var normalizedString = value.Normalize(NormalizationForm.FormD);

            foreach (var character in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString();
        }
    }
}

