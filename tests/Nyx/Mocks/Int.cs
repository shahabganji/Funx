
using static Nyx.Helpers.OptionHelpers;

namespace Nyx.Tests.Mocks
{
    static class Int
    {

        internal static Option<int> Parse(string value)
        {
            bool parsed = int.TryParse(value, out var result);
            return parsed ? Some(result) : None;
        }

    }
}
