using Moq;
using NUnit.Framework;
using SubjectPlanner.Core;

namespace Tests
{
    public class AvailableClassTimesTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IncidenceBetweenClassTIme()
        {
            ClassTime classTime = new()
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(17, 0, 0),
            };

            ClassDay classDay = new ClassDay
            {
                Date = new DateTime(2023, 09, 20),
                ClassTime = classTime
            };

            List<Incidence> incidences = new List<Incidence>{
                new Incidence{
                    Date = classDay.Date,
                    TimeFrom = new TimeSpan(10,0,0),
                    TimeTo = new TimeSpan(11,0,0),
                }
            };

            var incidencesRepositoryMock = new Mock<IIncidencesRepository>();
            incidencesRepositoryMock.Setup(incidencesRepository => incidencesRepository.AffectingClassDay(classDay))
            .Returns(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);

            List<ClassTime> availableClassTimes = scheduleRepository.AvailableClassTimes(classDay);

            List<ClassTime> expectedAvailableClassTimes = new(){
                new ClassTime(){
                    TimeFrom = new TimeSpan(8,0,0),
                    TimeTo = new TimeSpan(10,0,0),
                },
                new ClassTime(){
                    TimeFrom = new TimeSpan(11,0,0),
                    TimeTo = new TimeSpan(17,0,0),
                },
            };

