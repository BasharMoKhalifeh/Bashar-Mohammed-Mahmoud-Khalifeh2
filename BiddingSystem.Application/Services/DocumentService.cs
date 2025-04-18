using System.IO;
using BiddingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TMS.Infrastructure.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly string _baseUploadPath;
        private readonly IConfiguration _config;

        public DocumentService(IConfiguration config)
        {
            _config = config;
            _baseUploadPath = _config["DocumentSettings:UploadsFolder"];

            // Ensure base upload directory exists
            if (!Directory.Exists(_baseUploadPath))
            {
                Directory.CreateDirectory(_baseUploadPath);
            }
        }

        public async Task<string> UploadTenderDocumentAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_baseUploadPath, "tenders");
            return await UploadFileAsync(file, uploadsFolder);
        }

        public async Task<string> UploadBidDocumentAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_baseUploadPath, "bids");
            return await UploadFileAsync(file, uploadsFolder);
        }

        private async Task<string> UploadFileAsync(IFormFile file, string targetFolder)
        {
            if (!Directory.Exists(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(targetFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<FileStreamResult> DownloadDocumentAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Document not found");
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return new FileStreamResult(memory, GetContentType(filePath))
            {
                FileDownloadName = Path.GetFileName(filePath)
            };
        }

        public void DeleteDocument(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private static string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream",
            };
        }

       
    }
}