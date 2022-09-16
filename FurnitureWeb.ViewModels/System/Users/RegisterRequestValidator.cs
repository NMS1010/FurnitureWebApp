using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FurnitureWeb.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x).Custom((req, ctx) =>
            {
                if (req.Password != req.ConfirmPassword)
                {
                    ctx.AddFailure("Confirm password does not match");
                }
            });
        }
    }
}