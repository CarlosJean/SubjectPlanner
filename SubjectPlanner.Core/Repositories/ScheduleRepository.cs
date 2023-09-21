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
        List<ClassTime> availableClassTimes = new(){
            classDay.ClassTime
        };

        List<Incidence> incidences = incidencesRepository.AffectingClassDay(classDay);

        foreach (Incidence incidence in incidences)
        {
            List<TimeSpan> incidenceIntervalSpread = this.SpreadInterval(incidence.TimeFrom, incidence.TimeTo);

            foreach (ClassTime availableClassTime in availableClassTimes.ToList())
            {
                bool incidenceAffectsClassTime = IncidenceAffectsClassDay(availableClassTime, incidence);

                if (incidenceAffectsClassTime)
                {
                    List<TimeSpan> classTimeIntervalSpread = this.SpreadInterval(availableClassTime.TimeFrom, availableClassTime.TimeTo);
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

                        List<ClassTime> newClassTimes = new List<ClassTime>{
                            firstClassSpace,
                            lastClassSpace
                        };

                        CleanAffectingPreviousAvailableClassTimes(ref availableClassTimes, newClassTimes);

                        availableClassTimes.Add(firstClassSpace);
                        availableClassTimes.Add(lastClassSpace);
                    }
                    else
                    {
                        ClassTime freeSpace = new ClassTime
                        {
                            TimeFrom = firstTimeInList,
                            TimeTo = lastTimeInList,
                        };

                        List<ClassTime> newClassTimes = new() { freeSpace };
                        CleanAffectingPreviousAvailableClassTimes(ref availableClassTimes, newClassTimes);

                        availableClassTimes.Add(freeSpace);
                    }
                }
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

    private void CleanAffectingPreviousAvailableClassTimes(ref List<ClassTime> previousClassTimes, List<ClassTime> newClassTimes)
    {

        foreach (var previousClassTime in previousClassTimes.ToList())
        {
            List<TimeSpan> previousClassTimeSpread = this.SpreadInterval(previousClassTime.TimeFrom, previousClassTime.TimeTo);

            foreach (var newClassTime in newClassTimes)
            {
                List<TimeSpan> newClassTimeSpread = this.SpreadInterval(newClassTime.TimeFrom, newClassTime.TimeTo);

                bool newClassAffectsPreviousClassTime = previousClassTimeSpread.Intersect(newClassTimeSpread).Any();

                if (newClassAffectsPreviousClassTime)
                {
                    previousClassTimes.Remove(previousClassTime);
                }
            }
        }
    }

    private bool IncidenceAffectsClassDay(ClassTime classTime, Incidence incidence)
    {

        List<TimeSpan> classTimeIntervalSpread = this.SpreadInterval(classTime.TimeFrom, classTime.TimeTo);
        List<TimeSpan> incidenceIntervalSpread = this.SpreadInterval(incidence.TimeFrom, incidence.TimeTo);

        bool incidenceAffectsClassTime = classTimeIntervalSpread.Intersect(incidenceIntervalSpread).Any();

        return incidenceAffectsClassTime;
    }
}