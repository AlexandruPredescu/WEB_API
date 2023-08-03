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
    public class MembersController : ControllerBase
    {
        private readonly IMembersRepository _membersRepository;
        private readonly ILogger<AnnoucementsController> _logger;

        public MembersController(IMembersRepository membersRepository, ILogger<AnnoucementsController> logger)
        {
            _membersRepository = membersRepository;
            _logger = logger;
        }



        // GET: MembersController
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogWarning("GetMembers start");
            try
            {
                var members = await _membersRepository.GetMembersAsync();
                if (members == null || members.Count() < 1)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFound);
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.Announcement.NoFound);
                }
                return Ok(members);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetMembers error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/<MembersController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var member = await _membersRepository.GetMemberByIdAsync(id);
                if (member == null)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFoundById);
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return Ok(member);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST api/<MembersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Member member)
        {
            try
            {
                if (member == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                await _membersRepository.CreateMemberAsync(member);
                return Ok(SuccesMessagesEnum.Announcement.AnnouncementAdded);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create member error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // PUT api/<MembersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] Member member)
        {
            try
            {
                if (member == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                member.IdMember = id;
                var updateMember = await _membersRepository.UpdateMemberAsync(id, member);
                if (updateMember == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create members error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // DELETE api/<MembersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _membersRepository.DeleteMemberAsync(id);
                if (result)
                {
                    _logger.LogInformation($"A fost sters pe baza id-ului {id}");
                    return Ok(SuccesMessagesEnum.Announcement.AnnouncementDeleted);
                }
                return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.NoFoundById);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DeleteMember error {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        //Patch
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute]  Guid id, [FromBody] Member member)
        {
            try
            {
                if (member == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                var updatedMember = await _membersRepository.UpdatePartiallyMemberAsync(id, member);
                if (updatedMember == null)
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
