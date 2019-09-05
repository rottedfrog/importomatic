using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class ImportSpecVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import";

        public override void Visit(YamlMappingNode mapping)
        {
            Spec.Import = new ImportSpec();
            VisitChildren(mapping);
        }
    }
}
