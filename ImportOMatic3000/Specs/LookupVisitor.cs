using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class LookupVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Lookups";

        public override void Visit(YamlMappingNode mapping)
        {
            Spec.Lookups = new Dictionary<string, Dictionary<string, string>>();
            VisitChildren(mapping);
        }
    }
}
