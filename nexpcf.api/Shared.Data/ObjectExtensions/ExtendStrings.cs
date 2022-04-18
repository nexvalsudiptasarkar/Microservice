﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ObjectExtensions;

namespace ObjectExtensions
{
    public static class ExtendStrings
    {
        public static bool IsPhone(this string text)
        {
            bool result = true;
            text = text.Replace("(", "").Replace(")", "").Replace("-", "");
            long value;
            result = ((text.Length == 7 || text.Length == 10) && long.TryParse(text, out value));
            return result;
        }

        public static bool IsIn(this string str, string container, bool caseSensitive = true)
        {
            bool result = !string.IsNullOrEmpty(str);
            if (result)
            {
                result = (caseSensitive) ? (str == container) || container.IndexOf(str) >= 0
                                         : str.ToUpper() == container.ToUpper() || container.ToUpper().IndexOf(str.ToUpper()) >= 0;
            }
            return result;
        }

        public static bool IsInExact(this string str, string container)
        {
            string[] parts = container.Split(',');
            var result = parts.Where(x => x == str).FirstOrDefault();
            return (result != null);
        }

        public static bool IsIn(this string str, IEnumerable<string> list, bool caseSensitive = true)
        {
            return (caseSensitive) ? (list.Where(x => x == str).FirstOrDefault() != null)
                                    : (list.Where(x => x.ToUpper() == str.ToUpper()).FirstOrDefault() != null);
        }

        public static bool IsIn<T>(this string str, IEnumerable<T> list, string propertyName, bool caseSensitive = true)
        {
            Type itemType = typeof(T);
            PropertyInfo info = itemType.GetPropertyInfo(propertyName, TypeChecker.String);
            if (info == null)
            {
                throw new InvalidOperationException(string.Format("Named property ({0}) is not a string.", propertyName));
            }
            return (caseSensitive) ? (list.Where(x => info.GetValue(x).ToString() == str).FirstOrDefault() != null)
                                    : (list.Where(x => info.GetValue(x).ToString().ToUpper() == str.ToUpper()).FirstOrDefault() != null);
        }

        public static string FromByteArray(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static byte[] ToByteArray(this string str)
        {
            byte[] result = Encoding.ASCII.GetBytes(str);
            return result;
        }

        public static string Base64Encode(this string text, Encoding encoding = null)
        {
            string value = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                encoding = (encoding == null) ? Encoding.UTF8 : encoding;
                byte[] bytes = encoding.GetBytes(text);
                value = Convert.ToBase64String(bytes);
            }
            return value;
        }

        public static string Base64Decode(this string text, Encoding encoding = null)
        {
            string value = string.Empty;
            byte[] bytes;
            if (!string.IsNullOrEmpty(text))
            {
                encoding = (encoding == null) ? Encoding.UTF8 : encoding;
                try
                {
                    bytes = Convert.FromBase64String(text);
                    value = encoding.GetString(bytes);
                }
                catch (Exception)
                {
                    value = text;
                }
            }
            return value;
        }

