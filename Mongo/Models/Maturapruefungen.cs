using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Models;

public class Maturapruefungen
{
    public Maturapruefungen(int id ,string name,string sj)
    {
        Id = id; 
        Name = name;
        SJ = sj;
    }



    [BsonId] public int Id { get; set; }
    public string Name { get; set; }
    public string SJ { get; set; }
    
    [BsonElement] 
    public List<Schueler> StudentPruefungen { get; set; } = new();
}