using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Drivers;
using Shouldly;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Backpack item creation")]
    public class BackpackItemCreateStepsDefinitions : BaseApiStep
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly BackpackDriver _backpackDriver;

        public BackpackItemCreateStepsDefinitions(
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

        [Given(@"Grisha is authenticated user")]
        public void GivenGrishaIsAuthenticatedUser()
        {
            _scenarioContext["Grisha"] = Grisha;
        }

        [Given(@"Sasha creates a backpack")]
        public async Task GivenSashaCreatesABackpack()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var backpack = await sasha.CreateNewBackpackAsync(new BackpackModelRequest { Name = "El backpacko" });
            _scenarioContext.Add("backpackId", backpack.Id);
        }

        [Given(@"Grisha is a backpack contributor")]
        public async Task GivenGrishaIsABackpackContributor()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");


            await _backpackDriver.AddToBackpackContributor(sasha, grisha, backpackId);
        }

        [When(@"Sasha creates an item")]
        public async Task WhenSashaCreatesAnItem()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(sasha, backpackId, DateTime.Now.AddDays(350));
        }

        private async Task CreateBackpackItem(ApiClient user, Guid backpackId, DateTime? expirationDate)
        {
            var backpackItemRequest = new BackpackItemRequest
            {
                Amount = 3,
                CategoryType = BackpackCategoryType._5,
                // TODO: fix 
                ExpirationDate = expirationDate ?? DateTime.Now.AddDays(350),
                Name = "A thing"
            };

            var backpackItemResponse = await user.CreateNewBackpackItemAsync(backpackId, backpackItemRequest);

            _scenarioContext.Add("backpack-item-request", backpackItemRequest);
            _scenarioContext.Add("backpack-item", backpackItemResponse);
        }

        [Then(@"returned item has same properties")]
        public void ThenReturnedItemHasSameProperties()
        {
            var request = _scenarioContext.Get<BackpackItemRequest>("backpack-item-request");
            var response = _scenarioContext.Get<BackpackItemModel>("backpack-item");

            response.Name.ShouldBe(request.Name);
            response.Amount.ShouldBe(request.Amount);
            response.ExpirationDate.Date.ShouldBe(request.ExpirationDate.Date);
            response.CategoryType.ShouldBe(request.CategoryType);
        }

        [Then(@"it does not have an empty id")]
        public void ThenItDoesNotHaveAnEmptyId()
        {
            var response = _scenarioContext.Get<BackpackItemModel>("backpack-item");
            response.Id.ShouldNotBe(Guid.Empty);
        }

        [When(@"Grisha queries for backpack items")]
        public async Task WhenGrishaQueriesForBackpackItems()
        {
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            var backpackItems = await grisha.GetBackpackItemsAsync(backpackId);

            _scenarioContext.Add("backpack-items", backpackItems.ToImmutableArray());
        }

        [Then(@"list contains newly created item")]
        public void ThenListContainsNewlyCreatedItem()
        {
            var backpackItems = _scenarioContext.Get<IImmutableList<BackpackItemModel>>("backpack-items");
            var createdBackpackItem = _scenarioContext.Get<BackpackItemModel>("backpack-item");

            backpackItems.ShouldContain(x => createdBackpackItem.Name == x.Name
                                           && createdBackpackItem.ExpirationDate.Date == x.ExpirationDate.Date
                                           && createdBackpackItem.CategoryType == x.CategoryType
                                           && createdBackpackItem.Amount == x.Amount);
        }

        [When(@"Grisha creates an item")]
        public async Task WhenGrishaCreatesAnItem()
        {
            var grisha = _scenarioContext.Get<ApiClient>("Grisha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(grisha, backpackId, DateTime.Now.AddDays(350));
        }

        [When(@"Sasha queries for backpack items")]
        public async Task WhenSashaQueriesForBackpackItems()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            var backpackItems = await sasha.GetBackpackItemsAsync(backpackId);

            _scenarioContext.Add("backpack-items", backpackItems.ToImmutableArray());
        }

        [When(@"Sasha creates an item with indefinite expiration date")]
        public async Task WhenSashaCreatesAnItemWithIndefiniteExpirationDate()
        {
            var sasha = _scenarioContext.Get<ApiClient>("Sasha");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            await CreateBackpackItem(sasha, backpackId, null);

        }

        [Given(@"Jora is a registered user")]
        public void GivenJoraIsARegisteredUser()
        {
            _scenarioContext["Jora"] = Jora;
        }

        [When(@"Jora creates an item")]
        public async Task WhenJoraCreatesAnItem()
        {
            var jora = _scenarioContext.Get<ApiClient>("Jora");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            try
            {
                await CreateBackpackItem(jora, backpackId, DateTime.Now.AddDays(350));
            }
            catch (Exception e)
            {
                _scenarioContext.Add("create-item-response", e);
            }
        }

        [Then(@"gets BadRequest in response")]
        public void ThenGetsBadRequestInResponse()
        {
            object createItemResponse = _scenarioContext["create-item-response"];

            createItemResponse.ShouldBeOfType<ApiException<ProblemDetails>>();
            (createItemResponse as ApiException<ProblemDetails>).StatusCode.ShouldBe(400);
        }

        [Given(@"Ion is un-authenticated user")]
        public void GivenIonIsUn_AuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [When(@"Ion creates an item")]
        public async Task WhenIonCreatesAnItem()
        {
            var ion = _scenarioContext.Get<ApiClient>("Ion");
            var backpackId = _scenarioContext.Get<Guid>("backpackId");

            try
            {
                await CreateBackpackItem(ion, backpackId, DateTime.Now.AddDays(350));
            }
            catch (Exception e)
            {
                _scenarioContext.Add("create-item-response", e);
            }
        }

        [Then(@"gets Unauthorized in response")]
        public void ThenGetsUnauthorizedInResponse()
        {
            object createItemResponse = _scenarioContext["create-item-response"];

            createItemResponse.ShouldBeOfType<ApiException>();
            (createItemResponse as ApiException).StatusCode.ShouldBe(401);
        }
    }
}