using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class FileTypeVisitor : YamlNodeVisitorBase
    {
        static Dictionary<string, IFileType> DefaultFileTypes;

        Dictionary<string, IFileType> _fileTypes;

        public FileTypeVisitor(Dictionary<string, IFileType> fileTypes)
        { _fileTypes = fileTypes; }

        public override string ExpectedYPath => "/Import/File Type";

        public override void Visit(YamlScalarNode scalar)
        {
            Spec.Import.FileType = _fileTypes[scalar.Value];
        }
    }
}
