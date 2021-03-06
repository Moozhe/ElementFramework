﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO.Imap
{
    public static class ModifiedUtf7Encoding
    {
        /// <summary>
        /// Decodes modified UTF-7 according to RFC 3501 5.1.3: Mailbox International Naming Convention
        /// </summary>
        public static string Decode(string input)
        {
            if (Utilities.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            string result = input.Replace("&-", "&");

            for (int indexOfAmpersand = result.IndexOf('&'); indexOfAmpersand != -1; indexOfAmpersand = result.IndexOf('&', indexOfAmpersand + 1))
            {
                int indexOfMinus = result.IndexOf('-', indexOfAmpersand);
                if (indexOfMinus > 0)
                {
                    string substring = result.Substring(indexOfAmpersand + 1, indexOfMinus - indexOfAmpersand - 1);
                    string modifiedBase64 = "+" + substring.Replace(',', '/');
                    result = result.Replace("&" + substring + "-", Encoding.UTF7.GetString(Encoding.UTF8.GetBytes(modifiedBase64)));
                }
            }

            return result;
        }

        /// <summary>
        /// Encodes to modified UTF-7 according to RFC 3501 5.1.3: Mailbox International Naming Convention
        /// </summary>
        public static string Encode(string input)
        {
            if (Utilities.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            bool isPrintableAscii = true;
            foreach (char c in input)
            {
                if (!IsPrintableAscii(c))
                {
                    isPrintableAscii = false;
                    break;
                }
            }

            if (isPrintableAscii)
                return input.Replace("&", "&-");

            var result = new StringBuilder();
            var nonAsciiBuffer = new StringBuilder();
            foreach (char c in input)
            {
                if (IsPrintableAscii(c))
                {
                    if (nonAsciiBuffer.Length > 0)
                    {
                        result.Append(EncodeNonPrintableAsciiString(nonAsciiBuffer.ToString()));
                        nonAsciiBuffer.Remove(0, nonAsciiBuffer.Length);
                    }

                    if (c == '&')
                    {
                        result.Append("&-");
                    }
                    else
                    {
                        result.Append(c);
                    }
                }
                else
                {
                    nonAsciiBuffer.Append(c);
                }
            }

            if (result.Length == 0 && nonAsciiBuffer.Length > 0)
            {
                result.Append(EncodeNonPrintableAsciiString(nonAsciiBuffer.ToString()));
            }

            return result.ToString();
        }

        private static string EncodeNonPrintableAsciiString(string nonAsciiString)
        {
            return Encoding.UTF8.GetString(Encoding.UTF7.GetBytes(nonAsciiString)).Replace('/', ',').Replace('+', '&');
        }

        private static bool IsPrintableAscii(char c)
        {
            return c >= '\x20' && c <= '\x7e';
        }
    }

}