        public static bool IsLike(this string str, string pattern)
        {
            bool isMatch = true;
            bool isWildCardOn = false;
            bool isCharWildCardOn = false;
            bool isCharSetOn = false;
            bool isNotCharSetOn = false;
            bool endOfPattern = false;
            int lastWildCard = -1;
            int patternIndex = 0;
            char p = '\0';
            List<char> set = new List<char>();

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                endOfPattern = (patternIndex >= pattern.Length);
                if (!endOfPattern)
                {
                    p = pattern[patternIndex];
                    if (!isWildCardOn && p == '%')
                    {
                        lastWildCard = patternIndex;
                        isWildCardOn = true;
                        while (patternIndex < pattern.Length && pattern[patternIndex] == '%')
                        {
                            patternIndex++;
                        }
                        p = (patternIndex >= pattern.Length) ? '\0' : p = pattern[patternIndex];
                    }
                    else if (p == '_')
                    {
                        isCharWildCardOn = true;
                        patternIndex++;
                    }
                    else if (p == '[')
                    {
                        if (pattern[++patternIndex] == '^')
                        {
                            isNotCharSetOn = true;
                            patternIndex++;
                        }
                        else
                        {
                            isCharSetOn = true;
                        }

                        set.Clear();
                        if (pattern[patternIndex + 1] == '-' && pattern[patternIndex + 3] == ']')
                        {
                            char start = char.ToUpper(pattern[patternIndex]);
                            patternIndex += 2;
                            char end = char.ToUpper(pattern[patternIndex]);
                            if (start <= end)
                            {
                                for (char ci = start; ci <= end; ci++)
                                {
                                    set.Add(ci);
                                }
                            }
                            patternIndex++;
                        }

                        while (patternIndex < pattern.Length && pattern[patternIndex] != ']')
                        {
                            set.Add(pattern[patternIndex]);
                            patternIndex++;
                        }
                        patternIndex++;
                    }
                }

                if (isWildCardOn)
                {
                    if (char.ToUpper(c) == char.ToUpper(p))
                    {
                        isWildCardOn = false;
                        patternIndex++;
                    }
                }
                else if (isCharWildCardOn)
                {
                    isCharWildCardOn = false;
                }
                else if (isCharSetOn || isNotCharSetOn)
                {
                    bool charMatch = (set.Contains(char.ToUpper(c)));
                    if ((isNotCharSetOn && charMatch) || (isCharSetOn && !charMatch))
                    {
                        if (lastWildCard >= 0)
                        {
                            patternIndex = lastWildCard;
                        }
                        else
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    isNotCharSetOn = isCharSetOn = false;
                }
                else
                {
                    if (char.ToUpper(c) == char.ToUpper(p))
                    {
                        patternIndex++;
                    }
                    else
                    {
                        if (lastWildCard >= 0)
                        {
                            patternIndex = lastWildCard;
                        }
                        else
                        {
                            isMatch = false;
                            break;
                        }
                    }
                }
            }
            endOfPattern = (patternIndex >= pattern.Length);

            if (isMatch && !endOfPattern)
            {
                bool isOnlyWildCards = true;
                for (int i = patternIndex; i < pattern.Length; i++)
                {
                    if (pattern[i] != '%')
                    {
                        isOnlyWildCards = false;
                        break;
                    }
                }
                if (isOnlyWildCards)
                {
                    endOfPattern = true;
                }
            }
            return (isMatch && endOfPattern);
        }

        public static int IndexOfFirstNumber(this string str)
        {
            //int index = -1;
            //string numbers = "0123456789";
            //foreach(char ch in str)
            //{
            //	if (ch.ToString().IsIn(numbers))
            //	{
            //		index = str.IndexOf(ch);
            //	}
            //}
            string numbers = "0123456789";
            int index = str.IndexOfAny(numbers.ToArray());
            return index;
        }

        public static int IndexOfLastNumber(this string str)
        {
            //int index = -1;
            //string numbers = "0123456789";
            //foreach(char ch in str)
            //{
            //	if (ch.ToString().IsIn(numbers))
            //	{
            //		index = str.IndexOf(ch);
            //	}
            //}
            string numbers = "0123456789";
            int index = str.LastIndexOfAny(numbers.ToArray());
            return index;
        }

        public static int IndexOfFirstAlpha(this string str, bool caseSensitive = false)
        {
            //int index = -1;
            //string alphas = "abcdefghijklmnopqrstuvwxyz";
            //foreach(char ch in str)
            //{
            //	if (ch.ToString().IsIn(alphas))
            //	{
            //		index = str.IndexOf(ch);
            //	}
            //}
            string alphas = "abcdefghijklmnopqrstuvwxyz";
            int index = str.IndexOfAny(alphas.ToArray());
            return index;
        }
        public static int IndexOfLastAlpha(this string str, bool caseSensitive = false)
        {
            string alphas = "abcdefghijklmnopqrstuvwxyz";
            int index = str.LastIndexOfAny(alphas.ToArray());
            return index;
        }

        public static int IndexOfFirstSpecial(this string str)
        {
            string alphas = @"~`!@#$%^&*()_+-=[]\{}|;':"",./<>?";
            int index = str.IndexOfAny(alphas.ToArray());
            return index;
        }
        public static int IndexOfLastSpecial(this string str)
        {
            string specials = @"~`!@#$%^&*()_+-=[]\{}|;':"",./<>?";
            int index = str.LastIndexOfAny(specials.ToArray());
            return index;
        }

