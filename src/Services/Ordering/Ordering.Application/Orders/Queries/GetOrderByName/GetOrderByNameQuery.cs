using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Orders.Queries.GetOrderByName
{
    public record GetOrderByNameQuery(string Name) : IQuery<GetOrderByNameReponse>;

    public record GetOrderByNameReponse(IEnumerable<OrderDto> Orders);

    public class GetOrderByNameValidator : AbstractValidator<GetOrderByNameQuery>
    {
        public GetOrderByNameValidator() 
        {
            RuleFor(x=> x.Name).NotEmpty().WithMessage("Tên không được để trống");
        }
    }
}
