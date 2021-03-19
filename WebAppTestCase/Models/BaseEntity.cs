using System.ComponentModel.DataAnnotations;

namespace WebAppTestCase.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
