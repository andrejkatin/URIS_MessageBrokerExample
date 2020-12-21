using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URIS_PackageService.Models;

namespace URIS_PackageService.Data
{
    public interface IPackageRepository
    {
        Package GetPackageById(string packageCode);
        bool CreatePackage(Package package);
    }
}
