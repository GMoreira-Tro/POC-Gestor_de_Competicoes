export function validarCpfCnpj(valor: string): boolean {
    valor = valor.replace(/\D/g, ''); // Remove caracteres não numéricos
  
    if (valor.length === 11) {
      return validarCPF(valor);
    } else if (valor.length === 14) {
      return validarCNPJ(valor);
    }
    return false;
  }
  
  function validarCPF(cpf: string): boolean {
    if (/^(\d)\1+$/.test(cpf)) return false; // Elimina CPFs com números repetidos
  
    let soma = 0;
    for (let i = 0; i < 9; i++) soma += parseInt(cpf.charAt(i)) * (10 - i);
    let resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
    if (resto !== parseInt(cpf.charAt(9))) return false;
  
    soma = 0;
    for (let i = 0; i < 10; i++) soma += parseInt(cpf.charAt(i)) * (11 - i);
    resto = (soma * 10) % 11;
    if (resto === 10 || resto === 11) resto = 0;
  
    return resto === parseInt(cpf.charAt(10));
  }
  
  function validarCNPJ(cnpj: string): boolean {
    if (/^(\d)\1+$/.test(cnpj)) return false; // Elimina CNPJs com números repetidos
  
    const pesosPrimeiroDigito = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    const pesosSegundoDigito = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
  
    let soma = cnpj
      .slice(0, 12)
      .split('')
      .reduce((acc, val, i) => acc + parseInt(val) * pesosPrimeiroDigito[i], 0);
  
    let resto = soma % 11;
    let primeiroDigito = resto < 2 ? 0 : 11 - resto;
  
    if (parseInt(cnpj.charAt(12)) !== primeiroDigito) return false;
  
    soma = cnpj
      .slice(0, 13)
      .split('')
      .reduce((acc, val, i) => acc + parseInt(val) * pesosSegundoDigito[i], 0);
  
    resto = soma % 11;
    let segundoDigito = resto < 2 ? 0 : 11 - resto;
  
    return parseInt(cnpj.charAt(13)) === segundoDigito;
  }  