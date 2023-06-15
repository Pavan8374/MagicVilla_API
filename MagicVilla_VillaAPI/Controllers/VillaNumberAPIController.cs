using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse response;
        private readonly IMapper mapper;
        private readonly IVillaNumberRepository dbVillaNumber;
        private readonly IVillaRepository dbVilla;
        public VillaNumberAPIController(ApplicationDbContext _db, IMapper _mapper, IVillaNumberRepository _dbVillaNumber, IVillaRepository _dbVilla)
        {
            mapper = _mapper;
            dbVillaNumber = _dbVillaNumber;
            this.response = new();
            dbVilla = _dbVilla;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await dbVillaNumber.GetAll();
                response.Result = mapper.Map<List<VillaNumberDTO>>(villaNumberList);
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
        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await dbVillaNumber.Get(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    return NotFound();
                }
                response.Result = mapper.Map<VillaNumberDTO>(villaNumber);
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
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO)
        {
            try
            {

                if (await dbVillaNumber.Get(u => u.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("CustomError", "VillaNumber already exists");
                    return BadRequest(ModelState);
                }
                if (await dbVilla.Get(u => u.Id == createDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomEroor", "Villa Id is in invalid!");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return NotFound();
                }
                VillaNumber villaNumber = mapper.Map<VillaNumber>(createDTO);
                await dbVillaNumber.Create(villaNumber);
                response.Result = mapper.Map<VillaNumberDTO>(villaNumber);
                response.StatusCode = System.Net.HttpStatusCode.Created;
                return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return response;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villaNumber = await dbVillaNumber.Get(u => u.VillaNo == id);
                if (villaNumber == null)
                {
                    return NotFound();
                }
                await dbVillaNumber.Remove(villaNumber);
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
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, VillaNumberUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.VillaNo)
                {
                    return BadRequest();
                }
                if (await dbVilla.Get(u => u.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("CustomEroor", "Villa Id is in invalid!");
                    return BadRequest(ModelState);
                }
                VillaNumber model = mapper.Map<VillaNumber>(updateDTO);

                await dbVillaNumber.Update(model);
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
        [HttpPatch("{id:int}", Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePartialVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villaNumber = await dbVillaNumber.Get(u => u.VillaNo == id, tracked: false);

            VillaNumberUpdateDTO villaDTO = mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            if (villaNumber == null)
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            VillaNumber model = mapper.Map<VillaNumber>(villaDTO);
            await dbVillaNumber.Update(model);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
