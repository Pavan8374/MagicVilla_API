using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse response;
        private readonly IMapper mapper;
        private readonly IVillaRepository dbVilla;
        public VillaAPIController( ApplicationDbContext _db, IMapper _mapper, IVillaRepository _dbVilla)
        {
            mapper = _mapper;
            dbVilla = _dbVilla;
            this.response = new();
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillas() 
        {
            try
            {
                IEnumerable<Villa> villaList = await dbVilla.GetAll();
                response.Result = mapper.Map<List<VillaDTO>>(villaList);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }
        [HttpGet("{id:int}", Name ="GetVillaById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(200, Type = typeof(VillaDTO))]
        public async Task<ActionResult<APIResponse>> GetVillaById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await dbVilla.Get(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                response.Result = mapper.Map<VillaDTO>(villa);
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDTO createDTO)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            try { 

            if (await dbVilla.Get(u => u.Name.ToLower() == createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return NotFound();
            }
            Villa villa = mapper.Map<Villa>(createDTO);
            await dbVilla.Create(villa);
            response.Result = mapper.Map<VillaDTO>(villa);
            response.StatusCode = System.Net.HttpStatusCode.Created;
            return CreatedAtRoute("GetVillaById",new {id = villa.Id }, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<ActionResult<APIResponse>> DeleteResult(int id)
        {
            try { 
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = await dbVilla.Get(u => u.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            await dbVilla.Remove(villa);
            response.StatusCode = System.Net.HttpStatusCode.NoContent;
            response.IsSuccess = true;
            return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }
        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, VillaUpdateDTO updateDTO)
        {
            try { 
            if(updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest();
            }
            Villa model = mapper.Map<Villa>(updateDTO);
            
            await dbVilla.Update(model);
            response.StatusCode = System.Net.HttpStatusCode.NoContent;
            response.IsSuccess =true;
            return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }
        [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa =  await dbVilla.Get(u => u.Id == id, tracked :false);

            VillaUpdateDTO villaDTO = mapper.Map<VillaUpdateDTO>(villa);
            
            if (villa == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = mapper.Map<Villa>(villaDTO);
            await dbVilla.Update(model);
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}   
