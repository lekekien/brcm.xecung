using System;
using System.Collections.Generic;
using System.Text;
using DVG.CRM.XeCung.DomainLayer.Aggregates.Users;
using FluentValidation;
namespace DVG.CRM.XeCung.DomainLayer.Aggregates
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.LoginInfo.UserName).NotEmpty().WithMessage("Username is not empty");
            RuleFor(x => x.LoginInfo.Password).NotEmpty().WithMessage("Password is not empty");
        }
    }
}
