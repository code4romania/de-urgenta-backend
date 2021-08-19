using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Drivers;
using Shouldly;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Backpack item update")]
    public class BackpackItemUpdateStepsDefinitions : BaseApiStep
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly BackpackDriver _backpackDriver;

        public BackpackItemUpdateStepsDefinitions(
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

        [Given(@"Sasha creates a backpack")]
        public async Task GivenSashaCreatesABackpack()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
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
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await _backpackDriver.AddToBackpackContributor(sasha, grisha, backpackId);
        }
        private async Task CreateBackpackItem(ApiClient user, Guid backpackId)
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
        }

        [Given(@"Sasha creates an item")]
        public async Task GivenSashaCreatesAnItem()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(sasha, backpackId);
        }

        [When(@"Sasha updates created backpack item")]
        [Given(@"Sasha updates created backpack item")]
        public async Task WhenSashaUpdatesCreatedBackpackItem()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            await UpdateItem(sasha, itemId);
        }

        private async Task UpdateItem(ApiClient user, Guid itemId)
        {
            var request = new BackpackItemRequest
            {
                Amount = 22,
                CategoryType = BackpackCategoryType._1,
                ExpirationDate = DateTimeOffset.Now.AddDays(25),
                Name = $"A completely new name-{Guid.NewGuid()}"
            };
            var response = await user.UpdateBackpackItemAsync(itemId, request);

            _scenarioContext.Add("backpack-item-request", request);
            _scenarioContext.Add("backpack-item", response);
        }

        [Then(@"returned item has same properties")]
        public void ThenReturnedItemHasSameProperties()
        {
            var request = _scenarioContext.Get<BackpackItemRequest>("backpack-item-request");
            var updatedItem = _scenarioContext.Get<BackpackItemModel>("backpack-item");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            updatedItem.Id.ShouldBe(itemId);
            updatedItem.Name.ShouldBe(request.Name);
            updatedItem.Amount.ShouldBe(request.Amount);
            updatedItem.ExpirationDate.Date.ShouldBe(request.ExpirationDate.Date);
            updatedItem.CategoryType.ShouldBe(request.CategoryType);
        }

        [When(@"Grisha queries for backpack items")]
        public async Task WhenGrishaQueriesForBackpackItems()
        {
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            var backpackItems = await grisha.GetBackpackItemsAsync(backpackId);

            _scenarioContext.Add("backpack-items", backpackItems.ToImmutableArray());
        }

        [Then(@"backpack item list contains updated item")]
        public void ThenListContainsUpdatedItem()
        {
            var backpackItems = _scenarioContext.Get<IImmutableList<BackpackItemModel>>("backpack-items");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            var createdBackpackItem = _scenarioContext.Get<BackpackItemModel>("backpack-item");

            backpackItems.ShouldContain(x => x.Id == itemId
                                             && createdBackpackItem.Name == x.Name
                                             && createdBackpackItem.ExpirationDate.Date == x.ExpirationDate.Date
                                             && createdBackpackItem.CategoryType == x.CategoryType
                                             && createdBackpackItem.Amount == x.Amount);
        }

        [Given(@"Grisha creates an item")]
        public async Task GivenGrishaCreatesAnItem()
        {
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(grisha, backpackId);
        }

        [Given(@"Grisha updates created backpack item")]
        public async Task GivenGrishaUpdatesCreatedBackpackItem()
        {
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            await UpdateItem(grisha, itemId);
        }

        [When(@"Sasha queries for backpack items")]
        public async Task WhenSashaQueriesForBackpackItems()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            var backpackItems = await sasha.GetBackpackItemsAsync(backpackId);

            _scenarioContext.Add("backpack-items", backpackItems.ToImmutableArray());
        }

        [Given(@"Jora is a registered user")]
        public void GivenJoraIsARegisteredUser()
        {
            _scenarioContext["Jora"] = Jora;
        }

        [When(@"Jora updates created item")]
        public async Task WhenJoraUpdatesAnItem()
        {
            var jora = _scenarioContext.Get<ApiClient>("Jora");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            try
            {
                await UpdateItem(jora, itemId);
            }
            catch (Exception e)
            {
                _scenarioContext.Add("update-item-response", e);
            }
        }

        [Then(@"gets BadRequest in response")]
        public void ThenGetsBadRequestInResponse()
        {
            object updateItemResponse = _scenarioContext["update-item-response"];

            updateItemResponse.ShouldBeOfType<ApiException<ProblemDetails>>();
            (updateItemResponse as ApiException<ProblemDetails>).StatusCode.ShouldBe(400);
        }

        [Given(@"Ion is un-authenticated user")]
        public void GivenIonIsUn_AuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [When(@"Ion updates created item")]
        public async Task WhenIonUpdatesAnItem()
        {
            var ion = _scenarioContext.Get<ApiClient>("Ion");
            var itemId = _scenarioContext.Get<Guid>("backpack-item-id");

            try
            {
                await UpdateItem(ion, itemId);
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