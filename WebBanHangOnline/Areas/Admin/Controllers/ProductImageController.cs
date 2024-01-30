using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int id)
        {
            ViewBag.ProductId = id;
            var items = db.ProductImages.Where(x => x.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId,string url)
        {
            db.ProductImages.Add(new ProductImage { 
                ProductId=productId,
                Image=url,
                IsDefault=false
            });
            db.SaveChanges();
            return Json(new { Success=true});
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.ProductImages.Find(id);
            db.ProductImages.Remove(item);
            db.SaveChanges();
            return Json(new { success = true });
        }






        // Phần Edit hình ảnh trong controller
        [HttpPost]
        public ActionResult SetDefaultImage(int id)
        {
            // Tìm ảnh cần cập nhật
            var image = db.ProductImages.Find(id);

            // Thực hiện logic để cập nhật trạng thái IsDefault
            image.IsDefault = !image.IsDefault;

            // Cập nhật trong cơ sở dữ liệu
            db.SaveChanges();

            return Json(new { success = true });
        }


        // Xóa hết ảnh
        //[HttpPost]
        //public ActionResult DeleteAll(int productId)
        //{
        //    try
        //    {
        //        // Lấy danh sách tất cả ảnh từ cơ sở dữ liệu cho sản phẩm cụ thể
        //        var productImages = db.ProductImages.Where(x => x.ProductId == productId).ToList();

        //        // Xóa từng ảnh trong danh sách
        //        foreach (var image in productImages)
        //        {
        //            db.ProductImages.Remove(image);
        //        }

        //        // Lưu thay đổi vào cơ sở dữ liệu
        //        db.SaveChanges();

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi nếu có
        //        return Json(new { success = false, error = ex.Message });
        //    }
        //}







    }
}