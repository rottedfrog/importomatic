using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class MatchSheetsVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Matching Sheets";

        public override void Visit(YamlScalarNode scalar)
        {
            LineClass.MatchingSheets = new Regex(scalar.Value, RegexOptions.Compiled);
        }
    }
}
