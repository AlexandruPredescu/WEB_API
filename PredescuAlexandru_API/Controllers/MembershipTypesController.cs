using Microsoft.AspNetCore.Mvc;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository;
using PredescuAlexandru_API.Repository.Interfaces;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PredescuAlexandru_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipTypesController : ControllerBase
    {
        private IMembershipTypesRepository _membershipTypesRepository;
        private readonly ILogger<AnnoucementsController> _logger;

        public MembershipTypesController(IMembershipTypesRepository membershipTypesRepository, ILogger<AnnoucementsController> logger)
        {
            _membershipTypesRepository = membershipTypesRepository;
            _logger = logger;
        }

        // GET: api/<MembershipTypesController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogWarning("GetMembershipType start");
            try
            {
                var membershipType = await _membershipTypesRepository.GetMembershipTypesAsync();
                if (membershipType == null || membershipType.Count() < 1)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFound);
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.Announcement.NoFound);
                }
                return Ok(membershipType);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetMembershipTypes error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/<MembershipTypesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var membershipType = await _membershipTypesRepository.GetMembershipTypeByIdAsync(id);
                if (membershipType == null)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFoundById);
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return Ok(membershipType);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<MembershipTypesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MembershipType membershipType)
        {
            try
            {
                if (membershipType == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                await _membershipTypesRepository.CreateMembershipTypeAsync(membershipType);
                return Ok(SuccesMessagesEnum.Announcement.AnnouncementAdded);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create membershipType error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<MembershipTypesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] MembershipType membershipType)
        {
            try
            {
                if (membershipType == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                membershipType.IdMembershipType = id;
                var updatemembershipType = await _membershipTypesRepository.UpdateMembershipTypeAsync(id, membershipType); 
                if (updatemembershipType == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create membershipType error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<MembershipTypesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _membershipTypesRepository.DeleteMembershipTypetAsync(id);//_announcementsRepository.DeleteAnnouncementAsync(id);
                if (result)
                {
                    _logger.LogInformation($"A fost sters membershipType cu id {id}");
                    return Ok(SuccesMessagesEnum.Announcement.AnnouncementDeleted);
                }
                return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.NoFoundById);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DeleteMembershiptype error {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] MembershipType membershipType)
        {
            try
            {
                if (membershipType == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                var updatemembershipType = await _membershipTypesRepository.UpdatePartiallyMembershipTypeAsync(id, membershipType);
                if (updatemembershipType == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
