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
    [Scope(Feature = "Backpack update")]
    public class BackpackUpdateStepsDefinitions : BaseStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;
        public BackpackUpdateStepsDefinitions(
            ScenarioContext scenarioContext,
            ApiWebApplicationFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"Sasha is authenticated user")]
        public void GivenSashaAnAuthenticatedUser()
        {
            _scenarioContext["Sasha"] = Sasha;
        }

        [Given(@"Grisha is authenticated user")]
        public void GivenGrishaAnAuthenticatedUser()
        {
            _scenarioContext["Grisha"] = Grisha;
        }

        [Given(@"Ion is un-authenticated user")]
        public void GivenIonIsUn_AuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [Given(@"Sasha creates a backpack named ""(.*)""")]
        public async Task GivenHasABackpack(string backpackName)
        {
            var user = _scenarioContext.Get<Client>("Sasha");

            var backpack = await user.CreateNewBackpackAsync(new()
            {
                Name = backpackName
            });

            _scenarioContext["backpack-id"] = backpack.Id;
        }

        [When(@"owner edits backpack name to ""(.*)""")]
        public async Task WhenEditBackpackNameTo(string newBackpackName)
        {
            var user = _scenarioContext.Get<Client>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            var backpackModel = await user.UpdateBackpackAsync(backpackId, new()
            {
                Name = newBackpackName
            });

            _scenarioContext["updated-backpack"] = backpackModel;
        }

        [When(@"owner queries for his backpacks")]
        public async Task WhenHeQueriesForHisBackpacks()

        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            var backpacks = await user.GetMyBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [When(@"owner queries for backpacks")]
        public async Task WhenHeQueriesForBackpacks()
        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            var backpacks = await user.GetBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [Then(@"returned backpacks contain updated backpack")]
        public void ThenReturnedBackpacksContainUpdatedBackpack()
        {
            var userBackpacks = _scenarioContext.Get<ImmutableArray<BackpackModel>>("backpacks");
            var updatedBackpack = _scenarioContext.Get<BackpackModel>("updated-backpack");

            userBackpacks.ShouldContain(x => x.Name == updatedBackpack.Name && x.Id == updatedBackpack.Id);
        }

        [When(@"Grisha edits backpack created by Sasha")]
        public async Task WhenEditBackpackCreatedBySasha()
        {
            var user = _scenarioContext.Get<Client>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            object updateResponse;
            try
            {
                updateResponse = await user.UpdateBackpackAsync(backpackId, new()
                {
                    Name = "a name"
                });
            }
            catch (Exception e)
            {
                updateResponse = e;
            }

            _scenarioContext["update-response"] = updateResponse;
        }

        [Then(@"gets BadRequest in response")]
        public void ThenGetsResponse()
        {
            object updateResponse = _scenarioContext["update-response"];

            updateResponse.ShouldBeOfType<ApiException<ProblemDetails>>();
            (updateResponse as ApiException<ProblemDetails>).StatusCode.ShouldBe(400);
        }

        [Given(@"is a contributor to Sasha's backpack")]
        public async Task GivenGrishaIsAuthenticatedUserAndAContributor()
        {
            var backpackOwner = _scenarioContext.Get<Client>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            await BackpackDriver.AddToBackpackContributor(backpackOwner, Grisha, backpackId);
        }

        [When(@"Ion edits backpack created by Sasha")]
        public async Task WhenIonEditsBackpackCreatedBySasha()
        {
            var user = _scenarioContext.Get<Client>("Ion");
            var backpackId = _scenarioContext.Get<Guid>("backpack-id");

            object updateResponse;
            try
            {
                updateResponse = await user.UpdateBackpackAsync(backpackId, new()
                {
                    Name = "a name"
                });
            }
            catch (Exception e)
            {
                updateResponse = e;
            }

            _scenarioContext["update-response"] = updateResponse;
        }

        [Then(@"gets Unauthorized in response")]
        public void ThenGetsUnauthorizedInResponse()
        {
            object updateResponse = _scenarioContext["update-response"];

            updateResponse.ShouldBeOfType<ApiException>();
            (updateResponse as ApiException).StatusCode.ShouldBe(401);
        }

        [When(@"Grisha queries for backpacks")]
        public async Task WhenGrishaQueriesForBackpacks()
        {
            var user = _scenarioContext.Get<Client>("Grisha");

            var backpacks = await user.GetBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

    }
}
