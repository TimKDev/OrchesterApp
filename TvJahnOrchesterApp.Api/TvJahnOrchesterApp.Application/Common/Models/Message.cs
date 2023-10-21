namespace TvJahnOrchesterApp.Application.Common.Models
{
    public record Message(string[] To, string Subject, string Content);
}