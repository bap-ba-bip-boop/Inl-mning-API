using AutoMapper;
using Inlämning_API.DTO;
using Inlämning_API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Inlämning_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AdsController : ControllerBase
{
    private readonly APIDbContext _context;
    private readonly IMapper _mapper;

    public AdsController(APIDbContext adc, IMapper mapper)
    {
        _context = adc;
        _mapper = mapper;
    }
    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        return Ok(
            _context.advertisements!.Select(ad =>
                _mapper.Map<AdItemsDTO>(ad)
            ).ToList()
        );
    }
    [HttpGet]
    [Authorize]
    [Route("{id}")]
    public IActionResult GetOne(int Id)
    {
        var advertisement = _context.advertisements!.FirstOrDefault(ad => ad.Id == Id);
        return (advertisement == null) ?
            NotFound() :
            Ok(
                _mapper.Map<AdDTO>(advertisement)
            );
    }
    [Authorize]
    [HttpPost]
    public IActionResult CreateAdvertisement(CreateAdDTO cad)
    {
        var newAd = _mapper.Map<Advertisement>(cad);

        _context.advertisements!.Add(newAd);
        _context.SaveChanges();

        return CreatedAtAction(
            nameof(GetOne),
            new { Id = newAd.Id },
            _mapper.Map<AdDTO>(newAd)
            );
    }
    [Authorize]
    [HttpPut]
    [Route("{id}")]
    public IActionResult EditAdvertisement(int Id, EditAdDTO ead)
    {
        var ad = _context.advertisements!.FirstOrDefault(ad => ad.Id == Id);
        if (ad == null)
            return NotFound();

        _mapper.Map(ead, ad);

        _context.SaveChanges();
        return NoContent();
    }
    [Authorize]
    [HttpDelete]
    [Route("{id}")]
    public IActionResult RemoveAdvertisement(int Id)
    {
        var ad = _context.advertisements!.FirstOrDefault(ad => ad.Id == Id);
        if (ad == null)
            return NotFound();

        _context.advertisements!.Remove(ad);
        _context.SaveChanges();

        return NoContent();
    }
    [Authorize]
    [HttpPatch]
    [Route("{id}")]
    public IActionResult PartialUpdateAdvertisement(int Id, [FromBody] JsonPatchDocument<Advertisement> adEntity)
    {
        var ad = _context.advertisements!.FirstOrDefault(ad => ad.Id == Id);
        if (ad == null)
            return NotFound();

        adEntity.ApplyTo(ad);
        _context.SaveChanges();

        return NoContent();
    }
}
