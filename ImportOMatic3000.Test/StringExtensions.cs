using System.IO;
using System.Text;

namespace ImportOMatic3000.Test
{
    static class StringExtensions
    {
        public static Stream ToStream(this string str) => new MemoryStream(Encoding.UTF8.GetBytes(str));
    }
}
