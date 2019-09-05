using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class SheetsVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Sheets";

        public override void Visit(YamlSequenceNode sequence)
        {
            if (Spec.Import.FileType is ISheetMatching sheetMatcher)
            {
                sheetMatcher.SheetMatchExpressions = new List<Regex>();
                VisitChildren(sequence);
            }
            else
            { throw new InvalidOperationException($"Unable to set sheets for FileType {Spec.Import.FileType.Format}"); }
        }
    }
}
