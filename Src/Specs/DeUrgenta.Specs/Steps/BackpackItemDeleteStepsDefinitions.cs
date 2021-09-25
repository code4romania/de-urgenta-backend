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
    [Scope(Feature = "Backpack item delete")]
    public class BackpackItemDeleteStepsDefinitions : BaseStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;

        public BackpackItemDeleteStepsDefinitions(
            ScenarioContext scenarioContext,
            ApiWebApplicationFactory factory) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"Sasha is authenticated user")]
        public void GivenSashaIsAuthenticatedUser()
        {
            _scenarioContext["Sasha"] = Sasha;
        }

        [Given(@"Sasha creates a backpack")]
        public async Task GivenSashaCreatesABackpack()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var backpack = await sasha.CreateNewBackpackAsync(new BackpackModelRequest { Name = "El backpacko" });
            _scenarioContext.Add("backpackId", backpack.Id);
        }

        [Given(@"Grisha is authenticated user")]
        public void GivenGrishaIsAuthenticatedUser()
        {
            _scenarioContext["Grisha"] = Grisha;
        }

        [Given(@"Grisha is a backpack contributor")]
        public async Task GivenGrishaIsABackpackContributor()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await BackpackDriver.AddToBackpackContributor(sasha, grisha, backpackId);
        }

        private async Task CreateBackpackItem(Client user, Guid backpackId)
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                Amount = 3,
                CategoryType = BackpackCategoryType._5,
                ExpirationDate = DateTime.Now.AddDays(350),
                Name = "A thing"
            };

            var backpackItemResponse = await user.CreateNewBackpackItemAsync(backpackId, backpackItemRequest);

            _scenarioContext.Add("backpack-item-id", backpackItemResponse.Id);
            _scenarioContext.Add("backpack-item", backpackItemResponse);
        }

        [Given(@"Sasha creates an item")]
        public async Task GivenSashaCreatesAnItem()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(sasha, backpackId);
        }

        [Given(@"Sasha deletes created backpack item")]
        public async Task GivenSashaDeletesCreatedBackpackItem()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            await sasha.DeleteBackpackItemAsync(itemId);
        }

        [When(@"Sasha queries for backpack items")]
        public async Task WhenSashaQueriesForBackpackItems()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            var backpackItems = await sasha.GetBackpackItemsAsync(backpackId);

            _scenarioContext.Add("backpack-items", backpackItems.ToImmutableArray());
        }

        [Then(@"backpack item list does not contains updated item")]
        public void ThenBackpackItemListDoesNotContainsUpdatedItem()
        {
            var backpackItems = _scenarioContext.Get<IImmutableList<BackpackItemModel>>("backpack-items");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            var createdBackpackItem = _scenarioContext.Get<BackpackItemModel>("backpack-item");

            backpackItems.ShouldNotContain(x => x.Id == itemId
                                             && createdBackpackItem.Name == x.Name
                                             && createdBackpackItem.ExpirationDate == x.ExpirationDate
                                             && createdBackpackItem.CategoryType == x.CategoryType
                                             && createdBackpackItem.Amount == x.Amount);
        }

        [When(@"Grisha queries for backpack items")]
        public async Task WhenGrishaQueriesForBackpackItems()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            var backpackItems = await grisha.GetBackpackItemsAsync(backpackId);

            _scenarioContext.Add("backpack-items", backpackItems.ToImmutableArray());
        }

        [Given(@"Grisha creates an item")]
        public async Task GivenGrishaCreatesAnItem()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(grisha, backpackId);
        }

        [Given(@"Grisha deletes created backpack item")]
        public async Task GivenGrishaDeletesCreatedBackpackItem()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            await grisha.DeleteBackpackItemAsync(itemId);
        }

        [Given(@"Jora is a registered user")]
        public void GivenJoraIsARegisteredUser()
        {
            _scenarioContext["Jora"] = Jora;
        }

        [When(@"Jora deletes created item")]
        public async Task WhenJoraDeletesCreatedItem()
        {
            var jora = _scenarioContext.Get<Client>("Jora");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            try
            {
                await jora.DeleteBackpackItemAsync(itemId);
            }
            catch (Exception e)
            {
                _scenarioContext.Add("delete-item-response", e);
            }
        }

        [Then(@"gets BadRequest in response")]
        public void ThenGetsBadRequestInResponse()
        {
            object updateItemResponse = _scenarioContext["delete-item-response"];

            updateItemResponse.ShouldBeOfType<ApiException<ProblemDetails>>();
            (updateItemResponse as ApiException<ProblemDetails>).StatusCode.ShouldBe(400);
        }

        [Given(@"Ion is un-authenticated user")]
        public void GivenIonIsUn_AuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [When(@"Ion deletes created item")]
        public async Task WhenIonDeletesCreatedItem()
        {
            var ion = _scenarioContext.Get<Client>("Ion");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            try
            {
                await ion.DeleteBackpackItemAsync(itemId);
            }
            catch (Exception e)
            {
                _scenarioContext.Add("update-item-response", e);
            }
        }

        [Then(@"gets Unauthorized in response")]
        public void ThenGetsUnauthorizedInResponse()
        {
            object updateItemResponse = _scenarioContext["update-item-response"];

            updateItemResponse.ShouldBeOfType<ApiException>();
            (updateItemResponse as ApiException).StatusCode.ShouldBe(401);
        }

    }
}