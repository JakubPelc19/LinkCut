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
            
            if (CheckErrs(trimmedOriginalLink, response))
                return response;

            var existentShortLink = await _context.ShortLinks.FirstOrDefaultAsync(s => s.OriginalLink == new Uri(trimmedOriginalLink));

            if (existentShortLink is not null)
            {
                response.Message = "ShortLink found";
                response.IsSuccessful = true;

                response.Data = existentShortLink;

                return response;
            }

            ShortLink link = new ShortLink();

            link.OriginalLink = new Uri(trimmedOriginalLink);
            link.OriginalLinkId = await GenerateOriginalLinkId();

            await _context.ShortLinks.AddAsync(link);

            await _context.SaveChangesAsync();

            response.Message = "ShortLink was created successfully";

            response.IsSuccessful = true;

            response.Data = link;

            return response;
            
            
        }

        private bool CheckErrs(string originalLink, ServiceResponse<ShortLink> response)
        {
            if (originalLink == string.Empty)
            {
                response.Message = "Link cant be empty";
                return true;
            }

            try
            {
                Uri testUri = new Uri(originalLink);
            }
            catch
            {
                response.Message = "Invalid format of link";
                return true;
            }
            
            return false;
        }

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
