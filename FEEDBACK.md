# Avaliação Técnica - Projeto Plataforma de Educação Online - educacao-xpert

## Organização do Projeto

**Pontos Positivos:**
- O projeto está organizado em pastas `Api`, `Business` e `Data`, com separação básica de responsabilidades.
- A identidade foi implementada com ASP.NET Core Identity, JWT e estrutura de autenticação funcional.
- A estrutura do projeto é funcional como um monolito.

**Pontos de Melhoria:**
- O projeto foi construído como um **monólito único**, **sem separação em Bounded Contexts**. Todos os domínios (conteúdo, alunos, pagamentos) estão centralizados na camada `Business`.
- A camada de aplicação está ausente. As controllers comunicam-se diretamente com repositórios, o que fere os princípios de DDD.
- Nenhuma outra controller além de `AutenticacaoController` está implementada.

---

## Modelagem de Domínio

**Pontos Positivos:**
- Entidades como `Curso` e `Pagamento` apresentam métodos encapsulados (`ObterProximaOrdemDeAula`, `AdicionarHistorico`) que mostram tentativa de implementar regras de negócio.
- O domínio possui entidades previstas no escopo como `Curso`, `Aula`, `Aluno`, `Matricula`, `Pagamento`, `Certificado`.

**Pontos de Melhoria:**
- A maioria das entidades está anêmica, como `Aluno`, que não encapsula nenhuma lógica de negócio.
- A modelagem está concentrada em uma única camada (`Business.Entities`), sem separação por contexto.
- Não há implementação de VOs como `ConteudoProgramatico`, `HistoricoAprendizado`, `DadosCartao`.
- Agregados estão mal definidos e não há nenhuma estratégia de Aggregate Root claramente aplicada.

---

## Casos de Uso e Regras de Negócio

**Pontos Positivos:**
- O fluxo de autenticação e registro de usuário está parcialmente funcional.

**Pontos de Melhoria:**
- O único fluxo implementado (registro de usuário/aluno) insere o `Aluno` diretamente via repositório dentro da controller, sem passar por um serviço de aplicação ou de domínio.
- Nenhum outro fluxo de negócio foi encontrado:
  - Cadastro de curso e aula
  - Matrícula
  - Pagamento
  - Progresso
  - Emissão de certificado

---

## Integração entre Contextos

**Pontos de Melhoria:**
- Não existem múltiplos contextos implementados. Todo o domínio está acoplado.
- Não há eventos de domínio ou qualquer mecanismo de integração assíncrona.

---

## Estratégias Técnicas Suportando DDD

**Pontos Positivos:**
- Uso parcial de repositórios nas interfaces da camada de `Business`.

**Pontos de Melhoria:**
- CQRS não foi implementado.
- TDD não foi adotado: não há nenhum teste de unidade ou integração.
- Não existe separação clara entre comandos, queries ou serviços de domínio.
- Persistência está acoplada ao modelo anêmico, sem controle de agregados.

---

## Autenticação e Identidade

**Pontos Positivos:**
- Implementação correta de autenticação com JWT e ASP.NET Core Identity.
- Usuário autenticado pode se registrar e gerar token.

**Pontos de Melhoria:**
- A criação do `Aluno` ocorre na controller via repositório, sem uso de serviços de domínio, violando princípios de encapsulamento.
- Não há modelagem explícita das personas de domínio (Aluno/Admin) com responsabilidades separadas.

---

## Execução e Testes

**Pontos de Melhoria:**
- Sem cobertura de testes ou estrutura de TDD.
- O Swagger está presente, mas não há endpoints além do de autenticação.

---

## Documentação

**Pontos Positivos:**
- O `README.md` apresenta a proposta, autor e tecnologias utilizadas.

**Pontos de Melhoria:**
- Não traz instruções de execução com SQLite nem exemplos de uso da API.

---

## Conclusão

O projeto está estruturado como um monólito sem separação de contextos, com entidades majoritariamente anêmicas e sem casos de uso reais implementados. A única funcionalidade presente é o registro de usuários, feita de forma incorreta. Faltam fluxos de negócio, encapsulamento adequado e testes. A abordagem não atende aos princípios de DDD propostos no desafio.

---

## Resumo dos Feedbacks do Ciclo Anterior

- ❌ **Não separou os contextos em BCs** → Confirmado, tudo centralizado em uma camada.
- ❌ **Entidades anêmicas** → Confirmado, maioria das entidades sem regras.
- ❌ **Fluxos ausentes** → Apenas autenticação, demais fluxos não implementados.
- ❌ **Registro de aluno via repositório direto na controller**
```
