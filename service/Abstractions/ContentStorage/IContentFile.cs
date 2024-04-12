// Copyright (c) Microsoft. All rights reserved.

using System;
using System.IO;

namespace Microsoft.KernelMemory.ContentStorage;
public interface IContentFile
{
    string? Volume { get; }
    string? RelativePath { get; }
    string? FileName { get; }
    string? ContentType { get; }
    DateTimeOffset? LastWrite { get; }
    Stream Stream { get; }
    long Length { get; }
}

public sealed class StreamableContentFile : IContentFile, IDisposable
{
    public StreamableContentFile()
    {
        this.Stream = new MemoryStream();
    }
    public StreamableContentFile(string volume, string relPath, string fileName, DateTime lastWriteTimeUtc, Stream stream, string contentType = "application/octet-stream")
    {
        this.Volume = volume;
        this.RelativePath = relPath;
        this.FileName = fileName;
        this.ContentType = contentType;
        this.LastWrite = lastWriteTimeUtc;
        this.Stream = stream;
        this.Length = stream.Length;
    }

    public string? Volume { get; } = null;
    public string? RelativePath { get; } = null;
    public string? FileName { get; } = null;
    public string? ContentType { get; } = null;
    public DateTimeOffset? LastWrite { get; } = null;
    public Stream Stream { get; }
    public long Length { get; } = 0;

    public void Dispose()
    {
        if (this.Stream != null)
        {
            this.Stream.Close();
            this.Stream.Dispose();
        }
    }
}
