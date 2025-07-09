using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nexus.Core
{
    public class Supplier
    {

        public int Id { get; set; }

        [Required] // set to not null
        [StringLength(100)] //set to only 100 characters max
        public string Name { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; } //? can be null

        [Required]
        [EmailAddress] //set email validation
        [StringLength(100)]
        public string Email { get; set; }

        // Navigation property for related products
        public ICollection<Product> Products { get; set; }
    }
}
