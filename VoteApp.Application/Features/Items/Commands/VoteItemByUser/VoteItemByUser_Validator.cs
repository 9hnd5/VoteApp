using FluentValidation;

namespace VoteApp.Application.Features.Items.Commands.VoteItemByUser
{
    public class VoteItemByUser_Validator : AbstractValidator<VoteItemByUser_Command>
    {
        public VoteItemByUser_Validator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            RuleFor(x => x.Vote).LessThanOrEqualTo(1).WithMessage("Vote is less than or equal 1");
        }
    }
}
