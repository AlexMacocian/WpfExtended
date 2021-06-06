﻿using Microsoft.Extensions.Logging;
using System;

namespace WpfExtended.Models
{
    public sealed record Log
    {
        public Exception Exception { get; set; }
        public DateTime LogTime { get; set; }
        public string Category { get; set; }
        public string EventId { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string CorrelationVector { get; set; }
    }
}