using AutoMapper;
using Inlämning_API.DTO;
using Inlämning_API.Model;
using Inlämning_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using static Inlämning_API.Services.IAccountAPIService;

namespace Inlämning_API.Controllers;

[ApiController]
[Route("[controller]")]
public class AdsController : ControllerBase
{
    private readonly APIDbContext _context;
    private readonly IMapper _mapper;
    private readonly IAccountAPIService _accService;

    public AdsController(APIDbContext adc, IMapper mapper, IAccountAPIService aas)
    {
        _context = adc;
        _mapper = mapper;
        _accService = aas;
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
        var (status, ad) = _accService.VerifyAccountID(Id, _context);

        return (status == AccountStatus.AccountDoesNotExist) ? NotFound() : Ok( _mapper.Map<AdDTO>(ad) );
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

    private NotFoundResult NonSafeHTTPMEthodWrapper(Action action)
    {
        action();
        _context.SaveChanges();
        return NotFound();
    }

    [Authorize]
    [HttpPut]
    [Route("{id}")]
    public IActionResult EditAdvertisement(int Id, EditAdDTO ead)
    {
        var (status, ad) = _accService.VerifyAccountID(Id, _context);
        return (status == AccountStatus.AccountDoesNotExist) ?
            NotFound() :
            NonSafeHTTPMEthodWrapper(
            ()=>
            {
                _mapper.Map(ead, ad);
            }
        );
    }
    [Authorize]
    [HttpDelete]
    [Route("{id}")]
    public IActionResult RemoveAdvertisement(int Id)
    {
        var (status, ad) = _accService.VerifyAccountID(Id, _context);
        return (status == AccountStatus.AccountDoesNotExist) ?
            NotFound() :
            NonSafeHTTPMEthodWrapper( ()=>
            {
                _context.advertisements!.Remove(ad);
            }
        );
    }
    [Authorize]
    [HttpPatch]
    [Route("{id}")]
    public IActionResult PartialUpdateAdvertisement(int Id, [FromBody] JsonPatchDocument<Advertisement> adEntity)
    {
        var (status, ad) = _accService.VerifyAccountID(Id, _context);
        return (status == AccountStatus.AccountDoesNotExist) ?
            NotFound() :
            NonSafeHTTPMEthodWrapper(
            ()=>
            {
                adEntity.ApplyTo(ad);
            }
        );
    }
}