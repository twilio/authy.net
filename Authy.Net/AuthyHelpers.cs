using System;
using System.Reflection;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Authy.Net
{
    public class AuthyHelpers
    {

        ///<summary>
        /// Remove all non-digits from the string
        /// </summary>
        ///<param name="value">The string to sanitize</param>
        public static string SanitizeNumber(string value) {
            return Regex.Replace(value, @"\D", string.Empty);
        }

        ///<summary>
        /// Validate the token entered by the user
        /// </summary>
        /// <param name="token">The token to validate</param>
        public static bool TokenIsValid(string token) {
            token = SanitizeNumber(token);

            if (token.Length < 6 || token.Length > 10) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the version of the Assembly.
        /// </summary>
        /// <returns>The version.</returns>
        public static string GetVersion() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fvi.ProductVersion;

            return version;
        }

        /// <summary>
        /// Get the system information
        /// </summary>
        /// <returns>The .NET version</returns>
        public static string GetSystemInfo() {
            var runtimeVersion = Environment.Version;
            return string.Format("Runtime v{0}", runtimeVersion);
        }

    }
}

