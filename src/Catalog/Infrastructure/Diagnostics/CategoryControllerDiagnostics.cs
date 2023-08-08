using Microsoft.Extensions.Logging;
using Prometheus;

namespace Catalog.Infrastructure.Diagnostics
{
    public partial class CategoryControllerDiagnostics
    {
        private readonly ILogger<CategoryControllerDiagnostics> _logger;
        private readonly Counter _categoryCount;

        public CategoryControllerDiagnostics(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CategoryControllerDiagnostics>();
            _categoryCount = Metrics.CreateCounter("add_category_total", "Total of categories added");
        }

        public void AddCategory(int count = 1)
        {
            _categoryCount.Inc(count);
        }

        [LoggerMessage(EventId = 50001, Level = LogLevel.Information, Message = "Category Added with Id: {id}")]
        public partial void CategoryAdded(Guid id);
    }
}
