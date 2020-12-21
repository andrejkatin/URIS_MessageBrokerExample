using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URIS_PackageService.Models;

namespace URIS_PackageService.Data
{
    public class PackageRepository : IPackageRepository
    {
        public static List<Package> Packages { get; set; } = new List<Package>();

        public PackageRepository()
        {
            FillData();
        }

        private void FillData()
        {
            Packages.Add(new Package
            {
                Id = Guid.Parse("79bb65eb-b204-4adf-9051-89d21e46698e"),
                Code = "P-001",
                ShipmentDate = "16.12.2020.",
                HaveDiscount = false,
                TotalAmount = 1400,
                OrderCode = "O-001"
            });
        }

        public bool CreatePackage(Package package)
        {
            package.Id = Guid.NewGuid();
            Packages.Add(package);

            var testPackage = GetPackageById(package.Code);
            if (testPackage == null)
            {
                return false;
            }

            return true;
        }

        public Package GetPackageById(string packageCode)
        {
            return Packages.FirstOrDefault(p => p.Code == packageCode);
        }
    }
}
