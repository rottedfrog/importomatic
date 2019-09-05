using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class YamlSpecReader
    {
        internal IReadOnlyList<IYamlNodeVisitor> Visitors { get; set; } = new List<IYamlNodeVisitor>();

        public Spec Parse(TextReader reader)
        {
            var spec = new Spec();
            var yaml = new YamlStream();
            yaml.Load(reader);
            var rootVisitor = new NullVisitor("")
            {
                Visitors = Visitors,
                Spec = spec,  
            };
            yaml.Documents[0].RootNode.Accept(rootVisitor);

            return spec;
        }
    }
}
