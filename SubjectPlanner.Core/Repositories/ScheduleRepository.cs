namespace SubjectPlanner.Core;

public class ScheduleRepository : IScheduleRepository
{
    protected IIncidencesRepository incidencesRepository;
    public ScheduleRepository(IIncidencesRepository incidencesRepository)
    {
        this.incidencesRepository = incidencesRepository;
    }

    public List<ClassTime> AvailableClassTimes(ClassDay classDay)
    {
        List<TimeSpan> classTimeIntervalSpread = this.SpreadInterval(classDay.ClassTime.TimeFrom, classDay.ClassTime.TimeTo);

        List<ClassTime> availableClassTimes = new();

        List<Incidence> incidences = incidencesRepository.AffectingClassDay(classDay);

        foreach (Incidence incidence in incidences)
        {
            List<TimeSpan> incidenceIntervalSpread = this.SpreadInterval(incidence.TimeFrom, incidence.TimeTo);

            List<TimeSpan> excludedAvailableClassTimes = classTimeIntervalSpread
            .Except(incidenceIntervalSpread)
            .ToList();

            TimeSpan firstTimeInList = excludedAvailableClassTimes.First();
            TimeSpan lastTimeInList = excludedAvailableClassTimes.Last();

            bool incidenceIsBetweenClassTime = (incidence.TimeFrom > classDay.ClassTime.TimeFrom && incidence.TimeTo < classDay.ClassTime.TimeTo);
            if (incidenceIsBetweenClassTime)
            {
                ClassTime firstClassSpace = new()
                {
                    TimeFrom = firstTimeInList,
                    TimeTo = incidence.TimeFrom,
                };

                ClassTime lastClassSpace = new()
                {
                    TimeFrom = incidence.TimeTo,
                    TimeTo = lastTimeInList,
                };

                availableClassTimes.Add(firstClassSpace);
                availableClassTimes.Add(lastClassSpace);
            }
            else
            {
                ClassTime availableClassTime = new ClassTime
                {
                    TimeFrom = firstTimeInList.Add(new TimeSpan(0, 0, -1)),
                    TimeTo = lastTimeInList,
                };

                /*
                    Se resta un segundo porque cuando una incidencia termina por ejemplo a las 10 en punto, la clase entonces inciaría a las 10:00:01.
                    Esto provocaría que sobre un segundo el último día de clases entonces se tenga que pasar a otro día de clases solo por ese segundo.                
                */
                availableClassTime.TimeFrom.Add(new TimeSpan(0, 0, -1));

                availableClassTimes.Add(availableClassTime);
            }
        }

        return availableClassTimes;
    }

    private List<TimeSpan> SpreadInterval(TimeSpan start, TimeSpan end)
    {
        TimeSpan timeInterval = end - start;
        List<TimeSpan> intervalSpread = new List<TimeSpan>();
        for (int i = 0; i <= timeInterval.TotalSeconds; i++)
        {
            TimeSpan newTime = new(0, 0, start.Seconds + i);
            newTime = start.Add(newTime);
            intervalSpread.Add(newTime);
        }

        return intervalSpread;
    }    
}