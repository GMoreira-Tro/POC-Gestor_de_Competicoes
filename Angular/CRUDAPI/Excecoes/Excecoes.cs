using System;

public class EmailJaCadastradoException : Exception
{
    public EmailJaCadastradoException() : base("Email já cadastrado.") { }
}

public class CpfCnpjJaCadastradoException : Exception
{
    public CpfCnpjJaCadastradoException() : base("CPF ou CNPJ já cadastrado.") { }
}

public class CpfCnpjInvalidoException : Exception
{
    public CpfCnpjInvalidoException() : base("CPF ou CNPJ inválido.") { }
}

public class CampoObrigatorioException : Exception
{
    public CampoObrigatorioException(string campo) : base($"O campo '{campo}' é obrigatório.") { }
}

public class EstadoNaoPertenceAoPaisException : Exception
{
    public EstadoNaoPertenceAoPaisException() : base("Estado não pertence ao país") { }
}

public class CidadeNaoPertenceAoEstadoException : Exception
{
    public CidadeNaoPertenceAoEstadoException() : base("Cidade não pertence ao estado") { }
}
