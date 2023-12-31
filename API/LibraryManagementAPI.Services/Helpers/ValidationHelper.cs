﻿using FluentValidation;
using FluentValidation.Results;

namespace LibraryManagementAPI.Services.Helpers
{
    public class ValidationHelper
    {
        public static void ValidateAndThrow(ValidationResult validationResult)
        {
            if (!validationResult.IsValid)
            {
                var validationErrors = validationResult.Errors.Select(error => new ValidationException(error.ErrorMessage));
                foreach (var error in validationErrors)
                {
                    throw error;
                }
            }
        }
    }
}
