using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plagiarism_Engine.Services
{
    /// <summary>
    /// API keys that are valid
    /// </summary>
    public static class ApiKeyService
    {
        /// <summary>
        /// A list of valid API keys
        /// </summary>
        static List<string> keys = new List<string> { "0e3a3316-8fc7-4a8b-a6a8-84cd8dc56c17" };


        public static bool isValid(string key)
        {
            return keys.Contains(key);
        }
    }
}
