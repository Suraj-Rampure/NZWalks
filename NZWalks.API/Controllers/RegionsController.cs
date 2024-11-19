﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    //https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

       
        // GET All Regions
        //GET : https://localhost:portnumber/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Models
            var regions = await regionRepository.GetAllAsync();
            
            //Map Domain Model to DTO
            var regionsdto = mapper.Map<List<RegionDto>>(regions);
            // Return DTOs
            return Ok(regionsdto);
        }

        //GET Single Region(Get Region by ID)
        //GET : https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
           
            var region = await regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            //Map Region domain model to DTO

           var regionsdto = mapper.Map<RegionDto>(region);

            //Return DTO Back to client

            return Ok(regionsdto);

        }

        //POST To Create a New Request
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            
                //Map or convert the DTO to Domain Model

                var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

                //Use domain model to create Region
                await regionRepository.CreateAsync(regionDomainModel);


                //Map Domain Model back to DTO
                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
          
        }

        //Update Region
        //PUT : https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           
                //Map DTO To Domain Model
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

                //Check if region exists

                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                await dbContext.SaveChangesAsync();

                //Convert Domain Model to DTO

                var regionDto = mapper.Map<RegionDto>(regionDomainModel);

                return Ok(regionDto);
            
        }

        //Delete Region
        //DELETE : https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id=Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);


            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Return Deleted region back
            //Map Domain model to Dto
            var regionDto = mapper.Map<RegionDto> (regionDomainModel);
            return Ok(regionDto);
        }
    }


}