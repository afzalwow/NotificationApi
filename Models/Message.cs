using System.ComponentModel.DataAnnotations;

namespace NotificationApi.Models
{
    public record Message(
        [Required] string id,
        [Required] string userId,
        [Required] string title,
        [Required] string body,
        string ctaLabel,
        string ctaUrl,
        string campaignCode,
        string campaignVariant,
        DateTime ttl,
        string createdOn
    );
}