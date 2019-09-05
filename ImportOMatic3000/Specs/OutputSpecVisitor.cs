using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class OutputSpecVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Output";
        private readonly ExpressionGenerator _generator;
        public OutputSpecVisitor(Type[] functionTypes, IEnumerable<string> specialFieldNames)
        {
            _generator = new ExpressionGenerator();

            foreach (var type in functionTypes ?? new Type[0])
            { _generator.AddFunctions(type); }
            _generator.ValidIdentifiers = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            if (specialFieldNames != null)
            { _generator.ValidIdentifiers.UnionWith(specialFieldNames); }
        }

        public override void Visit(YamlSequenceNode sequence)
        {
            Spec.Output = new OutputSpec();
            Spec.Output.Fields = new List<OutputField>();
            if (Spec.Import?.LineClasses != null)
            {
                _generator.ValidIdentifiers.UnionWith(Spec.Import.LineClasses.SelectMany(lt => lt.ValuesToExtract.Select(field => field.Name)));
            }
            Visitors = Visitors.Union(new IYamlNodeVisitor[] { new NullVisitor("/Output[*]"), new OutputFieldsVisitor(_generator) });
            VisitChildren(sequence);
        }
    }
}
