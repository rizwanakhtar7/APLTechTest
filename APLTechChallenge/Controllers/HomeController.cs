using APLTechChallenge.Data;
using APLTechChallenge.Models;
using APLTechChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace APLTechChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IImageService _imageService;
        private readonly ApplicationDBContext _context;
        public HomeController(ILogger<HomeController> logger, IImageService imageService, ApplicationDBContext context)
        {
            _logger = logger;
            _imageService = imageService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(ImageModel imageModel)
        {
            

            if (!ModelState.IsValid)
            {
                return View("Upload", imageModel);
            }

            try
            {
               
                if (await _imageService.PostFileAsync(imageModel.File))
                {

                    ViewBag.Message = "File upload successfully!";

                    var latestImage = imageModel.File.FileName;

                    var auditRecord = new ImageAudit
                    {
                        Name = imageModel.File.FileName,
                        OfflineMode = true,
                        SentToAzure = false,
                        DateUploaded = DateTime.Now,
                        SuccessRecord = ViewBag.Message
                    
                    };

                    _context.ImageAudits.Add(auditRecord);
                    await _context.SaveChangesAsync();

                    ViewBag.LatestImage = latestImage;


                }
                else
                {
                    ViewBag.Message = "There was an Error when uploading the file!";

                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();

                var exceptionLogDTO = new AuditErrorLogs
                {
                    Name = imageModel.File.FileName,
                    OfflineMode = true,
                    SentToAzure = false,
                    DateUploaded = DateTime.Now,
                    ExceptionMessage = ViewBag.Message,
                    TimeStamp = DateTime.Now
                };

                _context.ErrorAuditLogs.Add(exceptionLogDTO);
                await _context.SaveChangesAsync();
            }

            return View("Upload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadToAzure(ImageModel imageModel)
        {
            
            if (!ModelState.IsValid)
            {
                return View("Upload", imageModel);
            }

            try
            {

                if (await _imageService.UploadImageToAzure(imageModel.File))
                {

                    ViewBag.Message = "File upload successfully!";

                    var latestImage = imageModel.File.FileName;

                    var auditRecord = new ImageAudit
                    {
                        Name = imageModel.File.FileName,
                        OfflineMode = false,
                        SentToAzure = true,
                        DateUploaded = DateTime.Now,
                        SuccessRecord = ViewBag.Message

                    };

                    _context.ImageAudits.Add(auditRecord);
                    await _context.SaveChangesAsync();

                    ViewBag.LatestImage = latestImage;


                }
                else
                {
                    ViewBag.Message = "File upload UNsuccessfully!";

                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();

                var exceptionLogDTO = new AuditErrorLogs
                {
                    Name = imageModel.File.FileName,
                    OfflineMode = false,
                    SentToAzure = true,
                    DateUploaded = DateTime.Now,
                    ExceptionMessage = ViewBag.Message,
                    TimeStamp = DateTime.Now
                };

                _context.ErrorAuditLogs.Add(exceptionLogDTO);
                await _context.SaveChangesAsync();
            }

            return View("Upload");

        }

        [HttpGet]
        public ActionResult AllImages()
        {
           var images = _context.ImageAudits.ToList();

            return View(images);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}