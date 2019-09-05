using System.Collections.Generic;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    public interface IYamlNodeVisitor : IYamlVisitor
    {
        IEnumerable<IYamlNodeVisitor> Visitors { get; set; }
        string ActualPath { get; set; }
        string ExpectedYPath { get; }
        Spec Spec { get; set; }
    }
}
