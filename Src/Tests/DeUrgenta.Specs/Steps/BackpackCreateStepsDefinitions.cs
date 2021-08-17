using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Drivers;
using Shouldly;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Backpack creation")]
    public class BackpackCreateStepsDefinitions : BaseApiStep
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
            ApiClient user = _scenarioContext.Get<ApiClient>("Sasha");

            BackpackModel backpack = await CreateBackpack(backpackName, user);

            _scenarioContext["backpack-name"] = backpackName;
            _scenarioContext["backpack"] = backpack;
        }

        private static async Task<BackpackModel> CreateBackpack(string backpackName, ApiClient user)
        {
            return await user.CreateNewBackpackAsync(new BackpackModelRequest()
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
            ApiClient user = _scenarioContext.Get<ApiClient>("Sasha");

            var backpacks = await user.GetMyBackpacksAsync();
            _scenarioContext["backpacks"] = backpacks.ToImmutableArray();
        }

        [When(@"he queries for backpacks")]
        public async Task WhenHeQueriesForBackpacks()
        {
            ApiClient user = _scenarioContext.Get<ApiClient>("Sasha");

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
    }
}
