using System;
using System.Collections.Generic;
using Xunit;

namespace ImportOMatic3000.Test
{
    public class ExtractedValueTests
    {
        [Fact]
        public void ExtractingAFieldWithNoFiltersReturnsTheWholeField()
        {
            var field = new ExtractedValue("Foo", new ColumnInitialValue("A"));
            var result = field.Extract(new SimpleRow("Hello, World"));
            Assert.Equal("Hello, World", result);
        }

        [Fact]
        public void ExtractingAFieldWithNonZeroColumnExtractsFromTheCorrectColumn()
        {
            var field = new ExtractedValue("Foo", new ColumnInitialValue("C"));
            var result = field.Extract(new SimpleRow("Hello", "Brave", "New", "World"));
            Assert.Equal("New", result);
        }

        [Fact]
        public void ExtractingAFieldFromAColumnThatDoesNotExistThrowsExtractionErrorWithExtractedFieldSpecified()
        {
            var field = new ExtractedValue("Foo", new ColumnInitialValue("ZZ"));
            var exception = Assert.Throws<ExtractedValueException>(() => field.Extract(new SimpleRow("Hello World")));
            Assert.Equal(field, exception.ExtractedValue);
        }

        class FooFilter : IStringFilter
        {
            public string Apply(string value) => value + "Foo";
        }

        [Fact]
        public void AddingAFilterChangesTheExtractedValue()
        {
            var field = new ExtractedValue("Foo", new ColumnInitialValue("A"));
            field.Filters = new List<IStringFilter> { new FooFilter() };
            var result = field.Extract(new SimpleRow("Bar"));
            Assert.Equal("BarFoo", result);
        }

        class BarFilter : IStringFilter
        {
            public string Apply(string value) => value + "Bar";
        }

        [Fact]
        public void MultipleFiltersAppliesThemInTheOrderSpecified()
        {
            var field = new ExtractedValue("Foo", new ColumnInitialValue("A"));
            field.Filters = new List<IStringFilter> { new FooFilter(), new BarFilter() };
            var field2 = new ExtractedValue("Bar", new ColumnInitialValue("A"));
            field2.Filters = new List<IStringFilter> { new BarFilter(), new FooFilter() };
            var result = field.Extract(new SimpleRow("")) + field2.Extract(new SimpleRow(""));
            Assert.Equal("FooBarBarFoo", result);
        }

        private class BoomFilter : IStringFilter
        {
            public string Apply(string value) => throw new InvalidOperationException("Boom!");
        }

        [Fact]
        public void FilterExceptionsAreWrappedInExtractedValueExceptions()
        {
            var field = new ExtractedValue("Foo", new ColumnInitialValue("A"))
            { Filters = new List<IStringFilter> { new BoomFilter() } };
            var exception = Assert.Throws<ExtractedValueException>(() => field.Extract(new SimpleRow("Hello World")));
            Assert.IsType(typeof(InvalidOperationException), exception.InnerException);
        }
    }
}
