using LinkCut.Data;
using LinkCut.Models;
using Microsoft.EntityFrameworkCore;

namespace LinkCut.Services
{
    public class LinkCutterService(AppDbContext _context) : ILinkCutterService
    {
        public async Task<ServiceResponse<ShortLink>> CreateShortLink(LinkFromClient originalLink)
        {
            string trimmedOriginalLink = originalLink.Link.Trim();

            
            ServiceResponse<ShortLink> response = new ServiceResponse<ShortLink>();
            
            // Checks errors, if it finds any then it will return true
            if (CheckErrsLink(trimmedOriginalLink, response))
                return response;

            // Checks if ShortLink for OriginalLink already exists
            // if it does then it won't generate new ShortLink, but it will return the ShortLink that already exists in DB
            var existentShortLink = await _context.ShortLinks.FirstOrDefaultAsync(s => s.OriginalLink == new Uri(trimmedOriginalLink));

            if (existentShortLink is not null)
            {
                response.Message = "Short link found";

                response.StatusCode = StatusCodes.Status200OK;

                response.IsSuccessful = true;

                response.Data = existentShortLink;

                return response;
            }

            // Generates new ShortLink if it doesnt exist for OriginalLink in DB

            ShortLink link = new ShortLink();

            link.OriginalLink = new Uri(trimmedOriginalLink);
            link.OriginalLinkId = await GenerateOriginalLinkId();

            await _context.ShortLinks.AddAsync(link);

            await _context.SaveChangesAsync();

            response.Message = "Short link was created successfully";

            response.StatusCode = StatusCodes.Status201Created;

            response.IsSuccessful = true;

            response.Data = link;

            return response;
            
            
        }

        public async Task<ServiceResponse<ShortLink>> GetOriginalLinkFromShortLink(string originalLinkId)
        {
            string trimmedOriginalLinkId = originalLinkId.Trim();

            ServiceResponse<ShortLink> response = new ServiceResponse<ShortLink>();

            // Checks errors, if it finds any then it will return true
            if (CheckErrsOrignalLinkId(trimmedOriginalLinkId, response))
                return response;

            var shortlink = await _context.ShortLinks.FirstOrDefaultAsync(s => s.OriginalLinkId == originalLinkId);

            if (shortlink is null)
            {
                response.Message = "Short link with this ID doesn't exist";
                response.StatusCode = StatusCodes.Status404NotFound;

                return response;
            }

            response.Message = "Short link found, client can redirect to the original source";

            response.StatusCode = StatusCodes.Status301MovedPermanently;
            response.IsSuccessful = true;
            response.Data = shortlink;

            return response;

        }


        private bool CheckErrsLink(string originalLink, ServiceResponse<ShortLink> response)
        {
            if (originalLink == string.Empty)
            {
                response.Message = "Link cant be empty";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return true;
            }

            // Checks if Url has valid format
            try
            {
                Uri testUri = new Uri(originalLink);
            }
            catch
            {
                response.Message = "Invalid format of link";
                response.StatusCode = StatusCodes.Status400BadRequest;
                return true;
            }
            
            return false;
        }

        private bool CheckErrsOrignalLinkId(string originalLinkId, ServiceResponse<ShortLink> response)
        {
            if (originalLinkId == string.Empty || originalLinkId.Length != 6)
            {
                response.Message = "ID can't be empty must be 6 chars long";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return true;
            }

            if (originalLinkId.ToLowerInvariant() != originalLinkId)
            {
                response.Message = "ID must be lowercase";
                response.StatusCode = StatusCodes.Status400BadRequest;

                return true;
            }

            return false;
        }

        // Generates unique OriginalLinkId that is 6 chars long from allowedChars and 
        private async Task<string> GenerateOriginalLinkId()
        {
            
            string allowedChars = "abcdefghijklmnopqrstuvwxyz0123456789";
            string generatedLinkId;
            bool exists;


            do
            {

                generatedLinkId = "";
                Random rnd = new Random();

                for (int i = 0; i < 6; i++)
                {
                    int rndIndex = rnd.Next(allowedChars.Length);

                    generatedLinkId += allowedChars[rndIndex];

                }

                // Checks if this newly generated OriginalLinkId already exists in DB if it does then it will repeat this process
                // until it generates unique OriginalLinkId

                var existentShortLink = await _context.ShortLinks.FirstOrDefaultAsync(s => s.OriginalLinkId == generatedLinkId);

                if (existentShortLink is null)
                    exists = false;
                else
                    exists = true;
               
            } while (exists);
                
               
            return generatedLinkId;
        }
    }
}
