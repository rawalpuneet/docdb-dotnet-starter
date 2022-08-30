using System;
using System.Text;
using docdb_dotnet_starter.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;

namespace docdb_dotnet_starter.Services
{
    public class LocationsService
    {
        private readonly IMongoCollection<Location> _locations;
        private SecretsManagerCache cache = new SecretsManagerCache();

        public LocationsService(IRestaurantsDatabaseSettings settings)
        {
            string  docdbSecret = settings.secretName;
            var secret = this.GetDocDBSecret(docdbSecret);
            secret.Wait();

            

            string connectionString = String.Format(settings.connTemplate, secret.Result.user, secret.Result.pass, 
                                                    settings.clusterEndpoint, settings.clusterEndpoint, settings.readPreference);
            var settings1 = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings1);
            var database = client.GetDatabase(settings.DatabaseName);
            _locations = database.GetCollection<Location>(settings.LocationsCollectionName);
        }

        public async Task<(string user, string pass)> GetDocDBSecret(string secretId)
        {
            var sec = await this.cache.GetSecretString(secretId);
            var output = Newtonsoft.Json.Linq.JObject.Parse(sec);
            //Console.WriteLine(output);
            return ( user: output["username"].ToString(), pass: output["password"].ToString());
        }

        public List<Location> Get() => _locations.Find(location => true).Limit(100).ToList();

        public Location Get(string id) => _locations.Find(location => location.Id == id).FirstOrDefault();

        public Location Create(Location location)
        {
            _locations.InsertOne(location);

            return location;
        }

        public void Update(string id, Location updatedLocation) => _locations.ReplaceOne(location => location.Id == id, updatedLocation);

        public void Delete(Location locationForDeletion) => _locations.DeleteOne(location => location.Id == locationForDeletion.Id);

        public void Delete(string id) => _locations.DeleteOne(location => location.Id == id);
    }
}