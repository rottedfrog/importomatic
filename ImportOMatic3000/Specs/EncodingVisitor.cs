using System;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace ImportOMatic3000.Specs
{
    class EncodingVisitor : YamlNodeVisitorBase
    {
        public override string ExpectedYPath => "/Import/Encoding";

        static EncodingVisitor()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public override void Visit(YamlScalarNode scalar)
        {
            if (Spec.Import.FileType is IEncodable enc)
            { enc.Encoding = Encoding.GetEncoding(scalar.Value); }
            else
            { throw new InvalidOperationException($"Unable to set encoding for FileType {Spec.Import.FileType.Format}"); }
        }
    }
}
