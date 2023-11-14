using backendBaseDatos.Models;

namespace backendBaseDatos.Servicios.MongoDB;

public class ClinicaMongo:BaseMongoDB
{
    public void GuardarTurnos(List<TurnoClinica> turnos)
    {
        //Guarda los turnos la primera vez que se generan.
    }
    
    
    public void ReservarTurno(TurnoClinica turno)
    {
        //Recibe un turno, para cambiarle el estado y guardarlo
    }

    public List<TurnoClinica> ObtenerTurnos(PeriodoActualizacion periodo)
    {
        if (periodo == null) return null;
        //Devuelve los turnos disponibles y ya resevados.
        //En el front mostrar como no disponibles los que estan reservados.
        throw new NotImplementedException();
    }


}