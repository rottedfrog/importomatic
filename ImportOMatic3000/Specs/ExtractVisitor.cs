using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class ExtractVisitor : LineClassSubNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Line Classes[*]/*/Extract";

        public override void Visit(YamlMappingNode mapping)
        {
            LineClass.ValuesToExtract = new List<ExtractedValue>();
            VisitChildren(mapping);
        }
    }
}
