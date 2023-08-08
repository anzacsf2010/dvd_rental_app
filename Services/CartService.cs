using dvd_rental_app.Data;
using dvd_rental_app.Models;

namespace dvd_rental_app.Services
{
    public interface ICartService
    {
        void Update(CartItem item);
        List<CartItem> GetCartItems();
    }



    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _db;


        public CartService(ApplicationDbContext db)
        {
            _db = db;
        }


        public void Update(CartItem item)
        {
            var existingItem = _db.CartItems.Where(i => i.MovieId == item.MovieId).Where(i => i.UserId == item.UserId).FirstOrDefault();

            if (existingItem == null)
            {
                _db.CartItems.Add(item);
            }
            else
            {
                _db.CartItems.Remove(existingItem);
            }

            _db.SaveChanges();
        }


        public List<CartItem> GetCartItems()
        {
            return _db.CartItems.ToList();
        }
    }

}

