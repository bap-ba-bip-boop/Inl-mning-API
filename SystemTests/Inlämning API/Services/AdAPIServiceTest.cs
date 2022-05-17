using Inlämning_API.Services;
using static Inlämning_API.Services.IAdAPIService;
using Inlämning_API.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using SharedResources.Services;

namespace SystemTests.Inlämning_API.Services;

[TestClass]
public class AdAPIServiceTest
{

    private readonly APIDbContext _context;
    private readonly AdAPIService _sut;
    private readonly ICreateUniqeService _creator;
    public AdAPIServiceTest()
    {
        var options = new DbContextOptionsBuilder<APIDbContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        _context = new APIDbContext(options);

        _creator = new CreateUniqeService();

        addAdvertisement(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        addAdvertisement(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        _sut = new AdAPIService();
    }
    private void addAdvertisement(string title, string text) =>
        _creator.CreateIfNotExists(
            _context,
            _context.advertisements!,
            item => item.Title.Equals(title),
            new Advertisement{
                Title=title,
                FillerText=text
            }
        );

    [TestMethod]
    public void When_Account_Exists_Should_Return_AdExists()
    {
        //Arrange
        var existingAccount = _context.advertisements!.First();

        //Act
        var (returnStatus, returnAd) = _sut.VerifyAccountID(existingAccount.Id, _context);

        //Assert
        Assert.IsTrue(returnStatus == AdAPIStatus.AdExists);
        Assert.IsFalse(returnAd == null);
    }
    [TestMethod]
    public void When_Account_Dont_Exist_Should_Return_AdDoesNotExist()
    {
        //Arrange
        var nonExistingAccount = new Advertisement{
            Id = -1,
            Title="title",
            FillerText="text"
        };

        //Act
        var (returnStatus, returnAd) = _sut.VerifyAccountID(nonExistingAccount.Id, _context);

        //Assert
        Assert.IsTrue(returnStatus == AdAPIStatus.AdDoesNotExist);
    }
}