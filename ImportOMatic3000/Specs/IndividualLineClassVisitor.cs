using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class IndividualLineClassVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*";

        public override void Visit(YamlMappingNode mapping)
        {
            Spec.Import.LineClasses.Add(new LineClass() { Name = Name });
            VisitChildren(mapping);
        }
    }
}
