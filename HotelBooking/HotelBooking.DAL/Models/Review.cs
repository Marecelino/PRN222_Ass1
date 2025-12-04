using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.DAL.Models;

public class Review
{
    [Key]
    public int ReviewId { get; set; }

    [Required]
    public int BookingId { get; set; }

    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [MaxLength(500)]
    public string Comment { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // Navigation
    [ForeignKey("BookingId")]
    public Booking? Booking { get; set; }
}
