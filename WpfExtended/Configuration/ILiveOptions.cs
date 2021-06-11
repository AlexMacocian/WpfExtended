using Microsoft.Extensions.Options;

namespace System.Windows.Extensions
{
    public interface ILiveOptions<T> : IOptions<T>
        where T : class
    {
    }
}
