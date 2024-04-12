// Copyright (c) Microsoft. All rights reserved.

using Microsoft.KernelMemory.ContentStorage;

namespace Microsoft.KernelMemory;

public class DocumentDownloadResponse
{
    public DocumentDownloadResponse(IContentFile contentFile, DocumentDownloadRequest downloadRequest)
    {
        this.ContentFile = contentFile;
        this.DownloadRequest = downloadRequest;
    }

    public IContentFile ContentFile { get; set; }
    public DocumentDownloadRequest DownloadRequest { get; set; }
}
