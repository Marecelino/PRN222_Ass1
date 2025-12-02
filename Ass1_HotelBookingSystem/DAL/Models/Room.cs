using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Room : BaseEntity
    {
        [Required]
        [StringLength(20)]
        public string RoomNumber { get; set; }

        [ForeignKey("RoomType")]
        public Guid RoomTypeId { get; set; }
        public RoomType RoomType { get; set; }

        [Required]
        public string Status { get; set; } // Available, Booked, Maintenance

        public ICollection<Booking> Bookings { get; set; }
    }
}
