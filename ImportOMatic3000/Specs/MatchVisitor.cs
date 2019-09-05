using System.Collections.Generic;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class MatchVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Match";

        public override void Visit(YamlScalarNode scalar)
        {
            LineClass.MatchExpressions = new List<CellMatcher> { new CellMatcher(0, new Regex(scalar.Value, RegexOptions.Compiled)) };
        }

        public override void Visit(YamlMappingNode mapping)
        {
            LineClass.MatchExpressions = new List<CellMatcher>(mapping.Children.Count);
            VisitChildren(mapping);
        }
    }
}
