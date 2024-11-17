using NotificationApi.interfaces;
using NotificationApi.Models;
using Google.Cloud.Datastore.V1;

namespace NotificationApi.Repository;

public class MessageRepository : IMessageRepository
{
    private string PROJECTID = "gcp-wow-food-wlx-mpp-psh-dev";
    private string KIND = "nc_messages";
    private DatastoreDb db;

    public MessageRepository()
    {
        db = DatastoreDb.Create(PROJECTID);
    }

    public static Message transform(Entity entity)
    {
        return new Message
        (
            id: (string)entity["id"],
            userId: (string)entity["userId"],
            title: (string)entity["title"],
            body: (string)entity["body"],
            ctaLabel: (string)entity["ctaLabel"],
            ctaUrl: (string)entity["ctaUrl"],
            campaignCode: (string)entity["campaignCode"],
            campaignVariant: (string)entity["campaignVariant"],
            ttl: (DateTime)entity["ds_ttl"],
            createdOn: (string)entity["createdOn"]
        );
    }


    public Message AddMessage(Message message)
    {
        try
        {
            KeyFactory keyFactory = db.CreateKeyFactory(KIND);
            Key key = keyFactory.CreateKey($"{message.id}-{message.userId}");
            var entity = new Entity
            {
                Key = key,
                ["id"] = message.id,
                ["userId"] = message.userId,
                ["campaignCode"] = message.campaignCode,
                ["body"] = message.body,
                ["title"] = message.title,
                ["ctaUrl"] = message.ctaUrl,
                ["ctaLabel"] = message.ctaLabel,
                ["campaignVariant"] = message.campaignVariant,
                ["createdOn"] = message.createdOn,
                ["ds_ttl"] = message.ttl
            };

            using (DatastoreTransaction transaction = db.BeginTransaction())
            {
                transaction.Upsert(entity);
                transaction.Commit();
            }

            return transform(entity);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("faile to save entity", ex);
        }

    }

    public List<Message> GetMessageByUserId(string userId)
    {
        GqlQuery gqlQuery = new GqlQuery
        {
            QueryString = "SELECT * FROM nc_messages WHERE userId = @userId LIMIT @limit",
            NamedBindings = {
                    { "userId", userId },
                    { "limit", 20 }
                }
        };
        DatastoreQueryResults results = db.RunQuery(gqlQuery);
        List<Message> messages = new List<Message>();
        foreach (Entity entity in results.Entities)
        {
            messages.Add(transform(entity));
        }
        return messages;
    }
}