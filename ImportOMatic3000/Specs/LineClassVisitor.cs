using System.Collections.Generic;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class LineClassVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes";
        public override void Visit(YamlSequenceNode sequence)
        {
            Spec.Import.LineClasses = new List<LineClass>();
            Visitors = Visitors.Union(new IYamlNodeVisitor[] { new NullVisitor("/Import/Line Classes[*]") });
            VisitChildren(sequence);
        }
    }
}
