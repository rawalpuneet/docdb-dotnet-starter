namespace docdb_dotnet_starter.Models
{
    public class RestaurantsDatabaseSettings : IRestaurantsDatabaseSettings
    {
        public string LocationsCollectionName { get; set; }
        public string pathToCAFile { get; set; }
        public string connTemplate { get; set; }
        public string readPreference { get; set; }
        public string DatabaseName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string clusterEndpoint { get; set; }
        public string secretName {get; set; }
    }

    public interface IRestaurantsDatabaseSettings
    {
        public string LocationsCollectionName { get; set; }
        public string pathToCAFile { get; set; }
        public string connTemplate { get; set; }
        public string readPreference { get; set; }
        public string DatabaseName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string clusterEndpoint { get; set; }
        public string secretName {get; set; }
    }
}