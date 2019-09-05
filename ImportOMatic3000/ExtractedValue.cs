using System;
using System.Collections.Generic;

namespace ImportOMatic3000
{
    public class ExtractedValue
    {
        public string Name { get; }
        public IInitialFieldValue InitialValue { get; }
        public List<IStringFilter> Filters { get; set; } = new List<IStringFilter>();

        public ExtractedValue(string name, IInitialFieldValue initialValue)
        {
            Name = name;
            InitialValue = initialValue;
        }

        public string Extract(IRow row)
        {
            try
            {
                var field = InitialValue.GetValue(row);
                foreach(var filter in Filters)
                {
                    field = filter.Apply(field);
                }
                return field;
            }
            catch (Exception ex)
            { throw new ExtractedValueException(this, row, InitialValue, ex); }
        }
    }
}
