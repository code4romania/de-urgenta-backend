using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;
using DeUrgenta.Specs.Drivers;
using Shouldly;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Backpack delete")]
    public class BackpackDeleteStepsDefinitions : BaseApiStep
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly BackpackDriver _backpackDriver;
        public BackpackDeleteStepsDefinitions(
            ScenarioContext scenarioContext,
            BackpackDriver backpackDriver,
            ApiWebApplicationFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
            _backpackDriver = backpackDriver;
        }

        [Given(@"Sasha is authenticated user")]
        public void GivenSashaIsAuthenticatedUser()
        {
            _scenarioContext["Sasha"] = Sasha;
        }

        [Given(@"Sasha creates a backpack named ""(.*)""")]
        public async Task GivenSashaCreatesABackpackNamed(string backpackName)
        {
            var user = _scenarioContext.Get<Client>("Sasha");

            var backpack = await user.CreateNewBackpackAsync(new()
            {
                Name = backpackName
            });

            _scenarioContext["backpack-id"] = backpack.Id;
        }

        [When(@"owner deletes backpack named ""My backpack""")]
        public async Task WhenOwnerDeletesBackpackNamed()
        {
            var user = _scenarioContext.Get<Client>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            await DeleteBackpack(user, backpackId);
        }

        [When(@"owner queries for his backpacks")]
        public async Task WhenOwnerQueriesForHisBackpacks()
        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            var backpacks = await user.GetMyBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [Then(@"returned backpacks does not contain deleted backpack")]
        public void ThenReturnedBackpacksDoesNotContainDeletedBackpack()
        {
            var userBackpacks = _scenarioContext.Get<ImmutableArray<BackpackModel>>("backpacks");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            userBackpacks.ShouldNotContain(x => x.Id == backpackId);
        }

        [When(@"owner queries for backpacks")]
        public async Task WhenOwnerQueriesForBackpacks()
        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            var backpacks = await user.GetBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [Given(@"Grisha is authenticated user")]
        public void GivenGrishaIsAuthenticatedUser()
        {
            _scenarioContext["Grisha"] = Grisha;
        }

        [When(@"Grisha deletes backpack created by Sasha")]
        public async Task WhenGrishaDeletesBackpackCreatedBySasha()
        {
            var user = _scenarioContext.Get<Client>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            await DeleteBackpack(user, backpackId);
        }

        private async Task DeleteBackpack(Client user, Guid backpackId)
        {
            object deleteResponse = null;

            try
            {
                await user.DeleteBackpackAsync(backpackId);
            }
            catch (Exception e)
            {
                deleteResponse = e;
            }

            _scenarioContext["delete-response"] = deleteResponse;
        }

        [Then(@"gets BadRequest in response")]
        public void ThenGetsBadRequestInResponse()
        {
            object deleteResponse = _scenarioContext["delete-response"];

            deleteResponse.ShouldBeOfType<ApiException<ProblemDetails>>();
            (deleteResponse as ApiException<ProblemDetails>).StatusCode.ShouldBe(400);
        }

        [Given(@"is a contributor to Sasha's backpack")]
        public async Task GivenIsAContributorToSashaSBackpack()
        {
            var backpackOwner = _scenarioContext.Get<Client>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            await _backpackDriver.AddToBackpackContributor(backpackOwner, Grisha, backpackId);
        }

        [Given(@"Ion is un-authenticated user")]
        public void GivenIonIsUn_AuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [When(@"Ion deletes backpack created by Sasha")]
        public async Task WhenIonDeletesBackpackCreatedBySasha()
        {
            var user = _scenarioContext.Get<Client>("Ion");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            await DeleteBackpack(user, backpackId);
        }

        [Then(@"gets Unauthorized in response")]
        public void ThenGetsUnauthorizedInResponse()
        {
            object deleteResponse = _scenarioContext["delete-response"];

            deleteResponse.ShouldBeOfType<ApiException>();
            (deleteResponse as ApiException).StatusCode.ShouldBe(401);
        }
    }
}
