using System;
using CodingHelmet.Optional;

namespace OptionTests
{
    public class OptionOfIntTests : OptionInterfaceTests<int, string>
    {
        protected override int SampleValue => 5;

        protected override int AlternateSampleValue => 3;

        protected override string SampleMapToValue => SampleValue.ToString();

        protected override Option<int> CreateSome(int obj) => obj;

        protected override Option<int> CreateNone() => None.Value;

        protected override bool AreSame(int a, int b) => a == b;
    }
}