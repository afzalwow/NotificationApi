using NotificationApi.Models;

namespace NotificationApi.interfaces;
public interface IMessageRepository
{
    List<Message> GetMessageByUserId(string userId);
    Message AddMessage(Message message);
}