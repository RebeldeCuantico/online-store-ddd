using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using System.Diagnostics;

namespace Common.Infrastructure.Logging
{
    public class ElasticSearchJsonFormatterWithSpan : ElasticsearchJsonFormatter
    {

        protected override void WriteTimestamp(DateTimeOffset timestamp, ref string delim, TextWriter output)
        {
            var datetime = DateTime.UtcNow;         
            WriteJsonProperty("@timestamp", datetime, ref delim, output);
        }

        protected override void WritePropertiesValues(IReadOnlyDictionary<string, LogEventPropertyValue> properties, TextWriter output)
        {
            var precedingDelimiter = string.Empty;
            foreach (KeyValuePair<string, LogEventPropertyValue> property in properties) 
            {
                WriteJsonProperty(property.Key, property.Value, ref precedingDelimiter, output);
            }

            if(Activity.Current != null)
            {
                var traceId = Activity.Current.TraceId.ToString();
                if(traceId != null)
                {
                    WriteJsonProperty("TraceId", traceId, ref precedingDelimiter, output);
                }
                var spanId = Activity.Current.SpanId.ToString();
                if (spanId != null)
                {
                    WriteJsonProperty("SpanId", spanId, ref precedingDelimiter, output);
                }
                var parentId = Activity.Current.ParentId?.ToString();
                if (parentId != null)
                {
                    WriteJsonProperty("ParentId", parentId, ref precedingDelimiter, output);
                }
            }
        }
    }
}
