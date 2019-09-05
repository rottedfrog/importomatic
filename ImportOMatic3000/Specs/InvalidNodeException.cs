using System;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    public class InvalidNodeException : Exception
    {
        public string Path { get; }
        public YamlNode Node { get; }

        public InvalidNodeException(string path, YamlNode node)
          : base($"Data of type '{node.NodeType}' is not valid for node {path}.")
        {
            Node = node;
            Path = path;
        }
    }

}
