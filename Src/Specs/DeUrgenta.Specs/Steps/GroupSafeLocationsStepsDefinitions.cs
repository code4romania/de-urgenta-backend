using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;
using DeUrgenta.Specs.Drivers;
using Microsoft.AspNetCore.Http;
using Shouldly;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs.Steps
{
    [Binding]
    [Scope(Feature = "Add Safe Location to Group")]
    public class GroupSafeLocationsStepsDefinitions : BaseStepDefinition
    {
        private readonly GroupDriver _groupDriver;
        private readonly ScenarioContext _scenarioContext;

        public GroupSafeLocationsStepsDefinitions(
            ApiWebApplicationFactory factory,
            GroupDriver groupDriver,
            ScenarioContext scenarioContext) : base(factory)
        {
            _groupDriver = groupDriver;
            _scenarioContext = scenarioContext;
        }

        [Given(@"Sasha is an authenticated user")]
        public void GivenSashaIsAnAuthenticatedUser()
        {
            _scenarioContext["Sasha"] = Sasha;
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

        [Given(@"Grisha is an authenticated user")]
        public void GivenGrishaIsAnAuthenticatedUser()
        {
            _scenarioContext["Grisha"] = Grisha;
        }


        [When(@"Sasha adds a Safe Location to `Sasha's group`")]
        public async Task WhenSashaAddsASafeLocationToSashaSGroup()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            var safeLocationRequest = new SafeLocationRequest
            {
                Latitude = 66.55,
                Longitude = 44.33,
                Name = "A safe location"
            };
            var safeLocation = await sasha.CreateNewSafeLocationAsync(groupId, safeLocationRequest);

            _scenarioContext["safe-location-request"] = safeLocationRequest;
            _scenarioContext["safe-location-response"] = safeLocation;
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

        [When(@"Grisha adds a Safe Location to `Sasha's group`")]
        public async Task WhenGrishaAddsASafeLocationToSashaSGroup()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            try
            {
                await grisha.CreateNewSafeLocationAsync(groupId,
                    new SafeLocationRequest
                    {
                        Latitude = 11,
                        Longitude = 12,
                        Name = "a safe location"
                    });
            }
            catch (Exception e)
            {
                _scenarioContext.Add("safe-location-response-exception", e);
            }
        }

        [Then(@"Grisha gets a BadRequest response")]
        public void ThenGrishaGetsABadRequestResponse()
        {
            var apiException = _scenarioContext.Get<ApiException>("safe-location-response-exception");
            apiException.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [Given(@"Sasha adds a Safe Location to `Sasha's group`")]
        public async Task GivenSashaAddsASafeLocationToSashaSGroup()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            var safeLocation = await sasha.CreateNewSafeLocationAsync(groupId,
                 new SafeLocationRequest
                 {
                     Latitude = 11,
                     Longitude = 12,
                     Name = "a safe location"
                 });

            _scenarioContext["safe-location-id"] = safeLocation.Id;
            _scenarioContext["safe-location-response"] = safeLocation;
        }

        [When(@"Sasha deletes a Safe Location from `Sasha's group`")]
        public async Task WhenSashaDeletesASafeLocationFromSashaSGroup()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var safeLocationId = _scenarioContext.Get<Guid>("safe-location-id");

            await sasha.DeleteSafeLocationAsync(safeLocationId);
        }

        [When(@"Sasha Queries for safe locations of `Sasha's group`")]
        public async Task WhenSashaQueriesForSafeLocationsOfSashaSGroup()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            var safeLocations = await sasha.GetGroupSafeLocationsAsync(groupId);
            _scenarioContext.Add("group-safe-locations", safeLocations.ToImmutableArray());
        }

        [Then(@"the list of Safe Locations in `Sasha's group` does not include the deleted Location")]
        public void ThenTheListOfSafeLocationsInDoesNotIncludeTheDeletedLocation()
        {
            var safeLocations = _scenarioContext.Get<IImmutableList<SafeLocationModel>>("group-safe-locations");
            var safeLocationId = _scenarioContext.Get<Guid>("safe-location-id");

            safeLocations.ShouldNotContain(x => x.Id == safeLocationId);
        }

        [When(@"Grisha deletes a Safe Location from `Sasha's group`")]
        public async Task WhenGrishaDeletesASafeLocationFromSashaSGroup()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var safeLocationId = _scenarioContext.Get<Guid>("safe-location-id");

            try
            {
                await grisha.DeleteSafeLocationAsync(safeLocationId);
            }
            catch (Exception e)
            {
                _scenarioContext.Add("safe-location-response-exception", e);
            }
        }

        [Given(@"Grisha is a member of `Sasha's group`")]
        public async Task GivenGrishaIsAMemberOfSashaSGroup()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            await _groupDriver.AddGroupMember(groupId, sasha, grisha);
        }

        [When(@"Grisha checks the Safe Locations in `Sasha's group`")]
        public async Task WhenGrishaChecksTheSafeLocationsInSashaSGroup()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var groupId = _scenarioContext.Get<Guid>("group-id");
            var safeLocations = await grisha.GetGroupSafeLocationsAsync(groupId);

            _scenarioContext.Add("group-safe-locations", safeLocations.ToImmutableArray());
        }

        [Then(@"Grisha gets a list of all the Safe Locations set by the admin")]
        public void ThenGrishaGetsAListOfAllTheSafeLocationsSetByTheAdmin()
        {
            var safeLocations = _scenarioContext.Get<IImmutableList<SafeLocationModel>>("group-safe-locations");
            var safeLocation = _scenarioContext.Get<SafeLocationModel>("safe-location-response");

            safeLocations.ShouldContain(x => x.Name == safeLocation.Name
                                           && x.Longitude == safeLocation.Longitude
                                           && x.Latitude == safeLocation.Latitude
                                           && x.Id == safeLocation.Id);
        }


        [Given(@"Jora is an authenticated user")]
        public void GivenJoraIsAnAuthenticatedUser()
        {
            _scenarioContext.Add("Jora", Jora);
        }


        [When(@"Jora checks the Safe Locations in `Sasha's group`")]
        public async Task WhenJoraChecksTheSafeLocationsInSashaSGroup()
        {
            var jora = _scenarioContext.Get<Client>("Jora");
            var groupId = _scenarioContext.Get<Guid>("group-id");

            try
            {
                await jora.GetGroupSafeLocationsAsync(groupId);
            }
            catch (Exception e)
            {
                _scenarioContext.Add("get-safe-locations-response", e);
            }
        }

        [Then(@"Jora gets a BadRequest response")]
        public void ThenJoraGetsABadRequestResponse()
        {
            var response = _scenarioContext.Get<ApiException>("get-safe-locations-response");

            response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        }

        [When(@"Sasha changes the location of a Safe Location in `Sasha's group`")]
        public async Task WhenSashaChangesTheLocationOfASafeLocationInSashaSGroup()
        {
            var sasha = _scenarioContext.Get<Client>("Sasha");
            var safeLocationId = _scenarioContext.Get<Guid>("safe-location-id");

            var safeLocationRequest = new SafeLocationRequest
            {
                Latitude = 123,
                Longitude = 321,
                Name = "A new location"
            };

            var safeLocation = await sasha.UpdateSafeLocationAsync(safeLocationId, safeLocationRequest);

            _scenarioContext["safe-location-request"] = safeLocationRequest;
            _scenarioContext["safe-location-response"] = safeLocation;
        }

        [When(@"Grisha changes the location of a Safe Location from `Sasha's group`")]
        public async Task WhenGrishaChangesTheLocationOfASafeLocationFromSashaSGroup()
        {
            var grisha = _scenarioContext.Get<Client>("Grisha");
            var safeLocationId = _scenarioContext.Get<Guid>("safe-location-id");

            try
            {

                await grisha.UpdateSafeLocationAsync(safeLocationId,
                    new SafeLocationRequest
                    {
                        Latitude = 555, 
                        Longitude = 78, 
                        Name = "weird name"
                    });
            }
            catch (Exception e)
            {
                _scenarioContext["safe-location-response-exception"] = e;
            }
        }
    }
}