using System.Linq;
using TimeSeries.Domain.Entities;
using TimeSeries.Domain.Abstract;
using System.Data.Entity.Validation;

namespace TimeSeries.Domain.Concrete
{
    public class EFTimeSeriesRepository : ITimeSeriesRepository
    {
        private EFDbContext context = new EFDbContext();
        public IQueryable<TimeSerie> TimeSeries
        {
            get { return context.TimeSeries; }
        }

        public void SaveTimeSerie(TimeSerie timeSerie)
        {
            if (timeSerie.TimeSerieId == 0)
            {
                context.TimeSeries.Add(timeSerie);
            }
            else
            {
                TimeSerie dbEntry = context.TimeSeries.Find(timeSerie.TimeSerieId);
                if (dbEntry != null)
                {
                    dbEntry.VectorName = timeSerie.VectorName;
                    dbEntry.VectorData = timeSerie.VectorData;
                }
            }
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var failure in ex.EntityValidationErrors)
                {
                    //"Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    var entityType = failure.Entry.Entity.GetType().Name;
                    var validationErrors = failure.Entry.State;
                    foreach (var error in failure.ValidationErrors)
                    {
                        //"- Property: \"{0}\", Error: \"{1}\"",
                        var propertyName = error.PropertyName;
                        var errorMessage = error.ErrorMessage;
                    }
                }
                //throw;
            }
            catch (System.Exception ex)
            {
                string message = ex.Message;
                throw;
            }
        }

        public TimeSerie DeleteTimeSerie(int timeSerieId, string userId)
        {
            TimeSerie dbEntry = context.TimeSeries.FirstOrDefault(p => p.TimeSerieId == timeSerieId && p.User.Id == userId);
            if (dbEntry != null)
            {
                context.TimeSeries.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
