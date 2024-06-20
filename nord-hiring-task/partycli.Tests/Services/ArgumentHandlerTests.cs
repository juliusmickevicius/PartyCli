using Moq;
using NUnit.Framework;
using partycli.Domain.Enums;
using partycli.Infrastructure.Repository.Settings;
using partycli.Options;
using partycli.Services.ArgumentHandlerService;
using partycli.Services.MessageDisplay;
using partycli.Services.Server;
using System.Threading.Tasks;

namespace partycli.Tests.Services
{
    public  class ArgumentHandlerTests
    {
        Mock<IServersHttpService> _serversHttpServiceMock;
        Mock<IMessageDisplayService> _messageDisplayService;
        Mock<ISettingsRepository> _settingsRepositoryMock;
        IArgumentHandlerService _argumentHandlerService;

        [SetUp]
        public void Setup()
        {
            _serversHttpServiceMock = new Mock<IServersHttpService>();
            _messageDisplayService = new Mock<IMessageDisplayService>();
            _settingsRepositoryMock = new Mock<ISettingsRepository>();
            _argumentHandlerService = new ArgumentHandlerService(_serversHttpServiceMock.Object, _settingsRepositoryMock.Object, _messageDisplayService.Object);
        }

        [Test]
        public async Task ProcessArguments_Provide_main_arg_Should_return_server_list()
        {
            //Arrange
            _settingsRepositoryMock.Setup(x => x.GetServerListData()).Returns("Fake Value");
            _serversHttpServiceMock.Setup(x => x.GetAllServerByCountryListAsync(null)).ReturnsAsync("Fake servers");
            var args = new ArgumentOptions() { PrimaryArgument = ParentArgument.server_list };

            //Act
            var state = await _argumentHandlerService.ProcessArgumentsAsync(args);

            //Assert
            Assert.AreEqual(State.server_list, state);
        }

        [Test]
        public async Task ProcessArguments_No_servers_received_Should_return_none()
        {
            //Arrange
            var args = new ArgumentOptions() { PrimaryArgument = ParentArgument.server_list };

            //Act
            var state = await _argumentHandlerService.ProcessArgumentsAsync(args);

            //Assert
            Assert.AreEqual(State.none, state);
        }

        [Test]
        public async Task ProcessArguments_Ask_for_tcp_Should_return_server_list()
        {
            //Arrange
            _serversHttpServiceMock.Setup(x => x.GetAllServerByProtocolListAsync((int)Protocol.Tcp)).ReturnsAsync("Fake servers");
            var args = new ArgumentOptions() { PrimaryArgument = ParentArgument.server_list, IsTcp = true };

            //Act
            var state = await _argumentHandlerService.ProcessArgumentsAsync(args);

            //Assert
            Assert.AreEqual(State.server_list, state);
        }

        [Test]
        public async Task ProcessArguments_Ask_for_france_Should_return_server_list()
        {
            //Arrange
            _serversHttpServiceMock.Setup(x => x.GetAllServerByCountryListAsync((int)Country.France)).ReturnsAsync("Fake servers");
            var args = new ArgumentOptions() { PrimaryArgument = ParentArgument.server_list, IsFrance = true };

            //Act
            var state = await _argumentHandlerService.ProcessArgumentsAsync(args);

            //Assert
            Assert.AreEqual(State.server_list, state);
        }

        [Test]
        public async Task ProcessArguments_Ask_for_local_Should_return_server_list()
        {
            //Arrange
            _settingsRepositoryMock.Setup(x => x.GetServerListData()).Returns("Fake Value");
            var args = new ArgumentOptions() { PrimaryArgument = ParentArgument.server_list, IsLocal = true };

            //Act
            var state = await _argumentHandlerService.ProcessArgumentsAsync(args);

            //Assert
            Assert.AreEqual(State.server_list, state);
        }
    }
}
