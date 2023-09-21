
namespace SubjectPlanner.Core;
public class Subject : ISubject
{
    public IEnumerable<Schedule> Schedules { get; set;} = new List<Schedule>();
    public double Hours { get; set;}
    public DateTime StartDate { get; set;}
    public DateTime EndDate { get; set;}
}