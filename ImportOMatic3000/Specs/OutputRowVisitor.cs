using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class OutputRowVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Output Row";

        public override void Visit(YamlScalarNode scalar)
        {
            LineClass.OutputLine = bool.Parse(scalar.Value);
        }
    }
}
