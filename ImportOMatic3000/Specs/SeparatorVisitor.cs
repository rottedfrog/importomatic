using System;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class SeparatorVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Delimited Field Separator";

        public override void Visit(YamlScalarNode scalar)
        {
            if (Spec.Import.FileType is IDelimited delimited)
            {
                delimited.Separator = scalar.Value.TrimStart()[0];
            }
            else
            { throw new InvalidOperationException($"Unable to set separator for FileType {Spec.Import.FileType.Format}"); }
        }
    }
}
