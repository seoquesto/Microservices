using Microservices.Common.Api;
using Microservices.Common.MongoDb.Internal;
using Microservices.Common.Types;
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

    public static IAppBuilder AddMongo(this IAppBuilder appBuilder, string sectionName = SectionName)
    {
      var options = appBuilder.GetOptions<MongoOptions>(sectionName);
      appBuilder.Services.AddSingleton(options);

      appBuilder.Services.AddSingleton<IMongoClient>(sp =>
      {
        var options = sp.GetService<MongoOptions>();
        return new MongoClient(options.ConnectionString);
      });

      appBuilder.Services.AddTransient(sp =>
      {
        var options = sp.GetService<MongoOptions>();
        var client = sp.GetService<IMongoClient>();
        return client.GetDatabase(options.Database);
      });

      RegisterConventions();

      return appBuilder;
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

    public static IAppBuilder AddMongoRepository<TEntity, TIdentifiable>(this IAppBuilder appBuilder,
        string collectionName)
        where TEntity : IIdentifiable<TIdentifiable>
    {

      appBuilder.Services.AddTransient<IMongoRepository<TEntity, TIdentifiable>>(sp =>
     {
       var database = sp.GetService<IMongoDatabase>();
       return new MongoRepository<TEntity, TIdentifiable>(database, collectionName);
     });

      return appBuilder;
    }

  }
}