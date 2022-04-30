using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;
using Shouldly;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Backpack creation")]
    public class BackpackCreateStepsDefinitions : BaseStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;

        public BackpackCreateStepsDefinitions(ScenarioContext scenarioContext, ApiWebApplicationFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"Sasha is authenticated user")]
        public void GivenAnAuthenticatedUser()
        {
            _scenarioContext["Sasha"] = Sasha;
        }

        [Given(@"he creates a backpack called ""(.*)""")]
        public async Task GivenHeCreatesABackpackCalled(string backpackName)
        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            BackpackModel backpack = await CreateBackpack(backpackName, user);

            _scenarioContext["backpack-name"] = backpackName;
            _scenarioContext["backpack"] = backpack;
        }

        private static async Task<BackpackModel> CreateBackpack(string backpackName, Client user)
        {
            return await user.CreateNewBackpackAsync(new BackpackModelRequest
            {
                Name = backpackName
            });
        }

        [Then(@"returned backpack has same name")]
        public void ThenReturnedBackpackHasSameName()
        {
            var backpack = _scenarioContext.Get<BackpackModel>("backpack");
            var backpackName = _scenarioContext.Get<string>("backpack-name");

            backpack.Name.ShouldBe(backpackName);
        }

        [Then(@"it does not have an empty id")]
        public void ThenItDoesNotHaveAnEmptyId()
        {
            var backpack = _scenarioContext.Get<BackpackModel>("backpack");
            backpack.Id.ShouldNotBe(Guid.Empty);
        }

        [When(@"he queries for his backpacks")]
        public async Task WhenHeQueriesForHisBackpacks()
        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            var backpacks = await user.GetMyBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [When(@"he queries for backpacks")]
        public async Task WhenHeQueriesForBackpacks()
        {
            Client user = _scenarioContext.Get<Client>("Sasha");

            var backpacks = await user.GetBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [Then(@"returned backpacks contain created backpack")]
        public void ThenReturnedBackpacksContainABackpackWithSameName()
        {
            var userBackpacks = _scenarioContext.Get<ImmutableArray<BackpackModel>>("backpacks");
            var backpack = _scenarioContext.Get<BackpackModel>("backpack");
            userBackpacks.ShouldContain(x => x.Name == backpack.Name && x.Id == backpack.Id);
        }

        [Given(@"Ion is a non authenticated user")]
        public void GivenIonIsANonAuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [When(@"Ion tries to create a backpack")]
        public async Task WhenIonTriesToCreateABackpack()
        {
            Client ion = _scenarioContext.Get<Client>("Ion");

            try
            {
                await ion.CreateNewBackpackAsync(new BackpackModelRequest { Name = "a backpack" });
            }
            catch (Exception e)
            {
                _scenarioContext["create-response-exception"] = e;
            }
        }

        [Then(@"(.*) is returned")]
        public void ThenIsReturned(int statusCode)
        {
            var response = _scenarioContext.Get<ApiException>("create-response-exception");
            response.StatusCode.ShouldBe(statusCode);
        }
    }
}
