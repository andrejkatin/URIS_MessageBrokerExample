using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using URIS_PackageService.Data;
using URIS_PackageService.Models;

namespace URIS_PackageService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageRepository packageRepository;
        private readonly IMapper mapper;

        public PackageController(IPackageRepository packageRepository, IMapper mapper)
        {
            this.packageRepository = packageRepository;
            this.mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<Package>> Get()
        {
            return PackageRepository.Packages;
        }

        /// <summary>
        /// Gets a single order by orderId
        /// </summary>
        /// <param name="packageCode"></param>
        /// <returns></returns>
        [HttpGet("{packageCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<Package> GetPackage(string packageCode)
        {
            var package = packageRepository.GetPackageById(packageCode);

            if (package == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<Package>(package));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Package> PostPackage([FromBody] PackageCreation package)
        {
            try
            {
                Package packageEntity = mapper.Map<Package>(package);

                var addedPackage = packageRepository.CreatePackage(packageEntity);

                if (package == null)
                {
                    return BadRequest();
                }

                return Ok(addedPackage);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Create error");
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
