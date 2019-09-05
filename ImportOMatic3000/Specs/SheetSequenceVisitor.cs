using System;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class SheetSequenceVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Sheets[*]";

        public override void Visit(YamlScalarNode scalar)
        {
            if (Spec.Import.FileType is ISheetMatching sheetMatcher)
            { sheetMatcher.SheetMatchExpressions.Add(new Regex(scalar.Value, RegexOptions.IgnoreCase)); }
        }

    }
}
