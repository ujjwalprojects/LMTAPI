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
    //[Authorize]
    [ApiVersion("1")]
    [Route("api/v{v:apiVersion}/[controller]")]
    public class TaskAllocationFormController : ControllerBase
    {
        private readonly ITaskAllocationFormRepository _taskAllocationFormRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TaskAllocationFormController> _logger;

        public TaskAllocationFormController(ITaskAllocationFormRepository taskAllocationFormRepository, IMapper mapper, ILogger<TaskAllocationFormController> logger)
        {
            _taskAllocationFormRepository = taskAllocationFormRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/TaskAllocationForm
        [HttpGet]
        public async Task<ActionResult<List<T_TaskAllocationFormsDTO>>> GetTaskAllocationForms()
        {
            _logger.LogInformation("Method GetTaskAllocationForms invoked.");

            var taskAllocationForms = await _taskAllocationFormRepository.GetAllTaskAllocationFormsAsync();
            return Ok(_mapper.Map<List<T_TaskAllocationFormsDTO>>(taskAllocationForms));
        }

        // GET: api/TaskAllocationForm/id
        [HttpGet("{id}")]
        public async Task<ActionResult<T_TaskAllocationFormsDTO>> GetTaskAllocationForm(string id)
        {

            id = Uri.UnescapeDataString(id);
            _logger.LogInformation($"Method GetTaskAllocationForm({id}) invoked.");

            var taskAllocationForm = await _taskAllocationFormRepository.GetTaskAllocationFormByIdAsync(id);
            if (taskAllocationForm == null)
            {
                throw new BadHttpRequestException($"TaskAllocationForm with Id {id} not found.", StatusCodes.Status400BadRequest);
            }
            return Ok(_mapper.Map<T_TaskAllocationFormsDTO>(taskAllocationForm));
        }

        // POST: api/TaskAllocationForm
        [HttpPost]
        public async Task<ActionResult<T_TaskAllocationFormsDTO>> PostTaskAllocationForm(T_TaskAllocationFormsDTO taskAllocationFormDto)
        {
            _logger.LogInformation("Method PostTaskAllocationForm invoked.");

            var taskAllocationForm = _mapper.Map<T_TaskAllocationForms>(taskAllocationFormDto);
            await _taskAllocationFormRepository.CreateTaskAllocationFormAsync(taskAllocationForm);

            return CreatedAtAction(nameof(GetTaskAllocationForm), new { id = taskAllocationForm.Task_Id }, _mapper.Map<T_TaskAllocationFormsDTO>(taskAllocationForm));
        }

        // PUT: api/TaskAllocationForm/id
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTaskAllocationForm(string id, T_TaskAllocationFormsDTO taskAllocationFormDto)
        {
            _logger.LogInformation($"Method PutTaskAllocationForm({id}) invoked.");

            if (id != taskAllocationFormDto.Task_Id)
            {
                throw new BadHttpRequestException($"TaskAllocationForm with Id {id} not found.", StatusCodes.Status400BadRequest);
            }

            var taskAllocationForm = _mapper.Map<T_TaskAllocationForms>(taskAllocationFormDto);
            try
            {
                await _taskAllocationFormRepository.UpdateTaskAllocationFormAsync(taskAllocationForm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskAllocationFormExists(id))
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

        // DELETE: api/TaskAllocationForm/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskAllocationForm(string id)
        {
            id = Uri.UnescapeDataString(id);
            _logger.LogInformation($"Method DeleteTaskAllocationForm({id}) invoked.");

            var taskAllocationForm = await _taskAllocationFormRepository.GetTaskAllocationFormByIdAsync(id);
            if (taskAllocationForm == null)
            {
                throw new BadHttpRequestException($"TaskAllocationForm with Id {id} not found.", StatusCodes.Status400BadRequest);
            }

            await _taskAllocationFormRepository.DeleteTaskAllocationFormAsync(id);

            return NoContent();
        }

        private async Task<bool> TaskAllocationFormExists(string id)
        {
            var taskAllocationForm = await _taskAllocationFormRepository.GetTaskAllocationFormByIdAsync(id);
            return taskAllocationForm != null;
        }
    }
}
