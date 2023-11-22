using MongoDB.Bson.Serialization.Attributes;

namespace Mongo.Models;

public record Schueler(int id, string firstName, string lastName, string fach, string pruefer, int note, 
    string typ, string da);