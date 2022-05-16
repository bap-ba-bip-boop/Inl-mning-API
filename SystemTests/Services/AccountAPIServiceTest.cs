using Inlämning_API.Services;
using static Inlämning_API.Services.IAccountAPIService;
using Inlämning_API.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemTests.Services;

[TestClass]
public class AccountAPIServiceTest
{
    private readonly APIDbContext _context;
    private readonly AccountAPIService _sut;
    public AccountAPIServiceTest()
    {
        var options = new DbContextOptionsBuilder<APIDbContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        _context = new APIDbContext(options);

        _context.advertisements.add(
            new Advertisement{
                Title="Köp Falukorv",
                FillerText="Falukorv är gott"
            }
        );
        _context.advertisements.add(
            new Advertisement{
                Title="Skaffa AdBlock",
                FillerText="Ads är inte roligt"
            }
        );
        _context.SaveChanges();

        _sut = new AccountAPIService();
    }
    [TestMethod]
    public void When_Account_Exists_Should_Return_AccountExists()
    {
        //Arrange
        var existingAccount = _context.advertisements.First( account => 
            account.Title == "Köp Falukorv"
        );

        //Act
        var (returnStatus, returnAd) = _sut.VerifyAccountID(existingAccount.Id, _context);

        //Assert
        Assert.IsTrue(returnStatus == AccountStatus.AccountExists);
        Assert.IsFalse(returnAd == null);
    }
}