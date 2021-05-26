using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectre.Terminal.Tests
{
    public static class ShouldlyExtensions
    {
        public static T And<T>(this T item)
        {
            return item;
        }
    }
}
