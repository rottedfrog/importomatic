namespace ImportOMatic3000
{
    public interface IInitialFieldValue
    {
        string GetSourceField(IRow row);
        string GetValue(IRow row);
    }

}
