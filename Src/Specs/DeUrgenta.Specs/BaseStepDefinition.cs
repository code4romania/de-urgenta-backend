using System;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;
using TechTalk.SpecFlow;

namespace DeUrgenta.Specs
{
    public abstract class BaseStepDefinition
    {
        private readonly ApiWebApplicationFactory _factory;
        private readonly Client _dummyClient;

        /// <summary>
        /// A registered user
        /// </summary>
        public Client Sasha { get; private set; }

        /// <summary>
        /// A registered user.
        /// </summary>
        public Client Grisha { get; private set; }

        /// <summary>
        /// Ill intended user
        /// A registered user.
        /// Use this user to try to access private data as an non-authorized user
        /// </summary>
        public Client Jora { get; private set; }

        /// <summary>
        /// Ill intended user.
        /// Use this user to access private data as an non-authenticated user.
        /// </summary>
        public Client Ion { get; private set; }

        public BaseStepDefinition(ApiWebApplicationFactory factory)
        {
            _factory = factory;
            _dummyClient = new Client(_factory.CreateClient());
        }

        [BeforeScenario]
        public async Task SetupClients()
        {
            Ion = new Client(_factory.CreateClient());

            Sasha = await RegisterClient("Sasha");
            Grisha = await RegisterClient("Grisha");
            Jora = await RegisterClient("Jora");
        }

        private async Task<Client> RegisterClient(string name)
        {
            var uniqueId = Guid.NewGuid();

            string email = $"{name}@{uniqueId}.com";

            await _dummyClient.RegisterAsync(new UserRegistrationDto()
            {
                Email = email,
                FirstName = name,
                LastName = $"last_{name}",
                Password = uniqueId.ToString()
            });

            var loginResponse = await _dummyClient.LoginAsync(new UserLoginRequest()
            {
                Email = email,
                Password = uniqueId.ToString()
            });

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {loginResponse.Token}");

            return new Client(client);
        }
    }
}