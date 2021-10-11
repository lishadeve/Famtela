using Famtela.Application.Interfaces.Services;
using System;

namespace Famtela.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}