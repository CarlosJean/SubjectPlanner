namespace SubjectPlanner.Core;

public class ClassDay
{
    public DateTime Date { get; set; }
    public ClassTime ClassTime { get; set; } = new ClassTime();
}