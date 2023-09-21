namespace SubjectPlanner.Core;

public class SubjectRepository
{
    protected IScheduleRepository scheduleRepository;
    protected NextClassDayCalculation nextClassDayCalculation;
    public SubjectRepository(IScheduleRepository scheduleRepository, NextClassDayCalculation nextClassDayCalculation)
    {
        this.scheduleRepository = scheduleRepository;
        this.nextClassDayCalculation = nextClassDayCalculation;
    }

    public DateTime SubjectLastDay(ref Subject subject)
    {

        ClassDay classDay = this.SubjectStartClassDay(subject);
        classDay.Date = subject.StartDate;

        while (subject.Hours > 0)
        {
            List<ClassTime> availableClassTimes = this.scheduleRepository.AvailableClassTimes(classDay);

            foreach (ClassTime classTime in availableClassTimes)
            {
                double hoursDiff = (classTime.TimeTo - classTime.TimeFrom).TotalHours;

                subject.Hours -= hoursDiff;
                if (subject.Hours < 0)
                {
                    hoursDiff = (classTime.TimeTo - classTime.TimeFrom).TotalHours + subject.Hours;
                };

                classDay.Date = classDay.Date.AddHours(hoursDiff);
                subject.EndDate = classDay.Date;
            }

            classDay = this.nextClassDayCalculation.NextClassDay(classDay, subject.Schedules);
        }

        return subject.EndDate;
    }

    private ClassDay SubjectStartClassDay(Subject subject)
    {

        ClassTime? classTime = subject.Schedules
            .Where(schedule => schedule.DayOfWeek == subject.StartDate.DayOfWeek)
            .First()
            .ClassTime;

        ClassDay classDay = new()
        {
            Date = subject.StartDate,
            ClassTime = classTime ?? new ClassTime(),
        };

        return classDay;
    }
}