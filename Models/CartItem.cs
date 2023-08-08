using System.ComponentModel.DataAnnotations;

namespace dvd_rental_app.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }


        public int MovieId { get; set; }


        public string? Title { get; set; }


        public DateOnly AddedToCartDate { get; set; }


        public string? UserId { get; set; }
    }

}