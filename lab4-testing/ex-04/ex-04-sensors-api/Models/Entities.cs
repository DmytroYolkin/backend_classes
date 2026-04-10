using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Howest.lab2.ex04_sensor_monitoring.Models;

public class Sensor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? PhoneNumber { get; set; }
}

public class Reading
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string SensorId { get; set; } = string.Empty;
    
    public decimal Value { get; set; }
    public DateTime Timestamp { get; set; }
}
