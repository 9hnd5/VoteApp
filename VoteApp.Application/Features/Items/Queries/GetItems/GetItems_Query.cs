using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application.Commons.ExtensionMethods;
using VoteApp.Application.Commons.Interfaces;

namespace VoteApp.Application.Features.Items.Queries.GetItems
{
    public class GetItems_Query : IRequest<IReadOnlyList<GetItems_VM>>
    {
        public FilterOperator<DateTime?> CreatedDate { get; set; } = new FilterOperator<DateTime?>();
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public class Handler : IRequestHandler<GetItems_Query, IReadOnlyList<GetItems_VM>>
        {
            private readonly IAppDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IAppDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<IReadOnlyList<GetItems_VM>> Handle(GetItems_Query request, CancellationToken cancellationToken)
            {
                var itemEntities = await _dbContext.Items
                    .AsNoTracking()
                    .ApplyFilter(x => x.CreatedDate, request.CreatedDate)
                    .Skip((request.PageIndex.Value - 1) * request.PageSize.Value)
                    .Take(request.PageSize.Value)
                    .ToListAsync();
                var result = _mapper.Map<IReadOnlyList<GetItems_VM>>(itemEntities);
                return result;
            }
        }
    }
}
