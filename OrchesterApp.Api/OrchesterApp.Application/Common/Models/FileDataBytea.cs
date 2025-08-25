namespace TvJahnOrchesterApp.Application.Common.Models;

public class FileDataBytea
{
    public Guid Id { get; set; }
    public byte[] Data { get; set; } = [];
}