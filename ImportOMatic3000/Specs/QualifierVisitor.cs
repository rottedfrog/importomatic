using System;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class QualifierVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Delimited Field Qualifier";

        public override void Visit(YamlScalarNode scalar)
        {
            if (Spec.Import.FileType is IDelimited delimited)
            {
                delimited.Qualifier = scalar.Value.TrimStart()[0];
            }
            else
            { throw new InvalidOperationException($"Unable to set qualifier for FileType {Spec.Import.FileType.Format}"); }
        }
    }
}
