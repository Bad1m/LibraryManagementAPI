using FluentValidation;
using LibraryManagementAPI.Services.Constants;
using LibraryManagementAPI.Services.Interfaces;
using LibraryManagementAPI.Services.Models;

namespace LibraryManagementAPI.Services.Validators
{
    public class UserValidator : AbstractValidator<UserDto>, IUserValidator
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage(UserErrors.UsernameRequired);
            RuleFor(user => user.FirstName).NotEmpty().WithMessage(UserErrors.FirstNameRequired);
            RuleFor(user => user.LastName).NotEmpty().WithMessage(UserErrors.LastNameRequired);
            RuleFor(user => user.Patronymic).NotEmpty().WithMessage(UserErrors.PatronymicRequired);
            RuleFor(user => user.Email).NotEmpty().WithMessage(UserErrors.EmailRequired)
                .EmailAddress().WithMessage(UserErrors.InvalidEmailFormat);
            RuleFor(user => user.Password).NotEmpty().WithMessage(UserErrors.PasswordRequired);
        }

        public void ValidateUser(UserDto userDto)
        {
            var validationResult = Validate(userDto);
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => error.ErrorMessage);
                throw new ValidationException(string.Join("\n", validationErrors));
            }
        }
    }
}
