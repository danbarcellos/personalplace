using FluentValidation;
using PersonalPlace.Domain.Entities.Security;
using PersonalPlace.Domain.Validation.Base;

namespace PersonalPlace.Domain.Validation.Security
{
    public class UserConstraint : Constraint<User>
    {
        //todo:mover Pattern e configurações de tamanho de senha para configurações Personal Place
        private const int TamanhoMinimoDeSenha = 8;
        private const int TamanhoMaximoDeSenha = 512;
        //private const string PatternSenha = @"^(?=.*[A-Z])(?=.*\d)(?!.*(.)\1\1)[a-zA-Z0-9@]{8,15}$";
        //private const string PatternEmail = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public UserConstraint()
        {
            RuleFor(x => x.ExternalId)
                .Length(0, 50)
                    .WithMessage("Id externo é maior que o máximo permitido");
            RuleFor(x => x.FirstName)
                .NotNull()
                    .WithMessage("Primeiro nome do usuário não informado")
                .Length(2, 60)
                    .WithMessage("Primeiro nome do usuário não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.LastName)
                .NotNull()
                    .WithMessage("Sobrenome do usuário não informado")
                .Length(2, 60)
                    .WithMessage("Sobrenome do usuário não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Password)
                .NotNull()
                    .WithMessage("Senha do usuário não informado")
                .Length(TamanhoMinimoDeSenha, TamanhoMaximoDeSenha)
                    .WithMessage("Senha do usuário não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.Email)
                .NotNull()
                   .WithMessage("E-mail do usuário não informado")
                .Length(8, 80)
                   .WithMessage("E-mail do usuário não contém o mínimo de caracteres ou é maior que o máximo permitido")
                .EmailAddress()
                   .WithMessage("E-mail do usuário inválido.");
            //.And.MatchWith(PatternSenha)
            //    .WithMessage("Senha do usuário não confere com os padrões de segurança do sistema. Deve conter numeros, letras maiúsculas e minúsculas.");
            RuleFor(x => x.PhotoFilePrefix)
                .Length(1, 80)
                   .WithMessage("Prefixo de imagem do usuário não contém o mínimo de caracteres ou é maior que o máximo permitido");
            RuleFor(x => x.State)
                .NotNull()
                   .WithMessage("Estado do usuário não informado");
        }
    }
}