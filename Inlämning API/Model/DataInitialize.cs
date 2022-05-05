using Microsoft.EntityFrameworkCore;

namespace Inlämning_API.Model;

public class DataInitialize
{
    private readonly APIDbContext _context;

    public DataInitialize(APIDbContext adc)
    {
        _context = adc;
    }

    public void SeedData()
    {
        _context.Database.Migrate();
        CreateIfNotExists("Köp Falukorv", "Falukorv är gott");
        CreateIfNotExists("Skaffa AdBlock", "Ads är inte roligt");
    }

    private void CreateIfNotExists(string title, string fillerText)
    {
        var compValue = title.ToLower();
        if (!_context.advertisements.Any(ad => ad.Title.ToLower() == compValue))
        {
            _context.advertisements.Add(
                new Advertisement
                {
                    Title = title,
                    FillerText = fillerText
                }
                );
            _context.SaveChanges();
        }
    }
}
