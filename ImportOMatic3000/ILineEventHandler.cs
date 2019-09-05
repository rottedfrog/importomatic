namespace ImportOMatic3000
{
    public interface ILineEventListener
    {
        void OnError(RowException ex);
        void OnOutputLine(object[] values);
        void OnCompleted();
    }
}
