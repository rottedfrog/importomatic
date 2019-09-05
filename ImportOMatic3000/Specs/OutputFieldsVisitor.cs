using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class OutputFieldsVisitor : YamlNodeVisitorBase
    {
        private ExpressionGenerator _generator;

        public OutputFieldsVisitor(ExpressionGenerator generator)
        {
            _generator = generator;
        }

        public override string ExpectedYPath => "/Output[*]/*";

        public override void Visit(YamlScalarNode scalar)
        {
            Spec.Output.Fields.Add(new OutputField(Name, _generator.Generate(scalar.Value)));
        }
    }
}
