using System;
using System.Collections.Generic;
using System.Linq;
using log4net.Core;
using log4net.Filter;

namespace FhirStarter.STU3.Detonator.Core.LoggerSettings
{
    public class AndFilter : FilterSkeleton
    {
        private readonly IList<IFilter> _filters = new List<IFilter>();

        public override FilterDecision Decide(LoggingEvent loggingEvent)
        {
            if (loggingEvent == null)
                throw new ArgumentNullException(nameof(loggingEvent));
            return _filters.Any(filter => !filter.Decide(loggingEvent).Equals(FilterDecision.Accept)) ? FilterDecision.Deny : FilterDecision.Accept;
        }

        public IFilter Filter
        {
            set => _filters.Add(value);
        }

        public bool AcceptOnMatch { get; set; }
    }
}
