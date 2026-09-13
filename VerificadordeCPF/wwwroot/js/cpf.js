function validarCpfFront(cpf) {
    const numeros = (cpf || "").replace(/\D/g, "");

    if (numeros.length !== 11) return false;
    if (/^(\d)\1{10}$/.test(numeros)) return false;

    const digitos = numeros.split("").map(Number);

    const pesos1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
    let soma = 0;
    for (let i = 0; i < 9; i++) soma += digitos[i] * pesos1[i];
    let resto = soma % 11;
    const primeiroDigito = resto < 2 ? 0 : 11 - resto;
    if (primeiroDigito !== digitos[9]) return false;

    const pesos2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
    soma = 0;
    for (let i = 0; i < 10; i++) soma += digitos[i] * pesos2[i];
    resto = soma % 11;
    const segundoDigito = resto < 2 ? 0 : 11 - resto;

    return segundoDigito === digitos[10];
}

function pegarDados() {
    return {
        nome: document.getElementById("nome").value,
        cpf: document.getElementById("cpf").value
    };
}

document.getElementById("btnFront").addEventListener("click", () => {
    const { cpf } = pegarDados();
    const valido = validarCpfFront(cpf);
    document.getElementById("resultadoFront").textContent =
        valido ? "cpf correto no front" : "cpf incorreto no front";
});

document.getElementById("btnBack").addEventListener("click", async () => {
    const dados = pegarDados();
    const el = document.getElementById("resultadoBack");
    el.textContent = "consultando o back-end...";

    try {
        const resposta = await fetch("/Home/ValidarBack", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dados)
        });
        const json = await resposta.json();
        el.textContent = json.mensagem;
    } catch (erro) {
        el.textContent = "erro ao consultar o back-end";
        console.error(erro);
    }
});