using System.Collections.Generic;

namespace ImportOMatic3000.Specs
{
    public interface IYamlVisitorProvider
    {
        IEnumerable<IYamlNodeVisitor> GetVisitors();
    }
}
