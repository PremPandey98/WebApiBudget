using System.ComponentModel.DataAnnotations;

namespace WebApiBudget.DomainOrCore.Entities
{
    public class ExpenseCategoryEntity
    {
        [Key]
        public int ExpenseCategoryID { get; set; }
        [Required]
        public string ExpenseCategoryName { get; set; }
    }
}
