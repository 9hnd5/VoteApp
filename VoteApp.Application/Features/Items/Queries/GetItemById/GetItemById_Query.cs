using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application.Commons.Interfaces;

namespace VoteApp.Application.Features.Items.Queries.GetItemById
{
    public class GetItemById_Query : IRequest<GetItemById_VM>
    {
        public int? Id { get; set; }
        public class Handler : IRequestHandler<GetItemById_Query, GetItemById_VM>
        {
            private readonly IAppDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IMapper mapper, IAppDbContext dbContext)
            {
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<GetItemById_VM> Handle(GetItemById_Query request, CancellationToken cancellationToken)
            {
                var itemEntity = await _dbContext.Items
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
                var result = _mapper.Map<GetItemById_VM>(itemEntity);
                return result;
            }
        }
    }
}
