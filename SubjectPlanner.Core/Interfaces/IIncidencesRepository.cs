namespace SubjectPlanner.Core;

public interface IIncidencesRepository
{
    public List<Incidence> AffectingClassDay(DateTime classDate);
}