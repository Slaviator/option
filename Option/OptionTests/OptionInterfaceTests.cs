using System;
using CodingHelmet.Optional;
using Xunit;

namespace OptionTests
{
    public abstract class OptionInterfaceTests<T, TMap>
    {
        [Fact]
        public void Some_MatchesSomePattern()
        {
            bool callbackInvoked = false;
            Action<T> callback = obj => callbackInvoked = true;

            Option<T> option = CreateSome(SampleValue);

            if (option is Some<T> some)
            {
                callback(some);
            }

            Assert.True(callbackInvoked);
        }

        [Fact]
        public void SomeContent_PassedToCallbeck_AfterPatternMatching()
        {
            T expectedArgument = SampleValue;
            T actualArgument = default(T);
            Action<T> callback = obj => actualArgument = obj;

            Option<T> option = CreateSome(expectedArgument);

            if (option is Some<T> some)
            {
                callback(some.Content);
            }

            Assert.True(AreSame(expectedArgument, actualArgument));
        }

        [Fact]
        public void None_DoesNotMatchSomePattern()
        {
            bool callbackInvoked = false;
            Action<object> callback = obj => callbackInvoked = true;

            Option<T> option = None.Value;

            if (option is Some<T> some)
            {
                callback(some);
            }

            Assert.False(callbackInvoked);
        }

        [Fact]
        public void CreateSome_ReturnsNonNull()
        {
            Assert.NotNull(CreateSome(SampleValue));
        }

        [Fact]
        public void CreateNone_ReturnsNonNull()
        {
            Assert.NotNull(CreateNone());
        }

        [Fact]
        public void CreateSome_NotEqualNone()
        {
            Assert.NotEqual(None.Value, CreateSome(SampleValue));
        }

        [Fact]
        public void CreateNone_EqualNone()
        {
            Assert.Equal(None.Value, CreateNone());
        }

        [Fact]
        public void Some_EqualContent()
        {
            T value = SampleValue;
            Assert.Equal(value, CreateSome(value));
        }

        [Fact]
        public void Map_SomeReceivesMappingToObject_ReturnsSomeOfThatObject()
        {
            TMap mappedValue = SampleMapToValue;

            Option<T> option = CreateSome(SampleValue);
            Option<TMap> mapped = option.Map(_ => mappedValue);

            Assert.Equal(mappedValue, mapped);
        }

        [Fact]
        public void Map_NoneReceivesMappingToObject_ReturnsNone()
        {
            Option<T> option = CreateNone();
            Option<TMap> mapped = option.Map(_ => SampleMapToValue);

            Assert.Equal(None.Value, mapped);
        }

        [Fact]
        public void Map_SomeReceivesMappingFunction_PassesContainedValueToMappingFunction()
        {
            T expectedValue = SampleValue;
            T actualValue = default(T);

            Option<T> option = CreateSome(expectedValue);
            Option<T> mapped = option.Map(x => actualValue = x);

            Assert.True(AreSame(expectedValue, actualValue));
            Assert.Equal(expectedValue, mapped);
        }

        [Fact]
        public void Map_NoneReceivesMappingFunction_MappingFunctionNotCalled()
        {
            bool mappingInvoked = false;

            Option<T> option = CreateNone();
            Option<TMap> mapped = option.Map(_ =>
            {
                mappingInvoked = true;
                return default(TMap);
            });

            Assert.False(mappingInvoked);
        }

        [Fact]
        public void Collapse_SomeContainingValue_ReturnsContainedValue()
        {
            T expectedValue = SampleValue;
            Option<T> option = CreateSome(expectedValue);
            T actualValue = option.Reduce(() => AlternateSampleValue);

            Assert.True(AreSame(expectedValue, actualValue));
        }

        [Fact]
        public void Collapse_NoneReceivesMethodWhichReturnsAlternateValue_ReturnsThatValue()
        {
            T expectedValue = AlternateSampleValue;
            Option<T> option = CreateNone();
            T actualValue = option.Reduce(() => expectedValue);

            Assert.True(AreSame(expectedValue, actualValue));
        }

        protected abstract T SampleValue { get; }
        protected abstract T AlternateSampleValue { get; }
        protected abstract TMap SampleMapToValue { get; }

        protected abstract Option<T> CreateSome(T obj);
        protected abstract Option<T> CreateNone();

        protected abstract bool AreSame(T a, T b);
    }
}