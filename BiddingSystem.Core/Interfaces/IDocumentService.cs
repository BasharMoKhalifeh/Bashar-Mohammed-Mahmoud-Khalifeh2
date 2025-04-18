using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiddingSystem.Application.Interfaces
{
    public interface IDocumentService
    {
        Task<string> UploadTenderDocumentAsync(IFormFile file);
        Task<string> UploadBidDocumentAsync(IFormFile file);
        Task<FileStreamResult> DownloadDocumentAsync(string filePath);
        public void DeleteDocument(string filePath);

    }
}
