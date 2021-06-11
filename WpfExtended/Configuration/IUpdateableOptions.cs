using Microsoft.Extensions.Options;

namespace System.Windows.Extensions
{
    public interface IUpdateableOptions<out T> : IOptions<T>
        where T : class
    {
        /// <summary>
        /// Updates the configuration with the current value stored in <see cref="IOptions{TOptions}.Value"/>.
        /// </summary>
        void UpdateOption();
    }
}
