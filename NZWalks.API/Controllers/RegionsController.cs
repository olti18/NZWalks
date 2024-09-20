
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;
using System.Diagnostics.CodeAnalysis;
namespace NZWalks.API.Controllers
{   
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {   
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var regionsDomain = await dbContext.Regions.ToListAsync();
            var regionsDomain = await regionRepository.GetAllAsync();

            //var regionsDto = new List<RegionDto>();
            //foreach (var regionDomain in regionsDomain) {
            //    regionsDto.Add(new RegionDto()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl,
            //    });
            //        }

            // Map domani models to DTO
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionsDto);
           // return Ok(var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain));
        }

        
        // GET single Region by ID (Get Region by Id)
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult>  GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id); Works only for Primary Key
            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            var regionDomain = await regionRepository.GetbyIdAsync(id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            /*var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };*/

            //return Ok(regionDto);
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }


        // POST to create a new Region
        // POST:  https://localhost:portnumber/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map or Convert DTO to Domain Model
            /*var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl,
            };*/

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Use Domain model to create Region
            //dbContext.Regions.Add(regionDomainModel);
            //dbContext.SaveChanges();
           regionDomainModel =  await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain Model back to Dto
            /*var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,

            };*/

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);


            return CreatedAtAction(nameof(GetById), new {id= regionDomainModel.Id},regionDto);
        }

        // UPDATE Region
        // PUT: https://localhost:portnumber/api/regions
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map dto to domain model
            /*var regionDomainmodel = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                RegionImageUrl = updateRegionRequestDto.RegionImageUrl,
            };*/

            var regionDomainmodel = mapper.Map<Region>(updateRegionRequestDto);

            // check if region exist
            //var regionDomainmodel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            regionDomainmodel = await regionRepository.UpdateAsync(id,regionDomainmodel);

            if(regionDomainmodel == null)
            {
                return NotFound();
            }

            // Map dto to domain models

            /* regionDomainmodel.Code= updateRegionRequestDto.Code;
             regionDomainmodel.Name = updateRegionRequestDto.Name;
             regionDomainmodel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;

             dbContext.SaveChanges();*/

            // Convert domain models to dto
            //var regionDto = mapper.Map<RegionDto>(regionDomainmodel);
            //return Ok(regionDto);
            return Ok(mapper.Map<RegionDto>(regionDomainmodel));
        }

        // Delete Region
        // Delete: Https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
           //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
           var regionDomainModel = await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }
            //Delete region
            //dbContext.Regions.Remove(regionDomainModel);
            //dbContext.SaveChanges();

            /*var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };*/

            //return Ok(regionDto);

            return Ok(mapper.Map<Region>(regionDomainModel));
        }
    }
}
