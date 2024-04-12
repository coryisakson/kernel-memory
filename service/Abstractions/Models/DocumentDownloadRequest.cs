// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;

namespace Microsoft.KernelMemory;

public class DocumentDownloadRequest
{
    public DocumentDownloadRequest(string index, string documentId, string fileName, MemoryFilter filter=null, ICollection<MemoryFilter>? filters=null)
    {
        this.Index = index;
        this.DocumentId = documentId;
        this.FileName = fileName;
        this.filter = filter;
        this.filters=filters;
    }

    public string Index { get; set; } = string.Empty;

    public string DocumentId { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public MemoryFilter? filter = null;

    public ICollection<MemoryFilter>? filters = null;
}
