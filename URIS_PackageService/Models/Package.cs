using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URIS_PackageService.Models
{
    public class Package
    {
        public Package()
        {

        }
        public Package(JObject packageFromJson)
        {
            Id = Guid.NewGuid();
            Code = (string)packageFromJson["code"];
            ShipmentDate = (string)packageFromJson["shipmentDate"];
            TotalAmount = (double)packageFromJson["totalAmount"];
            HaveDiscount = (bool)packageFromJson["haveDiscount"];
            OrderCode = (string)packageFromJson["orderCode"];
        }
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

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
        public bool HaveDiscount { get; set; }

        /// <summary>
        /// Gets or sets the total amount.
        /// </summary>
        [Required]
        public double TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets the order code.
        /// </summary>
        public string OrderCode { get; set; }
    }
}
