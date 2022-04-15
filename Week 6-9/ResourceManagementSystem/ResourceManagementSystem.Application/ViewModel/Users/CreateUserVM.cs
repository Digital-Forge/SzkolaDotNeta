using FluentValidation;
using ResourceManagementSystem.Application.ViewModel.ExtraViewModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace ResourceManagementSystem.Application.ViewModel.Users
{
    public class CreateUserVM
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool SetActive { get; set; } = true;
        public bool SendEmail { get; set; } = false;

        public List<AddRemoveStatusVM> DepartmentsList { get; set; } = new List<AddRemoveStatusVM>();
        public List<AddRemoveStatusVM> RolesList { get; set; } = new List<AddRemoveStatusVM>();
    }

    public class CreateUserValidation : AbstractValidator<CreateUserVM>
    {
        public CreateUserValidation()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}
