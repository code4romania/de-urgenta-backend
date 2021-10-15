using System.Threading.Tasks;
using DeUrgenta.Domain.Api;
using DeUrgenta.Domain.Api.Entities;
using DeUrgenta.User.Api.Models.DTOs.Requests;

namespace DeUrgenta.User.Api.Services
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly DeUrgentaContext _context;

        public ApplicationUserManager(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task CreateApplicationUserAsync(UserRegistrationDto user, string userSub)
        {
            var newUser = new DeUrgenta.Domain.Api.Entities.User
            {
                FirstName =  user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Sub = userSub
            };

            var userBackpack = new Backpack
            {
                Name = "Ruxacul tau personal"
            };

            var userToBackpack = new BackpackToUser
            {
                Backpack = userBackpack, 
                User = newUser, 
                IsOwner = true
            };

            await _context.Backpacks.AddAsync(userBackpack);
            await _context.Users.AddAsync(newUser);
            await _context.BackpacksToUsers.AddAsync(userToBackpack);
            await _context.SaveChangesAsync();
        }
    }
}