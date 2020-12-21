using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URIS_OrderService.Models
{
    public class OrderCreation
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
        public string OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the number of products in order.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the order amount.
        /// </summary>
        [Required]
        public double Amount { get; set; }
    }
}
