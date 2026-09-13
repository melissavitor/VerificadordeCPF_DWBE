namespace VerificadordeCPF.Services;

public static class VerificadorCpf {
    public static bool Validar(string? cpf) {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var numeros = new string(cpf.Where(char.IsDigit).ToArray());

        if (numeros.Length != 11)
            return false;

        if (numeros.Distinct().Count() == 1)
            return false;

        var digitos = numeros.Select(c => c - '0').ToArray();

        int[] pesos1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] pesos2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += digitos[i] * pesos1[i];

        int resto = soma % 11;
        int primeiroDigito = resto < 2 ? 0 : 11 - resto;

        if (primeiroDigito != digitos[9])
            return false;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += digitos[i] * pesos2[i];

        resto = soma % 11;
        int segundoDigito = resto < 2 ? 0 : 11 - resto;

        return segundoDigito == digitos[10];
    }
}