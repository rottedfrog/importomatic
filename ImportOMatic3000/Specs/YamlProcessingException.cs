using System;

namespace ImportOMatic3000.Specs
{
    public class YamlProcessingException : Exception
    {
        public string YamlPath { get; }

        public YamlProcessingException(string yamlPath, string message) : base(message) => YamlPath = yamlPath;
        public YamlProcessingException(string yamlPath, Exception ex) 
            : base($"Error reading node {yamlPath}: {ex.Message}", ex)
        { }
    }

}
