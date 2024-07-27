using System.Text;
using MatchDetailsApp.Controllers;
using MatchDetailsApp.Models.Domain;
using MatchDetailsApp.Models.DTOs;
using MatchDetailsApp.Repositories.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MatchDetailsApp.Tests;

public class XmlFileControllerTests
{
    private readonly Mock<XmlFileRepository> _mockRepository;
    private readonly XmlFileController _controller;

    public XmlFileControllerTests()
    {
        _mockRepository = new Mock<XmlFileRepository>();
        _controller = new XmlFileController(_mockRepository.Object);
    }


    [Fact]
    public async Task UploadXml_ValidFile_ReturnsOkResult()
    {
        // Arrange
        var content = "<PutDataRequest><Fixtures><Fixture MatchId='1' MatchDay='1' HomeTeamName='Team A' GuestTeamName='Team B' PlannedKickoffTime='2024-07-26T14:00:00' StadiumName='Stadium A' /></Fixtures></PutDataRequest>";
        var fileName = "file.xml";
        var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(content)), 0, content.Length, "name", fileName);
        var matchDays = new HashSet<int> { 1 };

        _mockRepository.Setup(repo => repo.ProcessXmlFileAsync(It.IsAny<IFormFile>())).ReturnsAsync(matchDays);

        // Act
        var result = await _controller.UploadXml(file);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var response = actionResult.Value as dynamic;
        Assert.NotNull(response);
        Assert.Equal("File uploaded and data saved!", (string)response.message);
        Assert.Equal(matchDays.ToList(), ((IEnumerable<int>)response.matchDays).ToList());
    }

    [Fact]
    public async Task GetByMatchDay_ValidMatchDay_ReturnsOkResult()
    {
        // Arrange
        var matchDay = 1;
        var values = new List<Value>
        {
            new Value
            { MatchDay = matchDay,
                HomeTeamName = "Team A",
                GuestTeamName = "Team B",
                PlannedKickoffTime = DateTime.Now,
                StadiumName = "Stadium A"
            }
        };
        _mockRepository.Setup(repo => repo.GetByDay(matchDay)).ReturnsAsync(values);

        // Act
        var result = await _controller.GetByMatchDay(matchDay);

        // Assert
        var actionResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<ValueDto>>(actionResult.Value);
        Assert.Single(response);
        Assert.Equal("Team A", response[0].HomeTeamName);
    }
}
