using StageUp.Models.POCO;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace StageUp.Services
{
    public static class ValidaFormato
    {
        public static bool Email(string email) 
        {
            if (!new EmailAddressAttribute().IsValid(email))
                return false;
            return true;
        }
        public static bool Senha(string senha)
        {
            Regex especialRegex = new Regex("[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]");
            if (!especialRegex.IsMatch(senha))
                return false;

            // Verifica se a senha contém pelo menos uma letra minúscula
            Regex minusculaRegex = new Regex("[a-z]");
            if (!minusculaRegex.IsMatch(senha))
                return false;

            // Verifica se a senha contém pelo menos uma letra maiúscula
            Regex maiusculaRegex = new Regex("[A-Z]");
            if (!maiusculaRegex.IsMatch(senha))
                return false;

            // Verifica se a senha contém pelo menos um número
            Regex numeroRegex = new Regex("[0-9]");
            if (!numeroRegex.IsMatch(senha))
                return false;

            // Se a senha passar por todas as verificações, retorna true
            return true;
        }


    }
}
