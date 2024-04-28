using Microsoft.AspNetCore.Http;

namespace Services.Abstractions.DTO
{
    public class FileUploadModel
    {
        public IFormFile File { get; set; }

        public Method Method { get; set; }

        public string Criterias { get; set; }
    }

    public enum Method
    {
        Vikor,
        Topsis
    }
}