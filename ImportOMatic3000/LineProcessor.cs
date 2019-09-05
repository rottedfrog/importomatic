using ImportOMatic3000.Specs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImportOMatic3000
{
    class LineProcessor
    {
        public IRowReader RowReader { get; set; }
        public List<(string Name, Func<Spec, IRow, LineClass, string> Create)> SpecialVariables { get; set; } = new List<(string Name, Func<Spec, IRow, LineClass, string> Create)>();
        public Spec Spec { get; set; }

        private void AddSpecialVariables(IDictionary<string, string> values, IRow row, LineClass matchingLineClass)
        {
            foreach(var variable in SpecialVariables)
            { values[variable.Name] = variable.Create(Spec, row, matchingLineClass); }
        }

        public void Process(ILineEventListener listener)
        {
            string section = null;
            var extractedFields = Spec.Import.LineClasses.SelectMany(x => x.ValuesToExtract)
                                                       .Select(x => x.Name)
                                                       .Distinct(StringComparer.InvariantCultureIgnoreCase)
                                                       .ToDictionary(x => x, x => "", StringComparer.InvariantCultureIgnoreCase)
                                                       .ToVariableDictionary();
            var results = new object[Spec.Output.Fields.Count];
            foreach (var row in RowReader)
            {
                try
                {
                    var lineClass = Spec.Import.LineClasses.FirstOrDefault(lt => lt.IsMatch(row, section));
                    if (lineClass == null)
                    { continue; }
                    if (!string.IsNullOrEmpty(lineClass.Section))
                    { section = lineClass.Section; }
                    lineClass.ExtractValues(extractedFields, row);
                    
                    if (lineClass.OutputLine)
                    {
                        AddSpecialVariables(extractedFields, row, lineClass);
                        for(int i = 0; i < Spec.Output.Fields.Count; ++i)
                        {
                            try
                            { results[i] = Spec.Output.Fields[i].OutputFormula(extractedFields); }
                            catch (Exception ex)
                            { throw new OutputFieldException(Spec.Output.Fields[i].Name, row, ex); }
                        }
                        listener.OnOutputLine(results);
                    }
                }
                catch (RowException ex)
                {
                    listener.OnError(ex);
                }
                catch (Exception ex)
                {
                    listener.OnError(new UnhandledRowException(row, ex));
                }
            }
            listener.OnCompleted(); 
        }
    }
}
