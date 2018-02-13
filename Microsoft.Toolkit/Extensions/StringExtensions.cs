﻿// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Net;
using System.Text.RegularExpressions;

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// All common string extensions should go here
    /// </summary>
    public static class StringExtensions
    {
        internal const string EmailRegex = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";

        /// <summary>
        /// Regular expression of HTML tags to remove.
        /// </summary>
        private const string RemoveHtmlTagsRegex = @"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>";

        /// <summary>
        /// Regular expression for removing comments.
        /// </summary>
        private static readonly Regex RemoveCommentsRegex = new Regex("<!--.*?-->", RegexOptions.Singleline);

        /// <summary>
        /// Regular expression for removing scripts.
        /// </summary>
        private static readonly Regex RemoveScriptsRegex = new Regex(@"(?s)<script.*?(/>|</script>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Regular expression for removing styles.
        /// </summary>
        private static readonly Regex RemoveStylesRegex = new Regex(@"(?s)<style.*?(/>|</style>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Returns whether said string is a valid email or not.
        /// Uses general Email Regex (RFC 5322 Official Standard) from emailregex.com
        /// </summary>
        /// <returns><c>true</c> for valid email.<c>false</c> otherwise</returns>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, EmailRegex);
        }

        /// <summary>
        /// Converts object into string.
        /// </summary>
        /// <param name="value">Object value.</param>
        /// <returns>Returns string value.</returns>
        public static string ToSafeString(this object value)
        {
            return value?.ToString();
        }

        /// <summary>
        /// Decode HTML string.
        /// </summary>
        /// <param name="htmlText">HTML string.</param>
        /// <returns>Returns decoded HTML string.</returns>
        public static string DecodeHtml(this string htmlText)
        {
            if (htmlText == null)
            {
                return null;
            }

            var ret = htmlText.FixHtml();

            // Remove html tags
            ret = new Regex(RemoveHtmlTagsRegex).Replace(ret, string.Empty);

            return WebUtility.HtmlDecode(ret);
        }

        /// <summary>
        /// Applies regular expressions to string of HTML to remove comments, scripts, styles.
        /// </summary>
        /// <param name="html">HTML string to fix</param>
        /// <returns>Fixed HTML string</returns>
        public static string FixHtml(this string html)
        {
            // Remove comments
            var withoutComments = RemoveCommentsRegex.Replace(html, string.Empty);

            // Remove scripts
            var withoutScripts = RemoveScriptsRegex.Replace(withoutComments, string.Empty);

            // Remove styles
            var withoutStyles = RemoveStylesRegex.Replace(withoutScripts, string.Empty);

            return withoutStyles;
        }

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="value">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this string value, int length)
        {
            return Truncate(value, length, false);
        }

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="value">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="ellipsis">if set to <c>true</c> add a text ellipsis.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this string value, int length, bool ellipsis)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                if (value.Length > length)
                {
                    if (ellipsis)
                    {
                        return value.Substring(0, length) + "...";
                    }

                    return value.Substring(0, length);
                }
            }

            return value ?? string.Empty;
        }
    }
}