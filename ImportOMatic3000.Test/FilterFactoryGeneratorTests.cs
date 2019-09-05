using ImportOMatic3000.Filters;
using System;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class FilterFactoryGeneratorTests
    {
        public class MultiConstructor : IStringFilter
        {
            public MultiConstructor(string a, string b) { }
            public MultiConstructor(string a) { }

            public string Apply(string value) => value;
        }

        public class PrivateConstructor : IStringFilter
        {
            private PrivateConstructor() { }

            public string Apply(string value) => value;
        }

        [Fact]
        public void Generate_WhenGeneratedFunctionIsCalledWithTooManyParameters_ThrowsInvalidOperationException()
        {
            var generator = new StringArgFactoryGenerator<IStringFilter>();
            var factory = generator.Generate<TrimFilter>();
            Assert.Throws<InvalidOperationException>(() => factory(new[] { "Too many params" }));
        }

        [Fact]
        public void Generate_WhenTypeHasMultiplePublicConstructors_ThrowsInvalidOperationException()
        {
            var generator = new StringArgFactoryGenerator<IStringFilter>();
            Assert.Throws<InvalidOperationException>(() => generator.Generate<MultiConstructor>());
        }

        [Fact]
        public void Generate_WhenTypeHasNoPublicConstructor_ThrowsInvalidOperationException()
        {
            var generator = new StringArgFactoryGenerator<IStringFilter>();

            Assert.Throws<InvalidOperationException>(() => generator.Generate<PrivateConstructor>());
        }

        [Fact]
        public void Generate_WhenTypeHasParameterlessPublicConstructor_CreatesASimpleFactoryFunction()
        {
            var generator = new StringArgFactoryGenerator<IStringFilter>();

            var factory = generator.Generate<TrimFilter>();
            var result = factory(null);
            Assert.IsType<TrimFilter>(result);
        }

        [Fact]
        public void Generate_WhenTypeHasStringParameters_CreatesFactoryThatAcceptsThoseParameters()
        {
            var generator = new StringArgFactoryGenerator<IStringFilter>();

            var factory = generator.Generate<MockStringFilter>();
            var result = factory(new[] { "Bar" });
            Assert.Equal("FooBar", result.Apply("Foo"));
        }

        [Fact]
        public void Generate_WhenTypeHasIntParameters_CreatesFactoryThatAcceptsThoseParameters()
        {
            var generator = new StringArgFactoryGenerator<IStringFilter>();

            var factory = generator.Generate<SubstringFilter>();
            var result = factory(new[] { "2", "4" });
            Assert.Equal("ooBa", result.Apply("FooBar"));
        }
    }
}
