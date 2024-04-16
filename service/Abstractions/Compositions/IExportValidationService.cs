// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.KernelMemory.ContentStorage;

namespace Microsoft.KernelMemory.Compositions;
public interface IExportValidationService
{
    Task<IContentFile> FindAndExportDocumentAsync(string index, string documentId, string fileName, ICollection<MemoryFilter>? filters = null, CancellationToken cancellationToken = default);
}
