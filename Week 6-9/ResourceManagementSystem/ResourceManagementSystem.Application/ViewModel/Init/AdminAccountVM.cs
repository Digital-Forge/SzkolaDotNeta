using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResourceManagementSystem.Application.ViewModel.Init
{
    public class AdminAccountVM
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [Display(Name = "Repeat password")]
        public string Password2 { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }

    public class AdminAccountValidation : AbstractValidator<AdminAccountVM>
    {
        public AdminAccountValidation()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().NotEqual(y => y.Username);
            RuleFor(x => x.Password2).NotEmpty().Equal(y => y.Password);
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
            RuleFor(x => x.FullName).NotNull().NotEmpty();
        }
    }
}
