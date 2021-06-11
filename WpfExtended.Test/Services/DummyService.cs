using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Extensions;
using System.Windows.Extensions;
using WpfExtended.Tests.Configuration;

namespace WpfExtended.Tests.Services
{
    public sealed class DummyService : IDummyService
    {
        private readonly ILogger<DummyService> logger;
        private readonly IOptions<DummyOptions> dummyOptions;
        private readonly IUpdateableOptions<DummyOptions> updateableDummyOptions;
        private readonly ILiveOptions<DummyOptions> liveOptions;
        private readonly ILiveUpdateableOptions<DummyOptions> liveUpdateableOptions;

        public DummyService(
            ILogger<DummyService> logger,
            IOptions<DummyOptions> options,
            IUpdateableOptions<DummyOptions> updateableOptions,
            ILiveUpdateableOptions<DummyOptions> liveUpdateableOptions,
            ILiveOptions<DummyOptions> liveOptions)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
            this.dummyOptions = options.ThrowIfNull(nameof(options));
            this.updateableDummyOptions = updateableOptions.ThrowIfNull(nameof(updateableOptions));
            this.liveOptions = liveOptions.ThrowIfNull(nameof(liveOptions));
            this.liveUpdateableOptions = liveUpdateableOptions.ThrowIfNull(nameof(liveUpdateableOptions));
        }

        public void OnClosing()
        {
            this.logger.LogInformation($"Closing {nameof(DummyService)}");
        }

        public void OnStartup()
        {
            this.logger.LogInformation($"Starting {nameof(DummyService)}");
        }
    }
}
