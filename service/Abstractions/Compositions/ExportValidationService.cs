// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.KernelMemory.ContentStorage;
using Microsoft.KernelMemory.Search;
using System.Linq;

namespace Microsoft.KernelMemory.Compositions;

public class ExportValidationService : IExportValidationService
{
    private readonly IContentStorage _contentStorage;
    private readonly ISearchClient _search;

    public ExportValidationService(IContentStorage storage, ISearchClient search)
    {
        this._contentStorage = storage;
        this._search = search;
    }

    // TODO: Tests for this!


    /// <inheritdoc />
    public async Task<IContentFile> FindAndExportDocumentAsync(
        string index,
        string documentId,
        string fileName,
        ICollection<MemoryFilter>? filters = null,
        CancellationToken cancellationToken = default)
    {
        // validate that the requested file meets the filter criteria
        if (filters != null && filters.Count > 0)
        {
            SearchResult result = await this._search.SearchAsync(index: index,
                query: string.Empty,
                filters: filters,
                limit: 1,
                cancellationToken: cancellationToken).ConfigureAwait(false);
            if (result == null || result.NoResult)
            {
                throw new KernelMemoryException("Search found no documents matching the requested filters");
            }

            Citation citation = result.Results.First();
            if (citation.DocumentId != documentId || citation.SourceName != fileName)
            {
                throw new KernelMemoryException("Search results do not contain the requested document");
            }
        }

        return await this._contentStorage.FileInfoAsync(
            index: index,
            documentId: documentId,
            fileName: fileName,
            logErrIfNotFound: true,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
