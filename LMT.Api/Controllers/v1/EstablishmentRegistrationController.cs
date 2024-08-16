using Asp.Versioning;
using AutoMapper;
using LMT.Application.DTOs;
using LMT.Application.Interfaces;
using LMT.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMT.Api.Controllers.v1
{
    [ApiController]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class EstablishmentRegistrationController : ControllerBase
    {
        private readonly IEstablishmentRegistrationRepository _establishmentRegistrationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<EstablishmentRegistrationController> _logger;

        public EstablishmentRegistrationController(IEstablishmentRegistrationRepository establishmentRegistrationRepository, IMapper mapper, ILogger<EstablishmentRegistrationController> logger)
        {
            _establishmentRegistrationRepository = establishmentRegistrationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/EstablishmentRegistration
        [HttpGet]
        public async Task<ActionResult<List<T_EstablishmentRegistrationsDTO>>> GetEstablishmentRegistrations()
        {
            _logger.LogInformation("Method GetEstablishmentRegistrations invoked.");

            var establishmentRegistrations = await _establishmentRegistrationRepository.GetAllEstablishmentRegistrationsAsync();
            return Ok(_mapper.Map<List<T_EstablishmentRegistrationsDTO>>(establishmentRegistrations));
        }
        [HttpGet("establishment-list")]
        public async Task<ActionResult<List<T_EstablishmentRegistrationsDTO>>> GetEstablishmentRegistrations(string? searchText)
        {
            _logger.LogInformation("Method GetEstablishmentRegistrations by searchText invoked.");

            var establishmentRegistrations = await _establishmentRegistrationRepository.GetAllEstablishmentRegistrationsAsync(searchText);
            return Ok(_mapper.Map<List<T_EstablishmentRegistrationsDTO>>(establishmentRegistrations));
        }


        // GET: api/EstablishmentRegistration/id
        [HttpGet("{id}")]
        public async Task<ActionResult<T_EstablishmentRegistrationsDTO>> GetEstablishmentRegistration(int id)
        {
            _logger.LogInformation($"Method GetEstablishmentRegistration({id}) invoked.");

            var establishmentRegistration = await _establishmentRegistrationRepository.GetEstablishmentRegistrationByIdAsync(id);
            if (establishmentRegistration == null)
            {
                throw new BadHttpRequestException($"EstablishmentRegistration with Id {id} not found.", StatusCodes.Status400BadRequest);
            }
            return Ok(_mapper.Map<T_EstablishmentRegistrationsDTO>(establishmentRegistration));
        }

        // POST: api/EstablishmentRegistration
        [HttpPost]
        public async Task<ActionResult<T_EstablishmentRegistrationsDTO>> PostEstablishmentRegistration(T_EstablishmentRegistrationsDTO establishmentRegistrationDto)
        {
            _logger.LogInformation("Method PostEstablishmentRegistration invoked.");

            var establishmentRegistration = _mapper.Map<T_EstablishmentRegistrations>(establishmentRegistrationDto);
            await _establishmentRegistrationRepository.CreateEstablishmentRegistrationAsync(establishmentRegistration);

            return CreatedAtAction(nameof(GetEstablishmentRegistration), new { id = establishmentRegistration.Estd_Id }, _mapper.Map<T_EstablishmentRegistrationsDTO>(establishmentRegistration));
        }

        // PUT: api/EstablishmentRegistration/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstablishmentRegistration(int id, T_EstablishmentRegistrationsDTO establishmentRegistrationDto)
        {
            _logger.LogInformation($"Method PutEstablishmentRegistration({id}) invoked.");

            if (id != establishmentRegistrationDto.Estd_Id)
            {
                throw new BadHttpRequestException($"EstablishmentRegistration with Id {id} not found.", StatusCodes.Status400BadRequest);
            }

            var establishmentRegistration = _mapper.Map<T_EstablishmentRegistrations>(establishmentRegistrationDto);
            try
            {
                await _establishmentRegistrationRepository.UpdateEstablishmentRegistrationAsync(establishmentRegistration);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EstablishmentRegistrationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/EstablishmentRegistration/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstablishmentRegistration(int id)
        {
            _logger.LogInformation($"Method DeleteEstablishmentRegistration({id}) invoked.");

            var establishmentRegistration = await _establishmentRegistrationRepository.GetEstablishmentRegistrationByIdAsync(id);
            if (establishmentRegistration == null)
            {
                throw new BadHttpRequestException($"EstablishmentRegistration with Id {id} not found.", StatusCodes.Status400BadRequest);
            }

            await _establishmentRegistrationRepository.DeleteEstablishmentRegistrationAsync(id);

            return NoContent();
        }

        private async Task<bool> EstablishmentRegistrationExists(int id)
        {
            var establishmentRegistration = await _establishmentRegistrationRepository.GetEstablishmentRegistrationByIdAsync(id);
            return establishmentRegistration != null;
        }
    }
}
