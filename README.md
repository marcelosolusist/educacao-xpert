# **[educacao-xpert] - Plataforma de Educação Online com API RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **[educacao-xpert]**. Este projeto é uma entrega do MBA DevXpert Full Stack .NET e é referente ao módulo **Arquitetura, Modelagem e Qualidade de Software**.
O objetivo principal é desenvolver uma plataforma educacional online com múltiplos bounded contexts (BC), aplicando DDD, TDD, CQRS e padrões arquiteturais para gestão eficiente de conteúdos educacionais, alunos e processos financeiros.

### **Autor**
- **Marcelo Menezes**

## **2. Proposta do Projeto**

O projeto consiste em:

- **API RESTful:** Exposição dos recursos da plataforma de educação para integração com outras aplicações ou desenvolvimento de front-ends alternativos.
- **Autenticação e Autorização:** Implementação de controle de acesso com registro e autenticação de usuários.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através de ORM.

## **3. Tecnologias Utilizadas**

- **Linguagens de Programação:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** SQL Server e Sqlite
- **Autenticação e Autorização:**
  - ASP.NET Core Identity
  - JWT (JSON Web Token) para autenticação na API
- **Documentação da API:** Swagger

## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

- backend/src/
  - Api/ - API RESTful
  - Business/ - Serviços de negócios
  - Data/ - Modelos de Dados e Configuração do EF Core
- README.md - Arquivo de Documentação do Projeto
- FEEDBACK.md - Arquivo para Consolidação dos Feedbacks
- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **Registro de Usuários:** Permite incluir usuários para utilizar o sistema.
- **Autenticação e Autorização:** Autenticação e autorização dos usuários registrados.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

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
   - `cd backend/src/Api/`
   - `dotnet run`
   - Acesse a documentação da API em: http://localhost:5224/swagger/ 
   
4. **Usuários de teste:**
   - Na carga inicial são criados dois usuários para testes. Caso queira utilizá-los use os seguintes dados:
   Perfil Aluno:
	- Usuário: usuario@aluno.com
	- Senha: Teste@123
   Perfil Admin:
	- Usuário: usuario@admin.com
	- Senha: Teste@123
   
5. **Registrar novo usuário ou usar o usuário de teste para acessar o sistema:**
   - Para acessar o sistema use o usuário de teste ou crie um novo usuário usando a opção Registrar.

## **6. Instruções de Configuração**

- **JWT para API:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **7. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:
http://localhost:5224/swagger/ 

## **8. Avaliação**

- Este projeto é parte de um curso acadêmico e não aceita contribuições externas. 
- Para feedbacks ou dúvidas utilize o recurso de Issues
- O arquivo `FEEDBACK.md` é um resumo das avaliações do instrutor e deverá ser modificado apenas por ele.
