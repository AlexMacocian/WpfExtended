namespace System.Windows.Extensions
{
    public sealed class DefaultOptionsManager : IOptionsManager
    {
        public T GetOptions<T>() where T : class
        {
            return default;
        }

        public void UpdateOptions<T>(T value) where T : class
        {
        }
    }
}
