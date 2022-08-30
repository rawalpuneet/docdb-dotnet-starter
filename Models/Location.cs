using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace docdb_dotnet_starter.Models
{
    [BsonIgnoreExtraElements]
    public class Location
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        
        public string Id { get; set; }
        public string URL { get; set; }

        public string name { get; set; }

        public string address { get; set; }
        public string outcode { get; set; }

        public string postcode { get; set; }
  
        public float rating { get; set; }

        public string type_of_food {get; set;} 

    }
}