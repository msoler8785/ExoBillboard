using ExoBillboard.Models;
using System.Linq;
using System.Web.Mvc;

namespace ExoBillboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Photo photo = new Photo();
            PhotoCollection photoCollection = new PhotoCollection();
            photo = photoCollection.Photos.FirstOrDefault();

            return View(photo);
        }

        public ActionResult Photos()
        {
            PhotoCollection photos = new PhotoCollection();
            return PartialView(photos);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult Json(int? id)
        {
            int index = (id ?? 1);
            PhotoCollection photos = new PhotoCollection();
            Photo photo = photos.Photos.ElementAtOrDefault(index);
            if (photo == null || index >= photos.Photos.Count() - 1) index = 0;
            else index++;
            photo.Index = index;
            return Json(photo, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            {
                if (disposing)
                {
                }
            }
            base.Dispose(disposing);
        }
    }
}