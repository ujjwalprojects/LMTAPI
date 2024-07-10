﻿using Asp.Versioning;
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
    public class WorkerRegistrationController : ControllerBase
    {
        private readonly IWorkerRegistrationRepository _workerRegistrationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<WorkerRegistrationController> _logger;

        public WorkerRegistrationController(IWorkerRegistrationRepository workerRegistrationRepository, IMapper mapper, ILogger<WorkerRegistrationController> logger)
        {
            _workerRegistrationRepository = workerRegistrationRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/WorkerRegistration
        [HttpGet]
        public async Task<ActionResult<List<T_WorkerRegistrationsDTO>>> GetWorkerRegistrations()
        {
            _logger.LogInformation("Method GetWorkerRegistrations invoked.");

            var workerRegistrations = await _workerRegistrationRepository.GetAllWorkerRegistrationsAsync();
            return Ok(_mapper.Map<List<T_WorkerRegistrationsDTO>>(workerRegistrations));
        }

        // GET: api/WorkerRegistration/id
        [HttpGet("{id}")]
        public async Task<ActionResult<T_WorkerRegistrationsDTO>> GetWorkerRegistration(int id)
        {
            _logger.LogInformation($"Method GetWorkerRegistration({id}) invoked.");

            var workerRegistration = await _workerRegistrationRepository.GetWorkerRegistrationByIdAsync(id);
            if (workerRegistration == null)
            {
                throw new BadHttpRequestException($"WorkerRegistration with Id {id} not found.", StatusCodes.Status400BadRequest);
            }
            return Ok(_mapper.Map<T_WorkerRegistrationsDTO>(workerRegistration));
        }

        // POST: api/WorkerRegistration
        [HttpPost]
        public async Task<ActionResult<T_WorkerRegistrationsDTO>> PostWorkerRegistration(T_WorkerRegistrationsDTO workerRegistrationDto)
        {
            _logger.LogInformation("Method PostWorkerRegistration invoked.");

            var workerRegistration = _mapper.Map<T_WorkerRegistrations>(workerRegistrationDto);
            await _workerRegistrationRepository.CreateWorkerRegistrationAsync(workerRegistration);

            return CreatedAtAction(nameof(GetWorkerRegistration), new { id = workerRegistration.Worker_Reg_Id }, _mapper.Map<T_WorkerRegistrationsDTO>(workerRegistration));
        }

        // PUT: api/WorkerRegistration/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkerRegistration(int id, T_WorkerRegistrationsDTO workerRegistrationDto)
        {
            _logger.LogInformation($"Method PutWorkerRegistration({id}) invoked.");

            if (id != workerRegistrationDto.Worker_Reg_Id)
            {
                throw new BadHttpRequestException($"WorkerRegistration with Id {id} not found.", StatusCodes.Status400BadRequest);
            }

            var workerRegistration = _mapper.Map<T_WorkerRegistrations>(workerRegistrationDto);
            try
            {
                await _workerRegistrationRepository.UpdateWorkerRegistrationAsync(workerRegistration);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await WorkerRegistrationExists(id))
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

        // DELETE: api/WorkerRegistration/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkerRegistration(int id)
        {
            _logger.LogInformation($"Method DeleteWorkerRegistration({id}) invoked.");

            var workerRegistration = await _workerRegistrationRepository.GetWorkerRegistrationByIdAsync(id);
            if (workerRegistration == null)
            {
                throw new BadHttpRequestException($"WorkerRegistration with Id {id} not found.", StatusCodes.Status400BadRequest);
            }

            await _workerRegistrationRepository.DeleteWorkerRegistrationAsync(id);

            return NoContent();
        }

        private async Task<bool> WorkerRegistrationExists(int id)
        {
            var workerRegistration = await _workerRegistrationRepository.GetWorkerRegistrationByIdAsync(id);
            return workerRegistration != null;
        }
    }
}
