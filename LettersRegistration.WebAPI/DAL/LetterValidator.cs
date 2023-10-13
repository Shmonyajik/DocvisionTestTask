using FluentValidation;
using LettersRegistration.WebAPI.Domain;
using System;

namespace LettersRegistration.WebAPI.DAL
{
    public class LetterValidator: AbstractValidator<Letter>
    {
        public LetterValidator()
        {
            RuleFor(r=>r.Name).NotNull().MaximumLength(20);
            RuleFor(r => r.RegistrationTime).NotNull();
            RuleFor(r => r.RegistrationTime)
            .LessThanOrEqualTo(DateTime.Now)  // Поле должно быть не больше текущего времени
            .WithMessage("Время регистрации не может быть в будущем");

            RuleFor(r => r.RegistrationTime)
                .GreaterThan(new DateTime(2023, 1, 1))  // 
                .WithMessage("Время регистрации не может быть до 01.01.2023");
            RuleFor(r=>r.Sender).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(r => r.Addressee).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(r => r.Content).NotNull().MaximumLength(200);
        }
    }
}
