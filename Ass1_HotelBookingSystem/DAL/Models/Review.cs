using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    public class Review : BaseEntity
    {
        [ForeignKey("Booking")]
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
