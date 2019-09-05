namespace ImportOMatic3000
{
    public class SheetInitialValue : IInitialFieldValue
    {
        public string GetSourceField(IRow row) => $"sheet \"{row.Sheet}\"";

        public string GetValue(IRow row) => row.Sheet;
    }

}
