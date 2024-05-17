
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Numerics;
using WebApplication3.DAL;
using WebApplication3.Models;

namespace WebApplication3.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class DoctorController : Controller
    {
        AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public DoctorController(AppDbContext context,IWebHostEnvironment environment)
        {
            _context=context;
            _environment= environment;
        }
        public IActionResult Index()
        {
            List<Doctor>Doctors= _context.Doctors.ToList();
            return View(Doctors);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Doctor doctor) 
        {
            if (doctor == null)
            {
                return View();
            }
            if(!doctor.ImgFile.ContentType.Contains("image/"))
            {
                return View();
            }
            string path = _environment.WebRootPath + @"\Upload\Doctor";
            string filname = doctor.ImgFile.FileName;   
            using(FileStream stream=new FileStream(path+filname, FileMode.Create))
            {
                doctor.ImgFile.CopyTo(stream);
            }
            doctor.ImgUrl = filname;
            _context.Doctors.Add(doctor);
            _context .SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Update(int id)
        {
            var doctor=_context.Doctors.FirstOrDefault(x => x.Id == id);    
            return View(doctor);

        }
        [HttpPost]
        public IActionResult Update(Doctor newdoctor)
        {
            var olddoctor = _context.Doctors.FirstOrDefault(x => x.Id == newdoctor.Id);
            if(olddoctor == null)
            {
                return View();
            }
            if(newdoctor.ImgFile!=null)
            {

            
            string path= _environment.WebRootPath + @"\Upload\Doctor"+olddoctor.ImgUrl;
            FileInfo fileInfo = new FileInfo(path);
            if(fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            string filname = newdoctor.ImgFile.FileName;
            using (FileStream stream = new FileStream(path + filname, FileMode.Create))
            {
                newdoctor.ImgFile.CopyTo(stream);
            }
            newdoctor.ImgUrl = filname;
            }
            olddoctor.Position = newdoctor.Position;
            olddoctor.Name = newdoctor.Name;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int id)
        {
            var doctor= _context.Doctors.FirstOrDefault(x=>x.Id == id);
            if(doctor == null)
            {
                return View();
            }
            string path = _environment.WebRootPath + @"\Upload\Doctor" + doctor.ImgUrl;
            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            _context.Doctors.Remove(doctor);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
            
    }
}
