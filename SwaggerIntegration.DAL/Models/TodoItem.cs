using System;
using System.ComponentModel.DataAnnotations;

namespace SwaggerIntegration.DAL.Models
{
    public class ToDoItem
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? FatherId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime? Created { get; set; }
        [Required]
        public bool Completed { get; set; }
    }
}
