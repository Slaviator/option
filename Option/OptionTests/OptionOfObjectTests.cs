using CodingHelmet.Optional;

namespace OptionTests
{
    public class OptionOfObjectTests : OptionInterfaceTests<object, string>
    {
        private static object SampleObject = new object();
        private static object AlternateObject = new object();

        protected override object SampleValue => SampleObject;

        protected override object AlternateSampleValue => AlternateObject;

        protected override string SampleMapToValue => SampleValue.ToString();

        protected override Option<object> CreateSome(object obj) => obj;

        protected override Option<object> CreateNone() => None.Value;

        protected override bool AreSame(object a, object b) => ReferenceEquals(a, b);
    }
}