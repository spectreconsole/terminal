using System.Text;

namespace Spectre.Terminal
{
    internal static class EncodingHelper
    {
        static EncodingHelper()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        internal static Encoding GetEncodingFromCodePage(int codePage)
        {
            return Encoding.GetEncoding(codePage) ?? Encoding.UTF8;
        }
    }
}
