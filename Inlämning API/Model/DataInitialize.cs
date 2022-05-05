using Microsoft.EntityFrameworkCore;

namespace Inlämning_API.Model
{
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
            CreateAdvertisementIfNotExists("Köp Falukorv", "Falukorv är gott");
            CreateAdvertisementIfNotExists("Skaffa AdBlock", "Ads är inte roligt");
            CreateUserIfNotExists("admin@abc.com", "$admin@2022");
        }

        private void CreateUserIfNotExists(string username, string password)
        {
            var compValue = username.ToLower();
            if (!_context.users.Any(user => user.Email.ToLower() == compValue))
            {
                _context.users.Add(new UserInfo { Email = username, Password = password, CreatedDate = DateTime.Now, DisplayName = "testAdmin", UserName = "testAdmin" });
                _context.SaveChanges();
            }
        }

        private void CreateAdvertisementIfNotExists(string title, string fillerText)
        {
            var compValue = title.ToLower();
            if (!_context.advertisements.Any(ad => ad.Title.ToLower() == compValue) )
            {
                _context.advertisements.Add(new Advertisement { Title = title, fillerText = fillerText });
                _context.SaveChanges();
            }
        }
    }
}
