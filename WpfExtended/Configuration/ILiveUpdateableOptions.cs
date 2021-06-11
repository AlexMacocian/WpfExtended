namespace System.Windows.Extensions
{
    public interface ILiveUpdateableOptions<T> : ILiveOptions<T>, IUpdateableOptions<T>
        where T : class
    {
    }
}
