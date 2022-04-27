using Inlämning_API.DTO;
using Inlämning_API.Model;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Inlämning_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly APIDbContext _context;

        public AdsController(APIDbContext adc)
        {
            _context = adc;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_context.advertisements.Select(ad => 
            new AdItemsDTO
            {
                Id = ad.Id,
                Title = ad.Title
            }).ToList());
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOne(int Id)
        {
            var advertisement = _context.advertisements.FirstOrDefault( ad => ad.Id == Id);
            return (advertisement == null) ? NotFound() : Ok(
                new AdDTO
                {
                    Title = advertisement.Title,
                    fillerText = advertisement.fillerText
                }
                );
        }
        [HttpPost]
        public IActionResult CreateAdvertisement(CreateAdDTO cad)
        {
            var newAd = new Advertisement
            {
                Title = cad.Title,
                fillerText = cad.fillerText
            };
            _context.advertisements.Add(newAd);
            _context.SaveChanges();

            return CreatedAtAction(
                nameof(GetOne),
                new { Id = newAd.Id },
                new AdDTO
                {
                    Title = newAd.Title,
                    fillerText= newAd.fillerText
                }
                );
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult EditAdvertisement(int Id, EditAdDTO ead)
        {
            var ad = _context.advertisements.FirstOrDefault(ad => ad.Id == Id);
            if (ad == null)
                return NotFound();

            ad.Title = ead.Title;
            ad.fillerText = ead.fillerText;

            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult RemoveAdvertisement(int Id)
        {
            var ad = _context.advertisements.FirstOrDefault(ad => ad.Id == Id);
            if (ad == null)
                return NotFound();

            _context.advertisements.Remove(ad);
            _context.SaveChanges();

            return NoContent();
        }
        [HttpPatch]
        [Route("{id}")]
        public IActionResult PartialUpdateAdvertisement(int Id, [FromBody] JsonPatchDocument<Advertisement> adEntity)
        {
            var ad = _context.advertisements.FirstOrDefault(ad => ad.Id == Id);
            if (ad == null)
                return NotFound();

            var patchData = new Advertisement();

            adEntity.ApplyTo(patchData);

            return NoContent();
        }
    }
}