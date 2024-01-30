using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanHangOnline.Models.EF;

namespace WebBanHangOnline.Models
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }

        //
        private readonly ApplicationDbContext _context;
        //

        //Tạm thời đống lại
        //public ShoppingCart()
        //{
        //    this.Items = new List<ShoppingCartItem>();
        //}

        public ShoppingCart(ApplicationDbContext context)
        {
            this.Items = new List<ShoppingCartItem>();
            _context = context; // Khởi tạo context
        }

        // Thử nghiệm chức năng tạm thời đống lại
        //public void AddToCart(ShoppingCartItem item, int Quantity)
        //{
        //    var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
        //    if (checkExits != null)
        //    {
        //        checkExits.Quantity += Quantity;
        //        checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
        //    }
        //    else
        //    {

        //        //

        //        Items.Add(item);
        //    }
        //}
        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
            }
            else
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    item.Inventory = product.Quantity;
                }

                Items.Add(item);
            }
        }
        //public void AddToCart(ShoppingCartItem item, int Quantity)
        //{
        //    var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
        //    if (checkExits != null)
        //    {
        //        checkExits.Quantity += Quantity;
        //        checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;
        //    }
        //    else
        //    {
        //        // Thay đổi để lấy thông tin số lượng tồn kho
        //        var productQuantity = GetProductQuantity(item.ProductId);

        //        // Thêm thông tin số lượng tồn kho vào item
        //        item.Inventory = productQuantity;

        //        Items.Add(item);
        //    }
        //}






        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x=>x.ProductId==id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
            }
        }

        public void UpdateQuantity(int id,int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.TotalPrice = checkExits.Price * checkExits.Quantity;

               
            }
        }

        public decimal GetTotalPrice()
        {
            return Items.Sum(x=>x.TotalPrice);
        }
        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }


        //

        public int GetProductQuantity(int productId)
        {
            if (Items == null)
            {
                // Hoặc xử lý khác tùy thuộc vào yêu cầu của bạn
                return 0;
            }

            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            return product != null ? product.Quantity : 0;
        }




    }

    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }

        // Di chuyển thuộc tính Inventory trước thuộc tính Quantity
        public int Inventory { get; set; }
        //

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }



    }
}