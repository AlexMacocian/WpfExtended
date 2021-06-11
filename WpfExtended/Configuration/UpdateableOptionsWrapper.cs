using System.Extensions;

namespace System.Windows.Extensions
{
    public sealed class UpdateableOptionsWrapper<T> : IUpdateableOptions<T>
        where T : class
    {
        private readonly IOptionsManager configurationManager;
        
        public T Value { get; }

        public UpdateableOptionsWrapper(IOptionsManager configurationManager, T options)
        {
            this.configurationManager = configurationManager.ThrowIfNull(nameof(configurationManager));
            this.Value = options;
        }

        public void UpdateOption()
        {
            this.configurationManager.UpdateOptions(this.Value);
        }
    }
}