        public static bool IsInteger(this string str)
        {
            bool result = false;
            int value;
            result = int.TryParse(str, out value);
            return result;
        }

        public static bool IsDouble(this string str)
        {
            bool result = false;
            double value;
            result = double.TryParse(str, out value);
            return result;
        }

        public static bool IsDecimal(this string str)
        {
            bool result = false;
            decimal value;
            result = decimal.TryParse(str, out value);
            return result;
        }

        public static bool IsNumeric(this string str)
        {
            bool result = false;
            result = str.IsInteger() || str.IsDouble() || str.IsDecimal();
            return result;
        }

        public static List<string> SplitQuotedString(this string text, char splitter, int lineno = 0)
        {
            List<string> result = new List<string>();
            bool inQuotedString = false;
            string[] parts = text.Split(splitter);
            StringBuilder field = new StringBuilder();
            foreach (string part in parts)
            {
                if (part.StartsWith("\""))
                {
                    inQuotedString = true;
                    field.AppendFormat("{0}, ", part);
                    if (part.EndsWith("\""))
                    {
                        inQuotedString = false;
                        result.Add(field.ToString());
                        field.Clear();
                    }
                }
                else if (part.EndsWith("\""))
                {
                    field.Append(part);
                    inQuotedString = false;
                    result.Add(field.ToString());
                    field.Clear();
                }
                else
                {
                    field.Append(part);
                    if (!inQuotedString)
                    {
                        result.Add(field.ToString());
                        field.Clear();
                    }
                    else
                    {
                        field.Append(", ");
                    }
                }
            }
            return result;
        }

        public static string Compress(this string text)
        {
            string result = text;
            for (int i = 1; i <= 32; i++)
            {
                result = result.Replace(((char)i).ToString(), "");
            }
            return result;
        }

        public static string CamelCaseWord(this string text)
        {
            string result = text;
            if (!string.IsNullOrEmpty(result))
            {
                result = result.ToLower();
                result = string.Format("{0}{1}", result[0].ToString().ToUpper(), (result.Length > 1) ? result.Substring(1) : string.Empty);
            }
            return result;
        }

