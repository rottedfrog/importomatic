using System;
using System.Collections.Generic;
using System.Linq;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    abstract class YamlNodeVisitorBase : IYamlNodeVisitor
    {
        public abstract string ExpectedYPath { get; }
        public string ActualPath { get; set; }
        public IEnumerable<IYamlNodeVisitor> Visitors { get; set; }

        public string Name => ActualPath.Split('/').Last();
        public Spec Spec { get; set; }

        public void Visit(YamlStream stream) { }

        public void Visit(YamlDocument document) { }
        public virtual void Visit(YamlScalarNode scalar) => throw new InvalidNodeException(ActualPath, scalar);
        public virtual void Visit(YamlSequenceNode sequence) => throw new InvalidNodeException(ActualPath, sequence);
        public virtual void Visit(YamlMappingNode mapping) => throw new InvalidNodeException(ActualPath, mapping);

        private bool YamlPathPartMatch(string match, string part)
        {
            if (match == part || match == "*")
            { return true; }
            if (match.EndsWith("[*]") && part.StartsWith(match.Substring(0, match.Length - 3)))
            { return part.Substring(match.Length - 3).StartsWith("["); }
            return false;
        }

        private bool YamlPathMatch(string matchPattern, string path)
        {
            if (matchPattern == "**")
            { return true; }
            var matchParts = matchPattern.Split('/');
            var pathParts = path.Split('/');
            var matchLength = matchParts.Length;
            if (matchParts.Last() == "**")
            { matchLength = matchParts.Length - 1; }
            else if (pathParts.Length != matchParts.Length)
            { return false; }
            return (pathParts.Length >= matchLength) &&
                   Enumerable.Range(0, matchLength).All(i => YamlPathPartMatch(matchParts[i], pathParts[i]));
        }

        private void Visit(YamlNode node, string path)
        {
            var visitor = Visitors.FirstOrDefault(x => YamlPathMatch(x.ExpectedYPath, path));
            if (visitor == null)
            { throw new UnknownNodeException(path); }

            visitor.Spec = Spec;
            visitor.Visitors = Visitors;
            visitor.ActualPath = path;
            try
            { node.Accept(visitor); }
            catch (YamlProcessingException)
            { throw; }
            catch (Exception ex)
            {
                throw new YamlProcessingException(path, ex);
            }
            
        }

        protected void VisitChildren(YamlSequenceNode sequence)
        {
            for(int i = 0; i < sequence.Children.Count; ++i)
            {
                Visit(sequence.Children[i], ActualPath + $"[{i}]");
            }
        }

        protected void VisitChildren(YamlMappingNode mapping)
        {
            foreach (var node in mapping.Children)
            { Visit(node.Value, string.Join("/", ActualPath, node.Key)); }
        }
    }
}
