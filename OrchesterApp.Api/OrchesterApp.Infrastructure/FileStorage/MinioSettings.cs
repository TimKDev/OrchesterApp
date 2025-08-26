﻿namespace OrchesterApp.Infrastructure.FileStorage;

public class MinioSettings
{
    public const string SectionName = "MinIO";

    public string Endpoint { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public bool Secure { get; set; } = false;
    public string BucketName { get; set; } = string.Empty;
}