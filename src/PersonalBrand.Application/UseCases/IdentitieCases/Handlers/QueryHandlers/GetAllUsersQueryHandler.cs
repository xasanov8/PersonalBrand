using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PersonalBrand.Application.UseCases.IdentitieCases.Queries;
using PersonalBrand.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBrand.Application.UseCases.IdentitieCases.Handlers.QueryHandlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserModel>>
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IMemoryCache _memoryCache;

        public GetAllUsersQueryHandler(UserManager<UserModel> userManager, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;
        }

        public async Task<IEnumerable<UserModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var cacheData = _memoryCache.Get("allusers");
            if (cacheData == null)
            {
                var users =  _userManager.Users.ToImmutableList();
                _memoryCache.Set("allusers", users, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                });
                return users;
            }
            return cacheData as IEnumerable<UserModel>;
        }
    }
}
