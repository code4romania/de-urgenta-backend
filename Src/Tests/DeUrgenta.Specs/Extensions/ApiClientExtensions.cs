using System;
using System.Threading.Tasks;
using DeUrgenta.Specs.Drivers;

namespace DeUrgenta.Specs.Extensions
{
    public static class ApiClientExtensions
    {
        public static async Task<Guid> GetUserId(this ApiClient client)
        {
            var user = await client.GetUserDetailsAsync();

            return user.Id;
        }
    }
}
