using System.Text;

namespace Spectre.Terminals
{
    internal static class EncodingExtensions
    {
        private const int UnicodeCodePage = 1200;

        public static bool IsUnicode(this Encoding encoding)
        {
            return encoding.CodePage == UnicodeCodePage;
        }
    }
}
