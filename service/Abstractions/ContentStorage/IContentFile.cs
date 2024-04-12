// Copyright (c) Microsoft. All rights reserved.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.KernelMemory.ContentStorage;
public interface IContentFile
{
    string? Volume { get; }
    string? RelativePath { get; }
    string? FileName { get; }
    string? ContentType { get; }
    DateTimeOffset? LastWrite { get; }
    Func<Task<Stream>> StreamAsync { get; }
    long Length { get; }
}

public sealed class StreamableContentFile : IContentFile, IDisposable
{
    private Stream? _stream;
    public StreamableContentFile()
    {
        this.StreamAsync = () => Task.FromResult<Stream>(new MemoryStream());
    }
    public StreamableContentFile(string volume, string relPath, string fileName, DateTimeOffset lastWriteTimeUtc, Func<Task<Stream>> asyncStreamDelegate, long contentLength, string contentType = "application/octet-stream")
    {
        this.Volume = volume;
        this.RelativePath = relPath;
        this.FileName = fileName;
        this.ContentType = contentType;
        this.LastWrite = lastWriteTimeUtc;
        this.StreamAsync = async () =>
        {
            this._stream = await asyncStreamDelegate().ConfigureAwait(false);
            return this._stream;
        };
        this.Length = contentLength;
    }

    public string? Volume { get; } = null;
    public string? RelativePath { get; } = null;
    public string? FileName { get; } = null;
    public string? ContentType { get; } = null;
    public DateTimeOffset? LastWrite { get; } = null;
    public Func<Task<Stream>> StreamAsync { get; }
    public long Length { get; } = 0;

    public void Dispose()
    {
        if (this._stream != null)
        {
            this._stream.Close();
            this._stream.Dispose();
        }
    }
}
