using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFModels.Models
{
    public class Person
    {
        public Person()
        {
        }

        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Required]
        public string MiddleName { get; set; }

        [StringLength(100)]
        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(100)]
        public string PrimaryPhone { get; set; }

        [StringLength(100)]
        public string SecondaryPhone { get; set; }

        public bool? IsActive { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }
    }
}
