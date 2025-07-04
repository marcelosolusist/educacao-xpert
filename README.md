# **[educacao-xpert] - Plataforma de Educação Online com API RESTful**

## **1. Apresentação**
Bem-vindo ao repositório do projeto **[educacao-xpert]**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Arquitetura, Modelagem e Qualidade de Software**.
O objetivo principal é desenvolver uma plataforma educacional online com múltiplos bounded contexts (BC), aplicando DDD, TDD, CQRS e padrões arquiteturais para gestão eficiente de conteúdos educacionais, alunos e processos financeiros.

### **Autor**
- **Marcelo Menezes**

## **2. Funcionalidades**
- Gestão de cursos, aulas, alunos, matrículas, pagamentos e certificados
- Autenticação e autorização com ASP.NET Core Identity e JWT
- API REST com documentação via Swagger
- Testes unitários e de integração com xUnit

## **3. Tecnologias Utilizadas**
- **Linguagens de Programação:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - Entity Framework Core
  - xUnit
- **Banco de Dados:** SQL Server e Sqlite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**
```
src/
├── EducacaoXpert.Api/
├── EducacaoXpert.Core/
├── EducacaoXpert.GestaoAlunos.Application/
├── EducacaoXpert.GestaoAlunos.Data/
├── EducacaoXpert.GestaoAlunos.Domain/
├── EducacaoXpert.GestaoConteudos.Application/
├── EducacaoXpert.GestaoConteudos.Data/
├── EducacaoXpert.GestaoConteudos.Domain/
├── EducacaoXpert.PagamentoFaturamento.Anticorruption/
├── EducacaoXpert.PagamentoFaturamento.Data/
└── EducacaoXpert.PagamentoFaturamento.Domain/

tests/
├── EducacaoXpert.Api.Tests/
├── EducacaoXpert.GestaoAlunos.Application.Tests/
├── EducacaoXpert.GestaoAlunos.Domain.Tests/
├── EducacaoXpert.GestaoConteudos.Application.Tests/
├── EducacaoXpert.GestaoConteudos.Domain.Tests/
└── EducacaoXpert.PagamentoFaturamento.Domain.Tests/

Outros:
├── .gitignore
├── FEEDBACK.md
└── README.md
```
## **5. Como Executar o Projeto**

### **Pré-requisitos**
- .NET SDK 8.0 ou superior
- SQL Server (se quiser rodar no modo production)
- Visual Studio 2022 ou superior (ou qualquer IDE de sua preferência)
- Git

### **Passos para Execução**

1. **Clone o Repositório:**
   - `git clone https://github.com/marcelosolusist/educacao-xpert.git`
   - `cd nome-do-repositorio`

2. **Configuração do Banco de Dados:**
   - Para rodar no modo Development não precisa configurar nada porque um banco de dados do Sqlite é criado automaticamente na primeira execução.
   - Se quiser rodar o projeto no modo Production é preciso configurar a string de conexão no arquivo `appsettings.json` do projeto Api.
   - Rode o projeto para que a configuração do Seed crie o banco e popule com os dados básicos.

3. **Executar a API:**
   - `cd EducacaoXpert\src\EducacaoXpert.Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:7139/swagger/ 
   
4. **Usuários de teste:**
   - Na carga inicial são criados dois usuários para testes. Caso queira utilizá-los use os seguintes dados:
   Perfil Aluno:
	- Usuário: usuario@aluno.com
	- Senha: Teste@123
   Perfil Admin:
	- Usuário: usuario@admin.com
	- Senha: Teste@123
	
5. **Executar os Testes:**
   - `cd EducacaoXpert`
   - `dotnet test EducacaoXpert.sln`
   - Os testes também podem ser executados diretamente pelo Visual Studio clicando com o botão direito na solution e escolhendo "Run Tests".
   
## **6. Instruções de Configuração**
- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **7. Documentação da API**
A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:
http://localhost:7139/swagger/ 
 
## **8. Avaliação**
- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
