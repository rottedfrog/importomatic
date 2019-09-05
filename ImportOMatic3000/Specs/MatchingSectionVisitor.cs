using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class MatchingSectionVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Matching Section";

        public override void Visit(YamlScalarNode scalar)
        {
            LineClass.MatchingSection = scalar.Value;
        }
    }
}
