using System;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Shared.Data
{
    public class MongoDbDataHelper
    {
        private IMongoDatabase db;

        public MongoDbDataHelper()
        {
            var client = new MongoClient(ConstantData.MongoDbConnectionString);
            db = client.GetDatabase(ConstantData.MongoDbName);
        }

        public void InsertDocument<T>(T document, string collectionName = "")
        {
            var collection = db.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? ConstantData.MongoDbCollection : collectionName);
            collection.InsertOne(document);
        }

        public List<T> LoadAllDocuments<T>(string collectionName = "")
        {
            try
            {
                var collection = db.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? ConstantData.MongoDbCollection : collectionName);
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public T LoadDocumentById<T>(Guid id, string collectionName = "")
        {
            var collection = db.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? ConstantData.MongoDbCollection : collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return collection.Find(filter).First();
        }

        public T LoadDocumentByQueueActionIdentifier<T>(string queueActionIdentifier)
        {
            var collection = db.GetCollection<T>(ConstantData.MongoDbCollection);
            var filter = Builders<T>.Filter.Eq("QueueActionIdentifier", queueActionIdentifier);
            return collection.Find(filter).FirstOrDefault();
        }

        public T LoadDocumentByMessageQueueIdentifier<T>(string messageQueueIdentifier)
        {
            var collection = db.GetCollection<T>(ConstantData.MongoDbCollectionForQueueManager);
            var filter = Builders<T>.Filter.Eq("MessageQueueIdentifier", messageQueueIdentifier);
            return collection.Find(filter).FirstOrDefault();
        }

        public void UpdateDocument<T>(Guid id, T document, string collectionName = "")
        {
            var collection = db.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? ConstantData.MongoDbCollection : collectionName);
            var result = collection.ReplaceOne(new BsonDocument("_id", id), document, new UpdateOptions { IsUpsert = false });
        }

        public void UpsertDocument<T>(Guid id, T document, string collectionName = "")
        {
            var collection = db.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? ConstantData.MongoDbCollection : collectionName);
            var result = collection.ReplaceOne(new BsonDocument("_id", id), document, new UpdateOptions { IsUpsert = true });
        }

        public void DeleteDocument<T>(Guid id, string collectionName = "")
        {
            var collection = db.GetCollection<T>(string.IsNullOrEmpty(collectionName) ? ConstantData.MongoDbCollection : collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            collection.DeleteOne(filter);
        }
    }
}
