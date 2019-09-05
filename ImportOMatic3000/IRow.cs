namespace ImportOMatic3000
{
    public interface IRow
    {
        int LineNumber { get; }
        string[] Fields { get; }
        string GetSourceField(int fieldIndex);
        string GetSourceRow();
        string Sheet { get; }
    }
}
