namespace SubjectPlanner.Test;

using Moq;
using SubjectPlanner.Core;

public class NextClassDayTest
{

    [Test]
    public void NoLastWeekClassDay()
    {

        IEnumerable<Schedule> schedules = new List<Schedule>{
            new Schedule{
                DayOfWeek = DayOfWeek.Monday
            },
            new Schedule{
                DayOfWeek = DayOfWeek.Friday
            },
        };
        
        ClassDay classDay = new(){
            Date = new DateTime(2023, 09, 18)
        };

        NextClassDayCalculation calculations = new();

        ClassDay newClassDay = calculations.NextClassDay(classDay, schedules);

        ClassDay expectedNewClassDay = new(){
            Date = new DateTime(2023, 09, 22)
        };

        Assert.That(newClassDay.Date, Is.EqualTo(expectedNewClassDay.Date));
    }

    [Test]
    public void LastWeekClassDay()
    {

        IEnumerable<Schedule> schedules = new List<Schedule>{
            new Schedule{
                DayOfWeek = DayOfWeek.Monday
            },
            new Schedule{
                DayOfWeek = DayOfWeek.Friday
            },
        };

        var schedulesRepositoryMock = new Mock<IScheduleRepository>();
        var subjectMock = new Mock<ISubject>();

        ISubject subject = subjectMock.Object;
        IScheduleRepository schedulesRepository = schedulesRepositoryMock.Object;
        
        ClassDay classDay = new(){
            Date = new DateTime(2023, 09, 22)
        };

        NextClassDayCalculation calculations = new NextClassDayCalculation();

        ClassDay newClassDay = calculations.NextClassDay(classDay, schedules);

        ClassDay expectedNewClassDay = new(){
            Date = new DateTime(2023, 09, 25)
        };

        Assert.That(newClassDay.Date, Is.EqualTo(expectedNewClassDay.Date));
    }
}