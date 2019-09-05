using System.Linq;

namespace ImportOMatic3000.Specs
{
    public class UnknownNodeException : YamlProcessingException
    {
        public UnknownNodeException(string yamlPath)
            : base(yamlPath, $"Unknown item '{yamlPath.Split('/').Last()}' found in specification. Path is {yamlPath}.")
        { }
    }
}
