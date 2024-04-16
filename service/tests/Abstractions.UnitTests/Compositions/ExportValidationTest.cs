// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.Compositions;
using Microsoft.KernelMemory.ContentStorage;
using Microsoft.KernelMemory.Search;
using Moq;
using Xunit.Abstractions;

namespace Abstractions.UnitTests.Compositions;

public class ExportValidationTest
{
    [Fact]
    [Trait("Category", "UnitTest")]
    public async void ItBypassesValidationWhenFiltersEmpty()
    {
        // Arrange
        var mockStorage = new Mock<IContentStorage>();
        var mockSearch = new Mock<ISearchClient>();
        var service = new ExportValidationService(mockStorage.Object, mockSearch.Object);

        // Act
        await service.FindAndExportDocumentAsync("index", "documentId", "filename");

        // Assert
        mockStorage.Verify(x => x.FileInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>()), Times.Once()); ;
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async void ItThrowsExceptionWhenNoFiltersMatch()
    {
        // Arrange
        var mockStorage = new Mock<IContentStorage>();
        var mockSearch = new Mock<ISearchClient>();
        var service = new ExportValidationService(mockStorage.Object, mockSearch.Object);
        mockSearch.Setup(x => x.SearchAsync(
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<ICollection<MemoryFilter>>(),
             It.IsAny<double>(),
             1,
             It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SearchResult());

        // Act
        var ex = Record.ExceptionAsync(async () =>
         await service.FindAndExportDocumentAsync("index", "documentId", "filename",
            new List<MemoryFilter> { new MemoryFilter().ByTag("name", "value") })
        );

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<KernelMemoryException>(ex.Result);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ItThrowsExceptionWhenNoDocumentIdMatch()
    {
        // Arrange
        var mockStorage = new Mock<IContentStorage>();
        var mockSearch = new Mock<ISearchClient>();
        var service = new ExportValidationService(mockStorage.Object, mockSearch.Object);
        mockSearch.Setup(x => x.SearchAsync(
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<ICollection<MemoryFilter>>(),
             It.IsAny<double>(),
             1,
             It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SearchResult()
            {
                Results = new List<Citation>
            {
                new Citation { DocumentId = "not it" } }
            });

        // Act
        var ex = Record.ExceptionAsync(async () =>
         await service.FindAndExportDocumentAsync("index", "documentId", "filename",
            new List<MemoryFilter> { new MemoryFilter().ByTag("name", "value") })
        );

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<KernelMemoryException>(ex.Result);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public void ItThrowsExceptionWhenNoFilenameMatch()
    {
        // Arrange
        var mockStorage = new Mock<IContentStorage>();
        var mockSearch = new Mock<ISearchClient>();
        var service = new ExportValidationService(mockStorage.Object, mockSearch.Object);
        mockSearch.Setup(x => x.SearchAsync(
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<ICollection<MemoryFilter>>(),
             It.IsAny<double>(),
             1,
             It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SearchResult()
            {
                Results = new List<Citation>
            {
                new Citation { SourceName = "not it" } }
            });

        // Act
        var ex = Record.ExceptionAsync(async () =>
         await service.FindAndExportDocumentAsync("index", "documentId", "filename",
            new List<MemoryFilter> { new MemoryFilter().ByTag("name", "value") })
        );

        // Assert
        Assert.NotNull(ex);
        Assert.IsType<KernelMemoryException>(ex.Result);
    }

    [Fact]
    [Trait("Category", "UnitTest")]
    public async void ItReturnsContentWhenFiltersMatch()
    {
        // Arrange
        var mockStorage = new Mock<IContentStorage>();
        var mockSearch = new Mock<ISearchClient>();
        var service = new ExportValidationService(mockStorage.Object, mockSearch.Object);
        mockSearch.Setup(x => x.SearchAsync(
             It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<ICollection<MemoryFilter>>(),
             It.IsAny<double>(),
             1,
             It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SearchResult()
            {
                Results = new List<Citation>
            {
                new() { DocumentId = "documentId", SourceName = "filename" } }
            });

        // Act
        await service.FindAndExportDocumentAsync("index", "documentId","filename", 
            new List<MemoryFilter> { new MemoryFilter().ByTag("tag", "value") });

        // Assert
        mockSearch.Verify(x => x.SearchAsync(It.IsAny<string>(),
             It.IsAny<string>(),
             It.IsAny<ICollection<MemoryFilter>>(),
             It.IsAny<double>(),
             1,
             It.IsAny<CancellationToken>()), Times.Once()); ;
        mockStorage.Verify(x => x.FileInfoAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), true, It.IsAny<CancellationToken>()), Times.Once()); ;

    }
}


