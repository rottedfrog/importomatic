using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class NullVisitor : YamlNodeVisitorBase
    {
        readonly string _expectedYPath;
        public override string ExpectedYPath { get => _expectedYPath; }

        public NullVisitor(string path) => _expectedYPath = path;

        public override void Visit(YamlScalarNode scalar) { }
        public override void Visit(YamlSequenceNode sequence) => VisitChildren(sequence);
        public override void Visit(YamlMappingNode mapping) => VisitChildren(mapping);
    }
}
