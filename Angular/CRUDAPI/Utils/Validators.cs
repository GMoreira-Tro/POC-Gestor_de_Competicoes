using System.Text.RegularExpressions;

public static partial class Validators
{
    // Propriedade estática para armazenar a expressão regular
    private static readonly Regex senhaRegex = SenhaRegex();
    public static bool IsValidEmail(string email)
    {
        // Expressão regular para validar email
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        // Verifica se o email corresponde ao padrão da expressão regular
        return Regex.IsMatch(email, pattern);
    }

    public static bool ValidarSenha(string senha)
    {
        // Verificar o comprimento mínimo da senha
        if (senha.Length < 8)
        {
            throw new SenhaDeveConterNoMinimo8CaracteresException();
        }

        // Verificar se a senha contém letras maiúsculas, minúsculas, números e caracteres especiais
        if (!senhaRegex.IsMatch(senha))
        {
            throw new SenhaDeveConterRegexMatchException();
        }

        return true;
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%&*!?\-_()])[A-Za-z\d@#$%&*!?\-_()]+$")]
    private static partial Regex SenhaRegex();
}
