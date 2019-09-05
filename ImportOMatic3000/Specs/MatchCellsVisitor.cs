using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class MatchCellsVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Match/*";


        public override void Visit(YamlScalarNode scalar)
        {
            LineClass.MatchExpressions.Add(new CellMatcher(Name.ToColumnIndex(), new Regex(scalar.Value, RegexOptions.Compiled)));
        }
    }
}
