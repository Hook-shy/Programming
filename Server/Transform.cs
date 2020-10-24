using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Server
{
    public static class Transform
    {
        public static string SizeToString(long size)
        {
            if (size < 1024)
            {
                return $"{size:f2}Byte";
            }
            else if (size < 1024 * 1024)
            {
                return $"{size / 1024.0:f2}KB";
            }
            else if (size < 1024 * 1024 * 1024)
            {
                return $"{size / 1024.0 / 1024:f2}MB";
            }
            else
            {
                return $"{size / 1024.0 / 1024 / 1024:f2}GB";
            }
        }
    }
}
