using Moq;
using NUnit.Framework;
using SubjectPlanner.Core;

namespace Tests
{
    public class SubjectEndDateTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void OneDayPerWeekThirtyMinutesClassNoIncidences()
        {
            ClassTime classTime = new ClassTime
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(12, 0, 0),
            };

            Subject subject = new()
            {
                Hours = .5,
                StartDate = new DateTime(2023, 09, 18, 8, 0, 0),
                Schedules = new List<Schedule>{
                    new Schedule{
                        DayOfWeek = DayOfWeek.Monday,
                        ClassTime = classTime
                    }
                },
            };

            ClassDay classDay = new();
            List<Incidence> incidences = new List<Incidence>();

            var incidencesRepositoryMock = new Mock<IIncidencesRepository>();
            incidencesRepositoryMock.Setup(incidencesRepository => incidencesRepository.AffectingClassDay(classDay))
            .Returns(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);
            NextClassDayCalculation nextClassDayCalculation = new NextClassDayCalculation();

            SubjectRepository subjectRepository = new(scheduleRepository, nextClassDayCalculation);

            subjectRepository.SubjectLastDay(ref subject);

            DateTime expectedEndDate = new DateTime(2023, 09, 18, 8, 30, 0);
            Assert.That(subject.EndDate, Is.EqualTo(expectedEndDate));
        }

        [Test]
        public void TwoDaysPerWeekClassEndDateIncidence()
        {
            ClassTime classTime = new ClassTime
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(12, 0, 0),
            };

            Subject subject = new()
            {
                Hours = 10,
                StartDate = new DateTime(2023, 09, 18, 8, 0, 0),
                Schedules = new List<Schedule>{
                    new Schedule{
                        DayOfWeek = DayOfWeek.Monday,
                        ClassTime = classTime
                    },
                    new Schedule{
                        DayOfWeek = DayOfWeek.Thursday,
                        ClassTime = classTime
                    },
                },
            };

            List<Incidence> incidences = new List<Incidence>(){
                new Incidence{
                    Date = new DateTime(2023,9,25),
                    TimeFrom = new TimeSpan(0,0,0),
                    TimeTo = new TimeSpan(23,59,59),
                },
            };

            var incidencesRepositoryMock = new Mock<IncidencesRepository>(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);
            NextClassDayCalculation nextClassDayCalculation = new NextClassDayCalculation();

            SubjectRepository subjectRepository = new(scheduleRepository, nextClassDayCalculation);

            subjectRepository.SubjectLastDay(ref subject);

            DateTime expectedEndDate = new DateTime(2023, 09, 28, 10, 0, 0);
            Assert.That(subject.EndDate, Is.EqualTo(expectedEndDate));
        }
    }
}