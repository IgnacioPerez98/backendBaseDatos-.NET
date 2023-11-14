using MongoDB.Driver;
using System.Text.Json;
using MongoDB.Bson;

namespace backendBaseDatos.Servicios.MongoDB
{
    public class BaseMongoDB
    {
        public string Usuario { get; set; } = "admin";
        public string Contrasena { get; set; } = "admin";
        public string Server { get; set; } = "localhost";
        public string Port { get; set; } = "27017";

        protected const string clinicaDDBB = "horariosclinica"; 

        private IMongoClient Cliente = null;
        private bool DatabaseExists { get; set; }=  false;

        private IMongoClient getConnection()
        {
            string connectionString = $"mongodb://{Usuario}:{Contrasena}@{Server}:{Port}";
            if(Cliente == null)
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
                Cliente = new MongoClient(settings);
            }
            return Cliente;
        }

        protected IMongoDatabase obtenerBBDD()
        {
            return getConnection().GetDatabase(clinicaDDBB);
        }
    }
}
