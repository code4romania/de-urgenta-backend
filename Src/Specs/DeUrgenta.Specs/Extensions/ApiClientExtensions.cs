using System;
using System.Threading.Tasks;
using DeUrgenta.Specs.Clients;

namespace DeUrgenta.Specs.Extensions
{
    public static class ApiClientExtensions
    {
        public static async Task<Guid> GetUserId(this Client client)
        {
            var user = await client.GetUserDetailsAsync();

            return user.Id;
        }
    }
}