        public static string CamelCase(this string text)
        {
            StringBuilder result = new StringBuilder();
            if (text.Length > 0)
            {
                string[] parts = text.Split(' ');
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].CamelCaseWord();
                    result.AppendFormat("{0} ", parts[i]);
                }
            }
            return result.ToString().Trim();
        }

        public static string TrimForDB(this string text, int length, bool trimWhiteSpace = true)
        {
            string result = (trimWhiteSpace) ? text.Trim() : text;
            if (result.Length > length)
            {
                result = result.Substring(0, length);
            }
            return result;
        }

        public static List<string> IdentifyPattern(this string text)
        {
            List<string> patternList = new List<string>();
            foreach (char ch in text)
            {
                if (patternList.Count == 0 || !patternList.Last().Contains(ch))
                {
                    patternList.Add(ch.ToString());
                }
                else
                {
                    patternList[patternList.Count - 1] += ch.ToString();
                }
            }
            return patternList;
        }

        public static bool StartsWithSingleDoubleQuote(this string text)
        {
            bool result = false;
            switch (text.Length)
            {
                case 0: result = false; break;
                case 1:
                case 2:
                case 3: result = (text[0] == '"'); break;
                default: result = (text.StartsWith("\"") && text[1] != '"'); break;
            }
            return result;
        }

        public static bool EndsWithSingleDoubleQuote(this string text)
        {
            bool result = false;
            switch (text.Length)
            {
                case 0:
                case 1:
                case 2:
                case 3: result = false; break;
                default: result = (text.EndsWith("\"") && text[text.Length - 2] != '"'); break;
            }
            return result;
        }
        public static string TrimAll(this string text, char ch)
        {
            return text.TrimAll(new char[] { ch });
        }
        public static string TrimAll(this string text, char[] charArray)
        {
            string result = text.Trim();
            int index = -1;
            do
            {
                index++;
                result = result.Replace(charArray[index].ToString(), "");
            } while (!string.IsNullOrEmpty(result) && index < charArray.Length - 1 && result.Contains(charArray[index]));
            return result;
        }

        public static bool EscapedWithQuotes(this string text)
        {
            bool result = (text.StartsWithSingleDoubleQuote() && text.EndsWithSingleDoubleQuote());
            return result;
        }

        public static MailAddressCollection ToAddresses(this string text)
        {
            string[] parts = text.Split(';');
            MailAddressCollection list = new MailAddressCollection();
            foreach (string part in parts)
            {
                list.Add(new MailAddress(part));
            }
            return list;
        }

        public static string Reverse(this string text)
        {
            if (text.Length > 1)
            {
                int pivotPos = text.Length / 2;
                for (int i = 0; i < pivotPos; i++)
                {
                    text = text.Insert(text.Length - i, text.Substring(i, 1)).Remove(i, 1);
                    text = text.Insert(i, text.Substring(text.Length - (i + 2), 1)).Remove(text.Length - (i + 1), 1);
                }
            }
            return text;
        }

        public static string GetNumberPostfix(int number)
        {
            string result = "";
            int num = number;
            if (num >= 0)
            {
                switch (num % 100)
                {
                    case 11:
                    case 12:
                    case 13: num = 4; break;
                }
                switch (num % 10)
                {
                    case 1: result = "st"; break;
                    case 2: result = "nd"; break;
                    case 3: result = "rd"; break;
                    default: result = "th"; break;
                }
            }
            return result;
        }

        public static string ReplaceAll(this string text, string oldchars, string newchars)
        {
            string newText = text;
            do
            {
                newText = newText.Replace(oldchars, newchars);
            } while (newText.Contains(oldchars));
            return newText;
        }

        public static string ReplaceNonAlphaNumeric(this string text, char newChar)
        {
            StringBuilder newText = new StringBuilder(text);
            for (int i = 0; i < newText.Length; i++)
            {
                if (!newText[i].IsAsciiNumeric() && !newText[i].IsAsciiAlpha())
                {
                    newText[i] = newChar;
                }
            }
            return newText.ToString();
        }

        public static string ReplaceNonAlphaNumeric(this string text, string newChars)
        {
            string newText = text;
            for (int i = 0; i < newText.Length; i++)
            {
                if (!newText[i].IsAsciiNumeric() && !newText[i].IsAsciiAlpha())
                {
                    newText = newText.Replace(new String(newText[i], 1), newChars);
                }
            }
            return newText.ToString();
        }

        public static string Replace(this string text, string oldChars, char newChar)
        {
            string newText = text.Replace(oldChars, new String(newChar, 1));
            return newText;
        }

        public static string Replace(this string text, char oldChar, string newChars)
        {
            string newText = text.Replace(new String(oldChar, 1), newChars);
            return newText;
        }

        public static string ParseConnectionString(this string text, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(text))
            {
                string[] parts = text.Split(';');
                string component = string.Empty;
                if (key.IsLike("Database") || key.IsLike("Initial catalog'"))
                {
                    component = parts.FirstOrDefault(x => x.IsLike("Database=%") || x.IsLike("Initial catalog=%'"));
                }
                else if (key.IsLike("Server") || key.IsLike("Data Source"))
                {
                    component = parts.FirstOrDefault(x => x.IsLike("Server=%") || x.IsLike("Data Source=%'"));
                }
                else
                {
                    component = parts.FirstOrDefault(x => x.IsLike(key));
                }

                if (!string.IsNullOrEmpty(component))
                {
                    string[] subparts = component.Split('=');
                    if (subparts.Length == 2)
                    {
                        result = subparts[1].Trim();
                    }
                }
            }
            return result;
        }

        public static string ParseConnectionStringEx(this string text, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(text) && (key.IsLike("Server") || key.IsLike("Instance")))
            {
                string[] parts = text.Split(';');

                string component = parts.FirstOrDefault(x => x.IsLike("Server=%") || x.IsLike("Data Source=%"));

                if (!string.IsNullOrEmpty(component))
                {
                    string[] subparts = component.Split('=');
                    if (subparts.Length == 2)
                    {
                        result = subparts[1].Trim();
                        int slashPos = result.IndexOf(@"\");
                        result = key.IsLike("Server") ? result.Substring(0, slashPos) : result.Substring(slashPos + 1);
                    }
                }
            }
            return result;
        }
    }
}
