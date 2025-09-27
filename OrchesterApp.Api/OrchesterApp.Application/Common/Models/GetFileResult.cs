namespace TvJahnOrchesterApp.Application.Common.Models;

public record GetFileResult(string FileName, string ContentType, Stream FileStream);