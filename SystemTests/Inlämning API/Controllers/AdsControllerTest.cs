using AutoMapper;
using Inlämning_API.Controllers;
using Inlämning_API.DTO;
using Inlämning_API.Infrastructure.Profiles;
using Inlämning_API.Model;
using Inlämning_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedResources.Services;
using static SharedResources.Services.ICreateUniqeService;

namespace SystemTests.Inlämning_API.Controllers;

[TestClass]
public class AdsControllerTest
{
    private readonly APIDbContext _context;
    private readonly CreateUniqeService _creator;
    private readonly AdsController _sut;

    public string testTitle { get; set; }
    public string testText { get; set; }

    private readonly int OkResponse = 200;
    private readonly int NotFoundResponse = 404;
    private readonly int NoContentResponse = 204;
    private readonly int CreatedAtResponse = 201;

    public AdsControllerTest()
    {
        var options = new DbContextOptionsBuilder<APIDbContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;
        _context = new APIDbContext(options);
        _creator = new CreateUniqeService();

        testTitle = Guid.NewGuid().ToString();
        testText = Guid.NewGuid().ToString();

        addAdvertisement(testTitle, testText);

        _sut = createAPI();
    }
    private CreateUniqueStatus addAdvertisement(string title, string text) =>
        _creator.CreateIfNotExists(
            _context,
            _context.advertisements!,
            item => item.Title.Equals(title),
            new Advertisement
            {
                Title = title,
                FillerText = text
            }
        );
    private AdsController createAPI()
    {
        var conf = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AdsProfile>();
            cfg.AddProfile<JwtSettingsProfile>();
        }
        );
        var app = new AdsController(_context, new Mapper(conf), new AdAPIService());
        app.ControllerContext.HttpContext = new DefaultHttpContext();
        return app;
    }
    private bool DefaultAPIResponseCodeCheck(IActionResult response, int returnCodeCompare)
    {
        var StatusCode = 0;
        StatusCode = (int)response.GetType().GetProperty(nameof(StatusCode))!.GetValue(response)!;
        return StatusCode.Equals(returnCodeCompare);
    }
    private bool APITestResponseCode<ReturnType>(Func<ReturnType> ActAction, Func<ReturnType, bool> AssertAction)
    {
        return AssertAction(ActAction());
    }
    //HTTP GET
    [TestMethod]
    public void When_Call_Get_Method_All_Items_Should_Return()
    {
        //Arrange
        var returnCodeCompare = OkResponse;
        //Act
        //Assert
        Assert.IsTrue(
            APITestResponseCode(
                () =>_sut.Index(),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    //HTTP GET/ID
    [TestMethod]
    public void When_Call_Get_Single_Method_With_Valid_Id()
    {
        var returnCodeCompare = OkResponse;
        var existingItem = _context.advertisements.First();

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.GetOne(existingItem.Id),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );

    }
    [TestMethod]
    public void When_Call_Get_Single_Method_With_Invalid_Id()
    {
        var returnCodeCompare = NotFoundResponse;
        var nonExistingID = -1;

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.GetOne(nonExistingID),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    //HTTP POST
    [TestMethod]
    public void When_Call_Post_Single_Method_With_New_Ad()
    {
        var returnCodeCompare = CreatedAtResponse;
        var title = Guid.NewGuid().ToString();
        var text = Guid.NewGuid().ToString();

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.CreateAdvertisement(
                    new CreateAdDTO
                    {
                        Title = title,
                        FillerText = text
                    }
                ),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    //HTTP PUT
    [TestMethod]
    public void When_Call_Put_With_Invalid_Id()
    {
        var returnCodeCompare = NotFoundResponse;
        var nonExistingID = -1;

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.EditAdvertisement(nonExistingID, null),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    [TestMethod]
    public void When_Call_Put_With_Valalid_Id()
    {
        var returnCodeCompare = NoContentResponse;
        var existingItem = _context.advertisements.First();
        var existingID = existingItem.Id;

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.EditAdvertisement(
                    existingID,
                    new EditAdDTO
                    {
                        Title = existingItem.Title,
                        FillerText = existingItem.FillerText
                    }
                ),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    //HTTP DELETE
    [TestMethod]
    public void When_Call_Delete_With_Invalalid_Id()
    {
        var returnCodeCompare = NotFoundResponse;
        var nonExistingId = -1;

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.RemoveAdvertisement(
                    nonExistingId
                ),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    [TestMethod]
    public void When_Call_Delete_With_Valalid_Id()
    {
        var returnCodeCompare = NoContentResponse;
        var existingID = _context.advertisements.First().Id;

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.RemoveAdvertisement(
                    existingID
                ),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    //HTTP PATCH
    [TestMethod]
    public void When_Call_Patch_With_Invalalid_Id()
    {
        var returnCodeCompare = NotFoundResponse;
        var nonExistingId = -1;

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.PartialUpdateAdvertisement(
                    nonExistingId,
                    null
                ),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
    [TestMethod]
    public void When_Call_Patch_With_Valalid_Id()
    {
        var returnCodeCompare = NoContentResponse;
        var existingID = _context.advertisements.First().Id;
        
        var body = new JsonPatchDocument<Advertisement>();
        body.Replace(Ad => Ad.FillerText, "This is the new Value");

        Assert.IsTrue(
            APITestResponseCode(
                () => _sut.PartialUpdateAdvertisement(
                    existingID,
                    body
                ),
                response => DefaultAPIResponseCodeCheck(response, returnCodeCompare)
            )
        );
    }
}