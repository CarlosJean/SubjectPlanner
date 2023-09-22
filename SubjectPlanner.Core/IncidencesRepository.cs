
namespace SubjectPlanner.Core;

public class IncidencesRepository : IIncidencesRepository
{

    protected List<Incidence> incidences;
    public IncidencesRepository(List<Incidence> incidences)
    {
        this.incidences = incidences;
    }
    public List<Incidence> AffectingClassDay(ClassDay classDay)
    {
        List<Incidence> incidences = this.incidences
            .Where(incidence => incidence.Date.Date == classDay.Date.Date)
            .ToList();

        return incidences;
    }
}