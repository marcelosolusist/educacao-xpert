using FluentValidation;

namespace Business.Entities.Validations
{
    public class AulaValidation : AbstractValidator<Aula>
    {
        public AulaValidation()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido.");
            RuleFor(x => x.Conteudo)
                .NotEmpty().WithMessage("O campo {PropertyName} deve ser fornecido.");
        }
    }
}
