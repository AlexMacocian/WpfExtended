using System.Extensions;

namespace System.Windows.Extensions
{
    public sealed class LiveUpdateableOptionsWrapper<T> : ILiveUpdateableOptions<T>
        where T : class
    {
        private readonly IOptionsManager configurationManager;

        private T value;

        public T Value
        {
            get
            {
                this.value = this.configurationManager.GetOptions<T>();
                return this.value;
            }
        }

        public LiveUpdateableOptionsWrapper(IOptionsManager configurationManager)
        {
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
        }

        public void UpdateOption()
        {
            this.configurationManager.UpdateOptions<T>(this.value);
        }
    }
}
