using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using Infrastructure.Persistence.Contexts;
using Application.Features.Identity.Users;
using Application.Models.Wrapper;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileSharesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public FileSharesController(ApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        // GET: api/FileShares
        [HttpGet]
        public async Task<IActionResult> GetFileShares()
        {
            var fileSharesInDb = await _context.FileShare.ToListAsync();

            return Ok(await ResponseWrapper<List<Domain.Entities.FileShare>>.SuccessAsync(data: fileSharesInDb));
        }

        // GET: api/FileShares/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileShare(Guid id)
        {

            var fileSharesInDb = await _context.FileShare.Where(x => x.ParentId == id).ToListAsync();

            return Ok(await ResponseWrapper<List<Domain.Entities.FileShare>>.SuccessAsync(data: fileSharesInDb));

        }

        // PUT: api/FileShares/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFileShare(string id, Domain.Entities.FileShare fileShare)
        {
            if (id != fileShare.Id.ToString())
            {
                return BadRequest();
            }

            _context.Entry(fileShare).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }

            return NoContent();
        }

        // POST: api/FileShares        
        [HttpPost]
        public async Task<IActionResult> PostFileShare(Domain.Entities.FileShare fileShare)
        {
            if (FileShareExists(fileShare))
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File with same name already exists."));
            }

            fileShare.Id = Guid.NewGuid();
            fileShare.CreatedBy = Guid.Parse(_currentUserService.GetUserId());
            fileShare.CreatedOn = DateTime.Now;
            fileShare.UpdatedBy = Guid.Parse(_currentUserService.GetUserId());
            fileShare.UpdatedOn = DateTime.Now;

            _context.FileShare.Add(fileShare);
            try
            {
                await _context.SaveChangesAsync();

                return Ok(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File saved successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.FailAsync(message: ex.Message));
            }
        }

        // DELETE: api/FileShares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFileShare(string id)
        {

            var fileShare = await _context.FileShare.FindAsync(id);
            if (fileShare == null)
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File does not exists."));
            }
            //fileShare.IsDeleted = true;
            _context.Entry(fileShare).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File deleted successfully."));
        }

        private bool FileShareExists(Domain.Entities.FileShare fs)
        {
            return _context.FileShare.Any(e => e.FileFor == fs.FileFor && e.ParentId == fs.ParentId && e.FileName == fs.FileName);
        }

        // Ayush code below
        [HttpPost, DisableRequestSizeLimit]
        //[RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Route("upload/{code}/{id}/{img?}")]
        public async Task<IActionResult> Upload(string code, string id, string img = null)
        {
            try
            {
                var mFileShares = new List<Domain.Entities.FileShare>();
                var files = Request.Form.Files;
                var folderName = Path.Combine("FilesShare", code);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }

                foreach (var file in files)
                {
                    if (file.Length > 0 && id != null)
                    {
                        //using (var trans = new TransactionScope())
                        //{
                        var fileName = id + "_" + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fullPath = Path.Combine(pathToSave, fileName);
                        var dbPath = Path.Combine(folderName, fileName);

                        if (dbPath.EndsWith(".rar"))
                        {
                            dbPath = dbPath.Replace(".rar", ".zip");
                        }
                        if (fullPath.EndsWith(".rar"))
                        {
                            fullPath = fullPath.Replace(".rar", ".zip");
                        }


                        if (img == "INST")
                        {
                            var instrument = await _context.Instrument.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                            if (instrument != null)
                            {
                                instrument.Image = dbPath;
                                _context.Entry(instrument).State = EntityState.Modified;
                            }
                        }
                        else if (img == "SPPRT")
                        {
                            var instrument = await _context.Spareparts.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                            if (instrument != null)
                            {
                                instrument.Image = dbPath;
                                _context.Entry(instrument).State = EntityState.Modified;
                            }
                        }
                        else if (img == "VIDEO")
                        {
                            var sREngineerAction = await _context.SREngAction.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
                            if (sREngineerAction != null)
                            {
                                sREngineerAction.TeamviewRecording = dbPath;
                                _context.Entry(sREngineerAction).State = EntityState.Modified;
                            }
                        }
                        else
                        {
                            var nfile = new Domain.Entities.FileShare
                            {
                                Id = Guid.NewGuid(),
                                CreatedBy = Guid.Parse(_currentUserService.GetUserId()),
                                CreatedOn = DateTime.Now,
                                UpdatedBy = Guid.Parse(_currentUserService.GetUserId()),
                                UpdatedOn = DateTime.Now,
                                DisplayName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'),
                                ParentId = Guid.Parse(id),
                                FileFor = code,
                                FilePath = dbPath,
                                FileType = GetContentType(dbPath),
                                FileName = fileName,                                
                            };
                            _context.FileShare.Add(nfile);
                            mFileShares.Add(nfile);
                        }

                        _context.SaveChanges();

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        // trans.Complete();
                        //}
                    }

                }
                return Ok(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File uploaded successfully."));
            }
            catch (Exception e)
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.FailAsync(message: e.Message));
            }
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("download/{code?}")]
        public async Task<IActionResult> Download([FromQuery] string fileUrl, string code)
        {
            var filePath = "";
            if (code == "SRATN")
            {
                try
                {
                    var db = _context.SREngAction.FirstOrDefault(x => x.Id == Guid.Parse(fileUrl));
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), db.TeamviewRecording);
                }
                catch (Exception Ex)
                {
                }
            }
            else
            {
                var db = _context.FileShare.FirstOrDefault(x => x.Id == Guid.Parse(fileUrl));
                filePath = Path.Combine(Directory.GetCurrentDirectory(), db.FilePath);
            }

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), filePath);
        }

        [HttpGet, DisableRequestSizeLimit]
        [Route("getfile/{id}")]
        public async Task<IActionResult> ListFiles(Guid id)
        {
            try
            {
                var fileSharesInDb = await _context.FileShare.Where(x => x.ParentId == id).ToListAsync();

                return Ok(await ResponseWrapper<List<Domain.Entities.FileShare>>.SuccessAsync(data: fileSharesInDb));
            }
            catch (Exception e)
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.FailAsync(message: e.Message));
            }
        }

        [HttpGet("getImg/{code}/{id}")]
        public async Task<IActionResult> GetImgFile(string code, string id)
        {
            var base64ImageRepresentation = string.Empty;
            try
            {
                string filePath;
                if (code == "INST")
                {
                    filePath = _context.Instrument.FirstOrDefault(x => x.Id == Guid.Parse(id)).Image;
                    var imageArray = System.IO.File.ReadAllBytes(filePath);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);

                }
                else if (code == "SPPRT")
                {

                    filePath = _context.Spareparts.FirstOrDefault(x => x.Id == Guid.Parse(id)).Image;
                    var imageArray = System.IO.File.ReadAllBytes(filePath);
                    base64ImageRepresentation = Convert.ToBase64String(imageArray);

                    return Ok(await ResponseWrapper<string>.SuccessAsync(data: base64ImageRepresentation));
                }
            }
            catch (Exception Ex)
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.FailAsync(message: Ex.Message));
            }
            return Ok(await ResponseWrapper<string>.SuccessAsync(data: base64ImageRepresentation));
        }

        [HttpDelete("file/{id}")]
        public async Task<IActionResult> DeleteFile(string id)
        {
            var fileShare = await _context.FileShare.FindAsync(id);
            if (fileShare == null)
            {
                return BadRequest(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File does not exists."));
            }
            //fileShare.IsDeleted = true;
            _context.Entry(fileShare).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return Ok(await ResponseWrapper<Domain.Entities.FileShare>.SuccessAsync(message: "File deleted successfully."));
        }


        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }
    }
}