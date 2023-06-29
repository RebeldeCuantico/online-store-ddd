using Common.Domain;

namespace Common.Infrastructure
{
    public interface IRepositoryBase<T>
        where T : AggregateRoot
    {
        void SetDates(T aggregate);

        void SetDeleteDate(T aggregate);

        public void SetUpdateDate(T aggregate, DateTime updateDate);
    }
}