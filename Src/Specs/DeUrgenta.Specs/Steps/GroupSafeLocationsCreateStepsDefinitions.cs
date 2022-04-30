using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;
using DeUrgenta.Specs.Drivers;
using Microsoft.AspNetCore.Http;
using Shouldly;
using TechTalk.SpecFlow;
using System.Linq;

namespace DeUrgenta.Specs.Steps
{

    [Binding]
    [Scope(Feature = "Add Safe Location to Group")]
    public class GroupSafeLocationsCreateStepsDefinitions : BaseStepDefinition
    {
        private readonly ScenarioContext _scenarioContext;

        public GroupSafeLocationsCreateStepsDefinitions(
            ApiWebApplicationFactory factory,
            ScenarioContext scenarioContext) : base(factory)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"Sasha is an authenticated user")]
        public void GivenSashaIsAnAuthenticatedUser()
        {
            _scenarioContext["Sasha"] = Sasha;
        }

        [Given(@"Grisha is an authenticated user")]
        public void GivenGrishaIsAnAuthenticatedUser()
        {
            _scenarioContext["Grisha"] = Grisha;
        }

        [Given(@"Ion is un-authenticated user")]
        public void GivenIonIsUn_AuthenticatedUser()
        {
            _scenarioContext["Ion"] = Ion;
        }

        [Given(@"Sasha creates a group named ""(.*)""")]
        public async Task GivenHeCreatesAGroupNamed(string groupName)
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var group = await sasha.CreateNewGroupAsync(new GroupRequest()
            {
                Name = groupName
            });

            _scenarioContext["group"] = group;
            _scenarioContext["group-id"] = group.Id;
        }

        [Then(@"the returned Safe Location object has the same latitude, longitude, name and a non-empty id")]
        public void ThenTheReturnedSafeLocationObjectHasTheSameLatitudeLongitudeNameAndANon_EmptyId()
        {
            var request = _scenarioContext.Get<SafeLocationRequest>("safe-location-request");
            var response = _scenarioContext.Get<SafeLocationModel>("safe-location-response");

            response.Name.ShouldBe(request.Name);
            response.Latitude.ShouldBe(request.Latitude);
            response.Longitude.ShouldBe(request.Longitude);
            response.Id.ShouldNotBe(Guid.Empty);
        }

        [When(@"(.*) queries for group safe locations")]
        public async Task WhenGrishaQueriesForGroupSafeLocations(string userName)
        {
            var user = _scenarioContext.Get<Client>(userName);
            var groupId = _scenarioContext.Get<Guid>("group-id");
            var safeLocations = await user.GetGroupSafeLocationsAsync(groupId);

            _scenarioContext["safe-locations"] = safeLocations.ToImmutableArray();
        }

        [Then(@"list contains newly created safe location")]
        public void ThenListContainsNewlyCreatedSafeLocation()
        {
            var safeLocations = _scenarioContext.Get<ImmutableArray<SafeLocationModel>>("safe-locations");
            var safeLocation = _scenarioContext.Get<SafeLocationModel>("safe-location-response");

            safeLocations.First().Name.ShouldBe(safeLocation.Name);
            safeLocations.First().Name.ShouldBe(safeLocation.Name);
            safeLocations.First().Latitude.ShouldBe(safeLocation.Latitude);
            safeLocations.First().Longitude.ShouldBe(safeLocation.Longitude);
            safeLocations.First().Id.ShouldBe(safeLocation.Id);
        }


        [Given(@"Jora is an authenticated user")]
        public void GivenJoraIsAnAuthenticatedUser()
        {
            _scenarioContext["Jora"] = Jora;
        }

        [When(@"(.*) adds a Safe Location to `Sasha's group`")]
        public async Task WhenUserAddsASafeLocationToSashaSGroup(string userName)
        {
            var user = _scenarioContext.Get<Client>(userName);
            var groupId = _scenarioContext.Get<Guid>("group-id");

            try
            {

                var safeLocationRequest = new SafeLocationRequest
                {
                    Latitude = 66.55,
                    Longitude = 44.33,
                    Name = "A safe location"
                };
                var safeLocation = await user.CreateNewSafeLocationAsync(groupId, safeLocationRequest);

                _scenarioContext["safe-location-request"] = safeLocationRequest;
                _scenarioContext["safe-location-response"] = safeLocation;
            }
            catch (Exception e)
            {
                _scenarioContext.Add("safe-location-response-exception", e);
            }
        }

        [Then(@"Jora gets a BadRequest response")]
        [Then(@"Grisha gets a BadRequest response")]
        public void ThenUserGetsABadRequestResponse()
        {
            var apiException = _scenarioContext.Get<ApiException>("safe-location-response-exception");
            apiException.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Given(@"Grisha is a member of `Sasha's group`")]
        public async Task GivenGrishaIsAMemberOfSashaSGroup()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            await GroupDriver.AddGroupMember(groupId, sasha, grisha);
        }

        [Then(@"gets Unauthorized in response")]
        public void ThenGetsUnauthorizedInResponse()
        {
            var apiException = _scenarioContext.Get<ApiException>("safe-location-response-exception");
            apiException.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
        }
    }
}