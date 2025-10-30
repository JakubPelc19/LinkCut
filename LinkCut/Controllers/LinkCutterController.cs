using LinkCut.Data;
using LinkCut.Models;
using LinkCut.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkCut.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkCutterController(ILinkCutterService _linkCutterService) : ControllerBase
    {
        [HttpPost("createshortlink")]
        public async Task<ActionResult<ServiceResponse<ShortLink>>> CreateShortLink(LinkFromClient request)
        {
            var createShortLinkResponse = await _linkCutterService.CreateShortLink(request);

            return StatusCode(createShortLinkResponse.StatusCode, createShortLinkResponse);
        }

        [HttpGet("getoriginalink/{originalLinkId}")]
        public async Task<ActionResult<ServiceResponse<ShortLink>>> GetOriginalLinkFromShortLink(string originalLinkId)
        {
            var getOriginalLinkFromShortLinkResponse = await _linkCutterService.GetOriginalLinkFromShortLink(originalLinkId);

            return StatusCode(getOriginalLinkFromShortLinkResponse.StatusCode, getOriginalLinkFromShortLinkResponse);
            
        }
    }
}
