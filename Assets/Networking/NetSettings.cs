using System.Collections.Generic;

namespace Networking
{
    public static class NetSettings
    {
        public static readonly Dictionary<string, string> DefaultHeaders = new Dictionary<string, string>
        {
            {"Content-Type", "application/json"}
        };
    }
}