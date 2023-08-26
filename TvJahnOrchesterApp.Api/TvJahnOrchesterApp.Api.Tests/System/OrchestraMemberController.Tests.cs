using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Api.Controllers;
using TvJahnOrchesterApp.Api.Tests.Fixures;
using TvJahnOrchesterApp.Application.OrchestraMembers.Commands.Create;
using TvJahnOrchesterApp.Application.Services;
using TvJahnOrchesterApp.Contracts.OrchestraMembers;

namespace TvJahnOrchesterApp.Api.Tests.System
{
    public class OrchestraMemberControllerTests
    {
        [Fact]
        public async Task CreateOrchesterMember_ShouldReturn200()
        {
            //Arrange
            var sut = CreateOrchestraMemberController();
            var orchesterMemberContract = OrchesterMemberFixture.GetOrchestraMemberContract();

            //Act
            var result = (ObjectResult) await sut.CreateAsync(orchesterMemberContract, CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task CreateOrchesterMember_GivenInvalidOrchestraMemberContract_ShouldReturnInvalidArgumentException()
        {
            //Arrange
            var sut = CreateOrchestraMemberController();
            var orchesterMemberContract = OrchesterMemberFixture.GetInvalidOrchestraMemberContractWithoutFirstName();

            //Act
            var result = (ObjectResult) await sut.CreateAsync(orchesterMemberContract, CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Invalid Contract");
        }

        [Fact]
        public async Task CreateOrchesterMember_GivenInvalidOrchestraMemberContractWithoutLastName_ShouldReturnInvalidArgumentException()
        {
            //Arrange
            var sut = CreateOrchestraMemberController();
            var orchesterMemberContract = OrchesterMemberFixture.GetInvalidOrchestraMemberContractWithoutLastName();

            //Act
            var result = (ObjectResult) await sut.CreateAsync(orchesterMemberContract, CancellationToken.None);

            //Assert
            result.StatusCode.Should().Be(400);
            result.Value.Should().Be("Invalid Contract");
        }

        [Fact]
        public async Task CreateOrchestraMember_ShouldSendCreateOrchestraMemberCommandOnce()
        {
            //Arrange
            var senderMock = new Mock<ISender>();
            var sut = CreateOrchestraMemberController(senderMock.Object);
            var orchesterMemberContract = OrchesterMemberFixture.GetOrchestraMemberContract();
            var createOrchestraMemberCommand = CreateOrchestraMemberCommandFromContract(orchesterMemberContract);
            //Act
            await sut.CreateAsync(orchesterMemberContract, CancellationToken.None);
            //Assert
            senderMock.Verify(s => s.Send(createOrchestraMemberCommand, CancellationToken.None), Times.Once());
        }

        public static OrchestraMemberController CreateOrchestraMemberController()
        {
            var serviceMock = new Mock<ISender>();
            return new OrchestraMemberController(serviceMock.Object);
        }

        public static OrchestraMemberController CreateOrchestraMemberController(ISender sender)
        {
            return new OrchestraMemberController(sender);
        }

        public static CreateOrchestraMemberCommand CreateOrchestraMemberCommandFromContract(OrchestraMemberContract orchesterMemberContract)
        {
            return new CreateOrchestraMemberCommand
            {
                FirstName = orchesterMemberContract.FirstName,
                LastName = orchesterMemberContract.LastName,
                InstrumentIds = orchesterMemberContract.InstrumentIds,
                PhoneNumber = orchesterMemberContract.PhoneNumber,
                Street = orchesterMemberContract.Address.Street,
                HouseNumber = orchesterMemberContract.Address.HouseNumber,
                City = orchesterMemberContract.Address.City,
                Zip = orchesterMemberContract.Address.Zip,
            };
        }
    }
}
