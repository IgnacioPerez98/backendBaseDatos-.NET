using backendBaseDatos.Models;
using MongoDB.Driver;

namespace backendBaseDatos.Servicios.MongoDB;

public class ClinicaMongo:BaseMongoDB
{
    public void GuardarTurnos(List<TurnoClinica> turnos,string collectionname)
    {
        var db = this.obtenerBBDD();
        var coleccion =  db.GetCollection<TurnoClinica>(collectionname);
        coleccion.InsertMany(turnos); 
        //Guarda los turnos la primera vez que se generan.
    }
    
    
    public void ReservarTurno(TurnoClinica turno,string collectionName)
    {
        var copia = new TurnoClinica()
        {
            HoraInicio = turno.HoraInicio,
            NumeroAgenda = turno.NumeroAgenda,
            HoraFin = turno.HoraFin,
            EstaReservado = true
        };
        var filter = Builders<TurnoClinica>.Filter.Eq("HoraFin", turno.HoraFin);
        var respuesta = obtenerBBDD().GetCollection<TurnoClinica>(collectionName).ReplaceOne(filter, copia);
        if (respuesta.ModifiedCount == 0) throw new Exception("No se realizaron cambios.");
        //Recibe un turno, para cambiarle el estado y guardarlo
    }

    public List<TurnoClinica> ObtenerTurnos(PeriodoActualizacion periodo)
    {

        var db = this.obtenerBBDD();
        var col = db.GetCollection<TurnoClinica>(periodo.ToString());
        var items = col.CountDocuments(Builders<TurnoClinica>.Filter.Empty);
        if(items == 0)
        {
            ServicioHorariosClinica srv = new ServicioHorariosClinica(periodo);
            GuardarTurnos(srv.TurnosDelPeriodo,periodo.ToString());
            return srv.TurnosDelPeriodo;
        }
        else
        {
            var res = col.Find(Builders<TurnoClinica>.Filter.Empty)
                      .Project<TurnoClinica>(Builders<TurnoClinica>.Projection.Exclude("_id"));
            return res.ToList();

        }
    }


}