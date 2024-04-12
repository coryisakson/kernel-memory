// Copyright (c) Microsoft. All rights reserved.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.KernelMemory.Models;
public class DocumentQuery
{
    [JsonPropertyName("index")]
    [JsonPropertyOrder(0)]
    public string? Index { get; set; } = string.Empty;

    [JsonPropertyName("documentId")]
    [JsonPropertyOrder(0)]
    public string? DocumentId { get; set; } = string.Empty;

    [JsonPropertyName("fileName")]
    [JsonPropertyOrder(1)]
    public string FileName { get; set; } = string.Empty;

    [JsonPropertyName("filters")]
    [JsonPropertyOrder(10)]
    public List<MemoryFilter> Filters { get; set; } = new();
}
