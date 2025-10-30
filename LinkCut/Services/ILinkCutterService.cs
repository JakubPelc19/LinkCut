using LinkCut.Models;

namespace LinkCut.Services
{
    public interface ILinkCutterService
    {
        Task<ServiceResponse<ShortLink>> CreateShortLink(LinkFromClient originalLink);
        Task<ServiceResponse<ShortLink>> GetOriginalLinkFromShortLink(string originalLinkId);
    }
}
