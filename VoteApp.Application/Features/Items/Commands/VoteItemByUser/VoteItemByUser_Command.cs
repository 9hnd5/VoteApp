using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application.Commons.Exceptions;
using VoteApp.Application.Commons.Interfaces;
using VoteApp.Domain.Entities;

namespace VoteApp.Application.Features.Items.Commands.VoteItemByUser
{
    public class VoteItemByUser_Command : IRequest
    {
        public int? Id { get; set; }
        public int? Vote { get; set; }
        [JsonIgnore]
        public int? UserId { get; set; }
        public class Handler : IRequestHandler<VoteItemByUser_Command, Unit>
        {
            private readonly IAppDbContext _dbContext;

            public Handler(IAppDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(VoteItemByUser_Command request, CancellationToken cancellationToken)
            {
                var userEntity = await _dbContext.Users
                    .Include(x => x.UserVoteDetails.Where(y => y.ItemId == request.Id).Take(1))
                    .FirstOrDefaultAsync(x => x.Id == request.UserId);
                if (userEntity == null) throw new NotFoundException("User", request.UserId);

                var itemEntity = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (itemEntity == null) throw new NotFoundException("Item", request.Id);
                itemEntity.Vote += request.Vote.Value;

                if (userEntity.UserVoteDetails.Count == 0)
                {
                    var userVoteDetailEntity = new UserVoteDetail()
                    {
                        ItemId = request.Id.Value,
                        UserId = request.UserId.Value,
                        TotalVote = request.Vote.Value
                    };
                    userEntity.UserVoteDetails.Add(userVoteDetailEntity);
                }
                else
                {
                    var userVoteDetailEntity = userEntity.UserVoteDetails.FirstOrDefault();
                    if (userVoteDetailEntity.TotalVote == 3) throw new BadRequestException("Maximum 3 vote/item");
                    userVoteDetailEntity.TotalVote += request.Vote.Value;
                }

                _dbContext.SaveChanges();
                return Unit.Value;
            }
        }
    }
}
