namespace SubjectPlanner.Core;

public interface ISubject
{
    public IEnumerable<Schedule> Schedules { get; set; }
    public double Hours { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}