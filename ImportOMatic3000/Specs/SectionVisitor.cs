using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class SectionVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Section";

        public override void Visit(YamlScalarNode scalar)
        {
            LineClass.Section = scalar.Value;
        }
    }
}
