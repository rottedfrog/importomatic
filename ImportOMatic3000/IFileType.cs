using System.IO;

namespace ImportOMatic3000
{
    public interface IFileType
    {
        string Format { get; }
        IRowReader RowReader(Stream file);
    }
}
