using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PredescuAlexandru_API.Exceptions;
using PredescuAlexandru_API.Helpers.Enum;
using PredescuAlexandru_API.Models;
using PredescuAlexandru_API.Repository.Interfaces;
using System.Net;

namespace PredescuAlexandru_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnoucementsController : ControllerBase
    {
        private IAnnouncementsRepository _announcementsRepository;

        private readonly ILogger<AnnoucementsController> _logger;

        public AnnoucementsController(IAnnouncementsRepository announcementsRepository, ILogger<AnnoucementsController> logger)
        {
            _announcementsRepository = announcementsRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogWarning("GetAnnouncements start");
            try
            {
                var announcements = await _announcementsRepository.GetAnnouncementsAsync();
                if (announcements == null || announcements.Count() < 1)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFound);
                    return StatusCode((int)HttpStatusCode.NoContent, ErrorMessagesEnum.Announcement.NoFound);
                }
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAnnouncements error: {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] Guid id)
        {
            try
            {
                var announcement = await _announcementsRepository.GetAnnouncementByIdAsync(id);
                if (announcement == null)
                {
                    _logger.LogInformation(ErrorMessagesEnum.Announcement.NoFoundById);
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return Ok(announcement);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Announcement announcement)
        {
            try
            {
                if (announcement == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                await _announcementsRepository.CreateAnnouncementAsync(announcement);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] Announcement announcement)
        {
            try
            {
                if (announcement == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                announcement.IdAnnouncement = id;
                var updateAnnouncement = await _announcementsRepository.UpdateAnnouncementAsync(id, announcement);
                if (updateAnnouncement == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Create announcement error occured {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch([FromRoute] Guid id, [FromBody] Announcement announcement)
        {
            try
            {
                if (announcement == null)
                {
                    return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.BadRequest);
                }
                var updatedAnnouncement = await _announcementsRepository.UpdatePartiallyAnnouncementAsync(id, announcement);
                if (updatedAnnouncement == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, ErrorMessagesEnum.Announcement.NoFoundById);
                }
                return StatusCode((int)HttpStatusCode.OK, SuccesMessagesEnum.Announcement.AnnouncementUpdated);
            }
            catch (ModelValidationException ex)
            {
                return StatusCode((int)HttpStatusCode.NotModified, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var result = await _announcementsRepository.DeleteAnnouncementAsync(id);
                if (result)
                {
                    _logger.LogInformation($"A fost sters anuntul cu id {id}");
                    return Ok(SuccesMessagesEnum.Announcement.AnnouncementDeleted);
                }
                return StatusCode((int)HttpStatusCode.BadRequest, ErrorMessagesEnum.Announcement.NoFoundById);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DeleteAnnouncemet error {ex.Message}");
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
