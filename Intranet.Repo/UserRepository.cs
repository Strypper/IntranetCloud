﻿using Intranet.Contract;
using Intranet.Entities.Database;
using Intranet.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Intranet.Repo
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IntranetContext ic): base(ic){}

        public async Task<User> FindByUserIdWithoutCancellationToken(int id)
            => await FindAll(u => u.Id == id).FirstOrDefaultAsync();

        public async Task<User> FindByUserName(string userName, CancellationToken cancellationToken = default)
            => await FindAll(u => u.UserName == userName).FirstOrDefaultAsync(cancellationToken);
    }
}
