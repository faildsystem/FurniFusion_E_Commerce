using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Review
{
    public class CreateReviewDto
    {
        public int Rating { get; set; }
        [Required]
        public string? ReviewText { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}