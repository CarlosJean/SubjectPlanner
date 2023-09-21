namespace SubjectPlanner.Core;
public interface IScheduleRepository
{
    public List<ClassTime> AvailableClassTimes(ClassDay classDay);
}