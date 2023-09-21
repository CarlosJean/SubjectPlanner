namespace SubjectPlanner.Core;

public interface IIncidencesRepository
{
    public List<Incidence> AffectingClassDay(ClassDay classDay);
}