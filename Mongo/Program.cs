using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Application;
using Mongo.Models;
using MongoDB.Bson;

class Program
{
    static void Main(string[] args)
    {
        var maturapruefungenContext = MaturapruefungenContext.connect();

        try
        {
            maturapruefungenContext.Seed();

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("TestDB");
            db.CreateCollection("IndraTest");
            var c = maturapruefungenContext.Maturapruefungen;
            // var c = maturapruefungenContext.Db.GetCollection<Maturapruefungen>(MaturapruefungenContext.maturapruefungenName);
            Console.WriteLine($" Number of Documents: {c.CountDocuments(new BsonDocument())}");
            // no project and project

            var documents = c.Find(_ => true).ToList();
            foreach (var doc in documents)
            {
                Console.WriteLine($"Only Name:{
                    doc.Name.ToJson()
                }");
            }

            var names_only = c.Find(_ => true).Project(a => a.Name).ToList();

            foreach (var doc in names_only)
            {
                Console.WriteLine($"Projection Name:{doc.ToJson()}");
            }

            // get 5ehif from year 21/22

            var ehif = c.Find(a => a.Name == "5EHIF" & a.SJ == "2021/22").First();
            Console.WriteLine($"Bevore Update: Name: {ehif.Name} SJ: {ehif.SJ}");

            // change some data 
            ehif.SJ = "2019/20";
            c.ReplaceOne(m => ehif.Id == m.Id, ehif);
            var ehif_2 = c.Find(a => a.Name == "5EHIF" & a.SJ == ehif.SJ).First();
            Console.WriteLine($"After Update: Name: {ehif.Name} SJ: {ehif.SJ}");

            
        }
        catch (Exception e1)

        {
            Console.WriteLine(e1.Message);
        }
    }
}