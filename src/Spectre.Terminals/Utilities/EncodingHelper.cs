using System.Text;

namespace Spectre.Terminals
{
    internal static partial class EncodingHelper
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
