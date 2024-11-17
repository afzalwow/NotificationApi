using Microsoft.AspNetCore.Mvc;
using NotificationApi.interfaces;
using NotificationApi.Models;
namespace NotificationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class MessageController : ControllerBase
{

    private readonly ILogger<MessageController> logger;
    private readonly IMessageRepository messageRepository;

    public MessageController(ILogger<MessageController> logger, IMessageRepository messageRepository)
    {
        this.logger = logger;
        this.messageRepository = messageRepository;
    }

    [HttpGet("{userId}")] 
    public IEnumerable<Message> GetMessages(string userId){
        logger.LogInformation($"fetching messages for userId: {userId}");
        return messageRepository.GetMessageByUserId(userId);
    }

    [HttpPut]
    public Message putMessage(Message message) {
        logger.LogInformation($"adding/updating(upsert) notifications for userId: {message.userId}");
        return messageRepository.AddMessage(message);
    }

}