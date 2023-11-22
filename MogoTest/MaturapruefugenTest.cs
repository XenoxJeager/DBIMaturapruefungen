using Mongo.Application;
using Mongo.Models;
using MongoDB.Driver;

namespace MogoTest;

public class MaturapruefugenTest
{
    [Fact]
    public void InsertTest()
    {
        var context = MaturapruefungenContext.connect();
        context.clearMaturpruefungen();
        var pf = new Maturapruefungen(id: 1, name: "inserttest", sj: "2021");
        pf.StudentPruefungen.AddRange(MaturapruefungenContext.MockSchueler(1));
        context.Maturapruefungen.InsertOne(pf);

        Assert.True(context.Maturapruefungen.Find(a => a.Id == pf.Id).First().Id == pf.Id);
    }

    [Fact]
    public void ReadTest()
    {
        var context = MaturapruefungenContext.connect();
        context.clearMaturpruefungen();

        
        var pf = new Maturapruefungen(id: 2, name: "readTest", sj: "2022");
        context.Maturapruefungen.InsertOne(pf);

        var readPf = context.Maturapruefungen.Find(a => a.Id == pf.Id).FirstOrDefault();

        Assert.NotNull(readPf);
        Assert.Equal(pf.Id, readPf.Id);
        context.Maturapruefungen.DeleteOne(a => a.Id == pf.Id);
    }

    [Fact]
    public void UpdateTest()
    {
        var context = MaturapruefungenContext.connect();
        context.clearMaturpruefungen();

        var pf = new Maturapruefungen(id: 3, name: "oldupdateTest", sj: "2023");
        context.Maturapruefungen.InsertOne(pf);

        pf.Name = "updatedTest";
        context.Maturapruefungen.ReplaceOne(a => a.Id == pf.Id, pf);

        var updatedPf = context.Maturapruefungen.Find(a => a.Id == pf.Id).FirstOrDefault();

        Assert.NotNull(updatedPf);
        Assert.Equal("updatedTest", updatedPf.Name);
        
        context.Maturapruefungen.DeleteOne(a => a.Id == pf.Id);
    }

    [Fact]
    public void DeleteTest()
    {
        var context = MaturapruefungenContext.connect();
        context.clearMaturpruefungen();

        var pf = new Maturapruefungen(id: 4, name: "deleteTest", sj: "2024");
        context.Maturapruefungen.InsertOne(pf);

        context.Maturapruefungen.DeleteOne(a => a.Id == pf.Id);

        var deletedPf = context.Maturapruefungen.Find(a => a.Id == pf.Id).FirstOrDefault();

        Assert.Null(deletedPf);
    }
}