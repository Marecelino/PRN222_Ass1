using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class RoomType : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal BasePrice { get; set; }

        public int MaxOccupancy { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
