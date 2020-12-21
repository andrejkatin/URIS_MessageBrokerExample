using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URIS_PackageService.Models
{
    public class PackageCreation
    {
        /// <summary>
        /// Gets or sets the order unique code.
        /// </summary>
        [Required]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        [Required]
        public string ShipmentDate { get; set; }

        /// <summary>
        /// Gets or sets information if package have discount.
        /// </summary>
        [Required]
        public bool HavDiscount { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        [Required]
        public double TotalAmount { get; set; }
    }
}
