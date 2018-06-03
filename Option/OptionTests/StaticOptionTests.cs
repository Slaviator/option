using CodingHelmet.Optional;
using Xunit;

namespace OptionTests
{
    public class StaticOptionTests
    {
        [Fact]
        public void SomeOfObject_ReceivesNonNullObject_ReturnsNonNull()
        {
            Option<object> option = None.Value;
            Assert.NotNull(option);
        }

        [Fact]
        public void NoneOfObject_ReturnsNonNull()
        {
            Option<object> option = None.Value;
            Assert.NotNull(option);
        }
    }
}