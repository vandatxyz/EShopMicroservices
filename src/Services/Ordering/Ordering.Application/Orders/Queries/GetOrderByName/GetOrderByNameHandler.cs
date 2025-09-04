

namespace Ordering.Application.Orders.Queries.GetOrderByName;

public class GetOrderByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrderByNameQuery, GetOrderByNameReponse>
{
    public async Task<GetOrderByNameReponse> Handle(GetOrderByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders.Include(o=>o.OrderItems).AsNoTracking().Where(o=> o.OrderName.Value.Contains(query.Name))
            .OrderBy(o=>o. CreatedAt).ToListAsync(cancellationToken);

        return new GetOrderByNameReponse(orders.ToOrderDtoList());

    }


}
