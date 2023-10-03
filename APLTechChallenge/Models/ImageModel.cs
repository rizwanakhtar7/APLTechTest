using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APLTechChallenge.Models
{
    public class ImageModel
    {
        [DisplayName("Upload Image")]
        public string? FileDetails { get; set; }


        [Required(ErrorMessage = "Please select a file before uploading")]
        [Display(Name = "Upload File")]
        public IFormFile? File { get; set; }
    }
}
