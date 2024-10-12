using System.ComponentModel.DataAnnotations;

namespace FurniFusion.Dtos.Order
{
    public class ChangeOrdersStatusDto
    {

        [Required]
       public Dictionary<int, int> orderStatusData { get; set; }
    }
}
