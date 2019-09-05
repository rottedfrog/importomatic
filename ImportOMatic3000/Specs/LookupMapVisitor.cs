using System.Linq;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class LookupMapVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Lookups/*";

        class StringValueVisitor : YamlNodeVisitorBase
        {
            public override string ExpectedYPath => "/Lookups/*/*";
            public override void Visit(YamlScalarNode scalar) => Value = scalar.Value;
            
            public string Value { get; private set; }
        }

        private string StringValue(YamlNode node)
        {
            var visitor = new StringValueVisitor();
            node.Accept(visitor);
            return visitor.Value;
        }

        public override void Visit(YamlMappingNode mapping)
        {
            Spec.Lookups.Add(Name, mapping.Children.ToDictionary(x => StringValue(x.Key), x => StringValue(x.Value)));
        }
    }
}