            Assert.That(availableClassTimes[0].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[0].TimeFrom));
            Assert.That(availableClassTimes[0].TimeTo, Is.EqualTo(expectedAvailableClassTimes[0].TimeTo));
            Assert.That(availableClassTimes[1].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[1].TimeFrom));
            Assert.That(availableClassTimes[1].TimeTo, Is.EqualTo(expectedAvailableClassTimes[1].TimeTo));
        }
        
        [Test]
        public void SeveralIncidencesBetweenClassTIme()
        {
            ClassTime classTime = new()
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(17, 0, 0),
            };

            ClassDay classDay = new ClassDay
            {
                Date = new DateTime(2023, 09, 20),
                ClassTime = classTime
            };

            List<Incidence> incidences = new List<Incidence>{
                new Incidence{
                    Date = classDay.Date,
                    TimeFrom = new TimeSpan(10,0,0),
                    TimeTo = new TimeSpan(11,0,0),
                },
                new Incidence{
                    Date = classDay.Date,
                    TimeFrom = new TimeSpan(15,0,0),
                    TimeTo = new TimeSpan(16,0,0),
                }
            };

            var incidencesRepositoryMock = new Mock<IIncidencesRepository>();
            incidencesRepositoryMock.Setup(incidencesRepository => incidencesRepository.AffectingClassDay(classDay))
            .Returns(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);

            List<ClassTime> availableClassTimes = scheduleRepository.AvailableClassTimes(classDay);

            List<ClassTime> expectedAvailableClassTimes = new(){
                new ClassTime(){
                    TimeFrom = new TimeSpan(8,0,0),
                    TimeTo = new TimeSpan(10,0,0),
                },
                new ClassTime(){
                    TimeFrom = new TimeSpan(11,0,0),
                    TimeTo = new TimeSpan(15,0,0),
                },
                new ClassTime(){
                    TimeFrom = new TimeSpan(16,0,0),
                    TimeTo = new TimeSpan(17,0,0),
                },
            };

            Assert.That(availableClassTimes[0].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[0].TimeFrom));
            Assert.That(availableClassTimes[0].TimeTo, Is.EqualTo(expectedAvailableClassTimes[0].TimeTo));
            Assert.That(availableClassTimes[1].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[1].TimeFrom));
            Assert.That(availableClassTimes[1].TimeTo, Is.EqualTo(expectedAvailableClassTimes[1].TimeTo));
            Assert.That(availableClassTimes[2].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[2].TimeFrom));
            Assert.That(availableClassTimes[2].TimeTo, Is.EqualTo(expectedAvailableClassTimes[2].TimeTo));
        }

        [Test]
        public void IncidenceAtStartOfClassDay()
        {
            ClassTime classTime = new()
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(17, 0, 0),
            };

            ClassDay classDay = new ClassDay
            {
                Date = new DateTime(2023, 09, 20),
                ClassTime = classTime
            };

            List<Incidence> incidences = new List<Incidence>{
                new Incidence{
                    Date = classDay.Date,
                    TimeFrom = new TimeSpan(0,0,0),
                    TimeTo = new TimeSpan(10,0,0),
                }
            };

            var incidencesRepositoryMock = new Mock<IIncidencesRepository>();
            incidencesRepositoryMock.Setup(incidencesRepository => incidencesRepository.AffectingClassDay(classDay))
            .Returns(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);

            List<ClassTime> availableClassTimes = scheduleRepository.AvailableClassTimes(classDay);

            List<ClassTime> expectedAvailableClassTimes = new(){
                new ClassTime(){
                    TimeFrom = new TimeSpan(10,0,1),
                    TimeTo = new TimeSpan(17,0,0),
                },
            };

            Assert.That(availableClassTimes[0].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[0].TimeFrom));
            Assert.That(availableClassTimes[0].TimeTo, Is.EqualTo(expectedAvailableClassTimes[0].TimeTo));
        }
        
        [Test]
        public void IncidenceAtEndOfClassDay()
        {
            ClassTime classTime = new()
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(17, 0, 0),
            };

            ClassDay classDay = new ClassDay
            {
                Date = new DateTime(2023, 09, 20),
                ClassTime = classTime
            };

            List<Incidence> incidences = new List<Incidence>{
                new Incidence{
                    Date = classDay.Date,
                    TimeFrom = new TimeSpan(12,0,0),
                    TimeTo = new TimeSpan(23,59,59),
                }
            };

            var incidencesRepositoryMock = new Mock<IIncidencesRepository>();
            incidencesRepositoryMock.Setup(incidencesRepository => incidencesRepository.AffectingClassDay(classDay))
            .Returns(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);

            List<ClassTime> availableClassTimes = scheduleRepository.AvailableClassTimes(classDay);

            List<ClassTime> expectedAvailableClassTimes = new(){
                new ClassTime(){
                    TimeFrom = new TimeSpan(8,0,0),
                    TimeTo = new TimeSpan(11,59,59),
                },
            };

            Assert.That(availableClassTimes[0].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[0].TimeFrom));
            Assert.That(availableClassTimes[0].TimeTo, Is.EqualTo(expectedAvailableClassTimes[0].TimeTo));            
        }

        [Test]
        public void IncidenceOnWholeClassTIme()
        {
            ClassTime classTime = new()
            {
                TimeFrom = new TimeSpan(8, 0, 0),
                TimeTo = new TimeSpan(17, 0, 0),
            };

            ClassDay classDay = new ClassDay
            {
                Date = new DateTime(2023, 09, 20),
                ClassTime = classTime
            };

            List<Incidence> incidences = new List<Incidence>{
                new Incidence{
                    Date = classDay.Date,
                    TimeFrom = new TimeSpan(0,0,0),
                    TimeTo = new TimeSpan(23,59,59),
                }
            };

            var incidencesRepositoryMock = new Mock<IIncidencesRepository>();
            incidencesRepositoryMock.Setup(incidencesRepository => incidencesRepository.AffectingClassDay(classDay))
            .Returns(incidences);

            IScheduleRepository scheduleRepository = new ScheduleRepository(incidencesRepositoryMock.Object);

            List<ClassTime> availableClassTimes = scheduleRepository.AvailableClassTimes(classDay);

            List<ClassTime> expectedAvailableClassTimes = new(){
                new ClassTime(){
                    TimeFrom = new TimeSpan(8,0,0),
                    TimeTo = new TimeSpan(8,0,0),
                },
            };

            Assert.That(availableClassTimes[0].TimeFrom, Is.EqualTo(expectedAvailableClassTimes[0].TimeFrom));
            Assert.That(availableClassTimes[0].TimeTo, Is.EqualTo(expectedAvailableClassTimes[0].TimeTo));            
        }
    }
}