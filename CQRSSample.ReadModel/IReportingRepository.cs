namespace CQRSSample.ReadModel
{
    public interface IReportingRepository
    {
        void Update<TDto>(object update, object where) where TDto : class;
    }
}