namespace SubjectPlanner.Core;

public class NextClassDayCalculation
{
    public ClassDay NextClassDay(ClassDay currentClassDay, IEnumerable<Schedule> schedules)
    {

        DayOfWeek currentDayOfWeek = currentClassDay
            .Date
            .DayOfWeek;

        bool isValidDate = this.IsValidDate(currentDayOfWeek, schedules);

        ClassDay classDay = new ClassDay();
        if (isValidDate)
        {
            float daysToSkip = this.SkipDaysToNext(currentDayOfWeek, schedules);

            DateTime nextClassDayDate = currentClassDay.Date.AddDays(daysToSkip);
            classDay = new ClassDay { Date = nextClassDayDate };
        };

        return classDay;
    }

    private int CurrentClassDayScheduleIndex(DayOfWeek currentDayOfWeek, IEnumerable<Schedule> SubjectSchedule)
    {
        int scheduleIndex = SubjectSchedule
                .ToList()
                .FindIndex(s => s.DayOfWeek == currentDayOfWeek);

        return scheduleIndex;
    }

    private bool IsValidDate(DayOfWeek currentDayOfWeek, IEnumerable<Schedule> SubjectSchedule)
    {
        try
        {
            int scheduleIndex = this.CurrentClassDayScheduleIndex(currentDayOfWeek, SubjectSchedule);
            bool scheduleFound = (scheduleIndex >= 0);

            return scheduleFound;
        }
        catch (System.Exception)
        {

            throw;
        }

    }

    private float SkipDaysToNext(DayOfWeek currentDayOfWeek, IEnumerable<Schedule> SubjectSchedule)
    {
        int currentDateIndex = this.CurrentClassDayScheduleIndex(currentDayOfWeek, SubjectSchedule);

        int lastScheduleIndex = SubjectSchedule.ToList().Count - 1;
        bool isCurrentDayLast = (currentDateIndex == lastScheduleIndex);

        int skippDays = 0;
        if (isCurrentDayLast)
        {
            int firstScheduleDayOfWeek = (int)SubjectSchedule
                .ToList()[0]
                .DayOfWeek;
            skippDays = 7 - (int)currentDayOfWeek + firstScheduleDayOfWeek;
        }
        else
        {
            int nextDayOfWeek = (int)SubjectSchedule.ToList()[currentDateIndex + 1].DayOfWeek;
            skippDays = nextDayOfWeek - (int)currentDayOfWeek;
        }

        return skippDays;
    }
}