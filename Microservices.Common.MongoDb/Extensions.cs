using Microservices.Common.Shell;
using Microservices.Common.MongoDb.Internal;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Microservices.Common.MongoDb
{
  public static class Extensions
  {
    private const string SectionName = "mongo";

    public static IShellBuilder AddMongo(this IShellBuilder shellBuilder, string sectionName = SectionName)
    {
      var options = shellBuilder.GetOptions<MongoOptions>(sectionName);
      shellBuilder.Services.AddSingleton(options);

      shellBuilder.Services.AddSingleton<IMongoClient>(sp =>
      {
        var options = sp.GetService<MongoOptions>();
        return new MongoClient(options.ConnectionString);
      });

      shellBuilder.Services.AddTransient(sp =>
      {
        var options = sp.GetService<MongoOptions>();
        var client = sp.GetService<IMongoClient>();
        return client.GetDatabase(options.Database);
      });

      RegisterConventions();

      return shellBuilder;
    }

    private static void RegisterConventions()
    {
      BsonSerializer.RegisterSerializer(typeof(decimal), new DecimalSerializer(BsonType.Decimal128));
      BsonSerializer.RegisterSerializer(typeof(decimal?),
          new NullableSerializer<decimal>(new DecimalSerializer(BsonType.Decimal128)));
      ConventionRegistry.Register("Conventions", new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new IgnoreExtraElementsConvention(true),
                new EnumRepresentationConvention(BsonType.String),
            }, _ => true);
    }

    public static IShellBuilder AddMongoRepository<TEntity, TIdentifiable>(
      this IShellBuilder shellBuilder, string collectionName)
      where TEntity : IIdentifiable<TIdentifiable>
    {

      shellBuilder.Services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
     {
       var database = sp.GetService<IMongoDatabase>();
       return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
     });

      return shellBuilder;
    }

  }
}