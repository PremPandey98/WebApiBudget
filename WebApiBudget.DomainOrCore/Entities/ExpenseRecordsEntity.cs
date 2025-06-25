using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBudget.DomainOrCore.Entities
{
    public class ExpenseRecordsEntity
    {
        [Key]
        public int ExpenseId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public Guid? AddedByUserId { get; set; }

        [ForeignKey("AddedByUserId")]
        public UsersEntity? AddedByUser { get; set; }

        [Required]
        public bool IsGroupRelated { get; set; } = false;

        public Guid? GroupId { get; set; }

        [ForeignKey("GroupId")]
        public GroupEntity? Group { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

}
