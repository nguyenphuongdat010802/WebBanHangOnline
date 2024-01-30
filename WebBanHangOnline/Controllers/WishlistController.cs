using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using WebBanHangOnline.Models;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Controllers
{
    [Authorize]
    public class WishlistController : Controller
    {
        // GET: Wishlist
        public ActionResult Index(int? page)
        {
            var pageSize = 4;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Wishlist> items = db.Wishlists.Where(x => x.UserName == User.Identity.Name).OrderByDescending(x=>x.CreatedDate);
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostWishlist(int ProductId)
        {
            if (Request.IsAuthenticated == false)
            {
                return Json(new { Success = false, Message = "Bạn chưa đăng nhập." });
            }
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkItem != null) 
            {
                return Json(new { Success = false, Message = "Sản phẩm đã được yêu thích rồi." });
            }
            var item = new Wishlist();
            item.ProductId = ProductId;
            item.UserName = User.Identity.Name;
            item.CreatedDate = DateTime.Now;
            db.Wishlists.Add(item);
            db.SaveChanges();
            return Json(new { Success = true});
        }

        // Xóa khỏi danh sách yêu thích bằng cách nhấn 2 lần vào icon trái tim
        [HttpPost]
        [AllowAnonymous]
        public ActionResult PostDeleteWishlist(int ProductId)
        {
            var checkItem = db.Wishlists.FirstOrDefault(x => x.ProductId == ProductId && x.UserName == User.Identity.Name);
            if (checkItem != null) 
            {
                db.Wishlists.Remove(checkItem);
                db.SaveChanges();
                return Json(new { Success = true, Message = "Xóa thàng công khỏi mục yêu thích" });
            }
            return Json(new { Success = false, Message = "Xóa sp thất bại khỏi mục yêu thích." });
        }

        // Xóa bằng button xóa trong danh sách sp yêu thích
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RemoveFromWishlist(int wishlistId)
        {
            var wishlistItem = db.Wishlists.FirstOrDefault(x => x.Id == wishlistId && x.UserName == User.Identity.Name);
            if (wishlistItem != null)
            {
                db.Wishlists.Remove(wishlistItem);
                db.SaveChanges();
                return Json(new { Success = true, Message = "Xóa thành công khỏi mục yêu thích." });
            }
            return Json(new { Success = false, Message = "Xóa sản phẩm khỏi mục yêu thích thất bại." });
        }


        private ApplicationDbContext db = new ApplicationDbContext();
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}