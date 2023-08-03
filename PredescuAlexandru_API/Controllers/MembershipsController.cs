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
    public class MembershipsController : ControllerBase
    {
        private IMembershipsRepository _membershipsRepository;
        private readonly ILogger<MembershipsController> _logger;

        public MembershipsController(IMembershipsRepository membershipsRepository, ILogger<MembershipsController> logger)
        {
            _membershipsRepository = membershipsRepository;
            _logger = logger;
        }

        // GET: api/<MembershipsController>
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogWarning("GetMemberships start");
            try
            {
                var memberships = await _membershipsRepository.GetMembershipsAsync();
                if (memberships == null || memberships.Count() < 1)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFound);
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.Announcement.NoFound);
                }
                return Ok(memberships);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAnnouncements error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/<MembershipsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var memberships = await _membershipsRepository.GetMembershipByIdAsync(id);
                if (memberships == null)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFoundById);
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return Ok(memberships);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<MembershipsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Membership membership)
        {
            try
            {
                if (membership == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                await _membershipsRepository.CreateMembershipAsync(membership);
                return Ok(SuccesMessagesEnum.Announcement.AnnouncementAdded);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create announcement error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<MembershipsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] Membership membership)
        {
            try
            {
                if (membership == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                membership.IdMembership = id;
                var updateMembership = await _membershipsRepository.UpdateMembershipAsync(id, membership);
                if (updateMembership == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create membership error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<MembershipsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _membershipsRepository.DeleteMemberAsync(id);
                if (result)
                {
                    _logger.LogInformation($"A fost sters membership cu id {id}");
                    return Ok(SuccesMessagesEnum.Announcement.AnnouncementDeleted);
                }
                return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.NoFoundById);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DeleteMembership error {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] Membership membership)
        {
            try
            {
                if (membership == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                var updateMembership = await _membershipsRepository.UpdatePartiallyMembershipAsync(id, membership);
                if (updateMembership == null)
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
