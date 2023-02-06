﻿using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace Step1_WebApi_Initials.Logger
{
    public sealed class LogMessage
    {
        public LogLevel Type { get; set; }

        [JsonConverter(typeof(EpochTimestampJsonConverter))]
        public DateTimeOffset Timestamp { get; set; }

        public string Message { get; set; }

        public string Category { get; set; }

        public int EventId { get; set; }
    }
}
