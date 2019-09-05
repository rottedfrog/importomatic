using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ImportOMatic3000
{
    public class StringArgFactoryGenerator<T>
    {
        public Func<string[], T> Generate<U>() where U : T
            => Generate(typeof(U));

        private static Dictionary<Type, Func<string, object>> _converters = new Dictionary<Type, Func<string, object>>()
        {
            [typeof(string)] = x => x,
            [typeof(int)] = x => int.Parse(x, NumberStyles.Integer, CultureInfo.InvariantCulture),
            [typeof(Regex)] = x => new Regex(x, RegexOptions.Compiled)
        };

        public Func<string[], T> Generate(Type t)
        {
            var constructors = t.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
            { throw new InvalidOperationException($"No public constructors found on {t.Name}"); }
            if (constructors.Length > 1)
            { throw new InvalidOperationException($"Too many public constructors found on ({t.Name})"); }
            var parameterInfo = constructors[0].GetParameters();
            var converters = parameterInfo.Select(x => _converters[x.ParameterType]).ToArray();
            return (stringArray) =>
            {
                if (stringArray == null)
                { stringArray = new string[0]; }
                if (stringArray.Length != converters.Length)
                { throw new InvalidOperationException($"Incorrect number of parameters. Expected {parameterInfo.Length}, found {stringArray.Length}."); }
                var parameters = stringArray.Select((x, i) => converters[i](x)).ToArray();
                return (T)constructors[0].Invoke(parameters);
            };
        }
    }
}
