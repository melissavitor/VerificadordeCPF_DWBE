# Validação de CPF — Arquitetura Cliente/Servidor

Projeto acadêmico (Desenvolvimento Web Back-End) que demonstra na prática
as diferenças entre validar um dado no **cliente**, no **servidor** e via
**serviço externo**, dentro de uma aplicação ASP.NET Core MVC.

## O que o projeto faz

Um formulário com Nome e CPF, e três botões independentes, cada um
representando uma camada diferente da arquitetura:

| Botão              | Onde a validação acontece                                   |
|---------------------|--------------------------------------------------------------|
| Valida CPF front    | JavaScript, direto no navegador (algoritmo módulo 11)         |
| Valida CPF back     | C#, no servidor, via requisição `fetch` (`POST /Home/ValidarBack`) |
| Valida CPF web      | Consulta a um serviço externo ([cpf-brasil.org](https://cpf-brasil.org)), feita pelo próprio servidor (`POST /Home/ValidarWeb`) |

O ponto central do exercício: o navegador **nunca** fala diretamente com o
serviço externo. Ele só conversa com o próprio back-end, que por sua vez
assume o papel de cliente de um terceiro — protegendo o token de API e
mostrando como um servidor pode compor múltiplos serviços.

## Tecnologias

- ASP.NET Core MVC (.NET 10)
- C# — validação de CPF (módulo 11) e integração HTTP com serviço externo
- JavaScript puro — validação no front e chamadas `fetch`
- Bootstrap — estilo (incluso no template padrão do `dotnet new mvc`)

## Como rodar

```bash
dotnet restore
dotnet build
dotnet run
```

Acesse a URL exibida no terminal (algo como `https://localhost:5001`).

## Configurando o botão "Valida CPF web"

Esse botão depende de uma conta gratuita no cpf-brasil.org:

1. Crie uma conta em `https://dash.cpf-brasil.org/token/registro/`.
2. Copie seu token no dashboard.
3. Cole em `appsettings.json`, substituindo `"SEU_TOKEN_AQUI"` pelo seu token.

Sem token configurado, o botão continua funcionando normalmente, retornando
`cpf incorreto na web` com um detalhe explicando que falta configuração.

## Estrutura do projeto

- `Models/CpfInputModel.cs` — dados do formulário (Nome, CPF)
- `Services/VerificadordeCPF.cs` — algoritmo de validação (C#)
- `Services/CpfBrasilApiClient.cs` — chamada ao serviço externo
- `Controllers/HomeController.cs` — actions `Index`, `ValidarBack`, `ValidarWeb`
- `Views/Home/Index.cshtml` — formulário e botões
- `wwwroot/js/cpf.js` — validação no front + chamadas `fetch` para back e web