using Inlämning_API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedResources.Services;
using static SharedResources.Services.ICreateUniqeService;

namespace SystemTests.SharedResources;

[TestClass]
public class CreateUniqueServiceTest
{
    public string testTitle { get; set; }
    public string testText { get; set; }
    private readonly APIDbContext _context;

    private readonly ICreateUniqeService _sut;
    public CreateUniqueServiceTest()
    {
        var options = new DbContextOptionsBuilder<APIDbContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        _context = new APIDbContext(options);
        _sut = new CreateUniqeService();

        testTitle= "Köp Falukorv";
        testText="Falukorv är gott";

        _ = addAdvertisement(testTitle, testText);

    }
    private CreateUniqueStatus addAdvertisement(string title, string text) =>
        _sut.CreateIfNotExists(
            _context,
            _context.advertisements!,
            item => item.Title == title,
            new Advertisement{
                Title=title,
                FillerText=text
            }
        );
    [TestMethod]
    public void When_Ad_Exists_Should_Return_AlreadyExists()
    {
        //Arrange
        var title = testTitle;
        var text = testText;

        //Act
        var returnStatus = addAdvertisement(title, text);

        //Assert
        Assert.IsTrue(returnStatus == CreateUniqueStatus.AlreadyExists);
    }
    [TestMethod]
    public void When_Ad_Dont_Exists_Should_Return_Ok()
    {
        //Arrange
        var title = testTitle + "1";
        var text = testText + "1";

        //Act
        var returnStatus = addAdvertisement(title, text);

        //Assert
        Assert.IsTrue(returnStatus == CreateUniqueStatus.Ok);
    }
}