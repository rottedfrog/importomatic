using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class ExtractableValuesVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Extract/*";
        private readonly FilterParser _parser;

        public ExtractableValuesVisitor(FilterParser parser) => _parser = parser;
        public override void Visit(YamlScalarNode scalar)
        {
            var (column, filters) = _parser.Parse(scalar.Value);
            LineClass.ValuesToExtract.Add(new ExtractedValue(Name, column) { Filters = filters });
        }
    }
}
