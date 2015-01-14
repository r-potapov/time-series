using System.Linq;
using TimeSeries.Domain.Entities;

namespace TimeSeries.Domain.Abstract
{
    public interface ITimeSeriesRepository
    {
        IQueryable<TimeSerie> TimeSeries { get; }
        void SaveTimeSerie(TimeSerie timeSerie);
        TimeSerie DeleteTimeSerie(int timeSerieId, string userId);
    }
}
