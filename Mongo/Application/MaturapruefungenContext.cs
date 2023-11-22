using Bogus;
using MongoDB.Driver;
using Mongo.Models;

namespace Mongo.Application;

public class MaturapruefungenContext
{
    public static int _id_schueler = 1;
    public static int _id_pruefungen = 1;
    public MongoClient Client { get; }
    public static string maturapruefungenName = "maturapruefungen";
    public IMongoDatabase Db { get; }

    public IMongoCollection<Maturapruefungen> Maturapruefungen =>
        Db.GetCollection<Maturapruefungen>(maturapruefungenName);

    private MaturapruefungenContext(MongoClient client, IMongoDatabase db)
    {
        Client = client;
        Db = db;
    }

    public void clearMaturpruefungen()
    {
        Db.DropCollection(maturapruefungenName);
    }

    public static MaturapruefungenContext connect(string connection="mongodb://localhost:27017")
    {
        var settings = MongoClientSettings.FromConnectionString(connection);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(5);
        var client = new MongoClient(settings);
        var db = client.GetDatabase("Indra");
        return new MaturapruefungenContext(client: client, db: db);
    }

    public void Seed()
    {
        Db.DropCollection(maturapruefungenName);
        Randomizer.Seed = new Random(666);
        Maturapruefungen.InsertMany(MockMaturaPruefung());
    }


    public static List<Schueler> MockSchueler(int amount)
    {
        var _sl = new List<Schueler>();
        var faker = new Faker("de");
        List<string> lehrer = new List<string> { "ZUM", "UNG", "SLZ", "STA", "BH" };
        List<MockFach> _mf = new List<MockFach>
        {
            new MockFach("POS", "fachtheorie", new List<string>() { "UNG", "SLZ", "EN" }),
            new MockFach("AM", "schriflich", new List<string> { "POL", "GA" }),
            new MockFach("DE", "schriflich", new List<string> { "SIL", "DES", "CEL" }),
            new MockFach("DE", "muendlich", new List<string> { "SIL", "DES", "CEL" }),
            new MockFach("PRE", "muendlich", new List<string> { "ZUM", "SCK" }),
            new MockFach("GAD", "muendlich", new List<string> { "SCK", "LO" }),
            new MockFach("OPS", "muendlich", new List<string> { "BH", "STA" }),
            new MockFach("BAP", "muendlich", new List<string> { "UNG", "GRU" }),
            new MockFach("E", "schriflich", new List<string> { "SAC", "ME", "LLL" }),
            new MockFach("E", "muendlich", new List<string> { "SAC", "ME", "LLL" })
        };
        for (int i = 0; i < amount; i++)
        {
            var fach = faker.PickRandom(_mf);
            _sl.Add(new Schueler(id: _id_schueler, firstName: faker.Name.FirstName(), lastName: faker.Name.LastName(),
                fach: fach.name, pruefer: faker.PickRandom(fach.pruefer), note: faker.Random.Int(1, 5), typ: fach.typ,
                da: faker.PickRandom(lehrer)));
            _id_schueler++;
        }

        return _sl;
    }

    public static List<Maturapruefungen> MockMaturaPruefung()
    {
        var faker = new Faker();
        List<string> classes = new List<string> { "5EHIF", "5AHIF", "5BHIF" };
        List<string> years = new List<string> { "2022/23", "2021/22", "2020/21" };
        List<Maturapruefungen> _mp = new List<Maturapruefungen>();
        foreach (var c in classes)
        {
            foreach (var y in years)
            {
                var mp = new Maturapruefungen(id: _id_pruefungen , name: c, sj: y);
                mp.StudentPruefungen.AddRange(MockSchueler(5));
                _id_pruefungen++;
                _mp.Add(mp);
                
            }
        }

        return _mp;
    }
}