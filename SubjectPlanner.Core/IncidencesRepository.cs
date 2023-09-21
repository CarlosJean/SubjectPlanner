
namespace SubjectPlanner.Core;

public class IncidencesRepository : IIncidencesRepository{
    public List<Incidence> AffectingClassDay(ClassDay classDay)
    {          
        List<Incidence> incidences = new List<Incidence>()
        .Where(incidence => incidence.Date == classDay.Date 
            && !(incidence.TimeFrom < classDay.ClassTime.TimeFrom 
                && incidence.TimeTo < classDay.ClassTime.TimeTo))
        .ToList();

        return incidences;
    }
}