# RiskAnalysis - Documentação Completa ✅

## 📋 Índice
1. [Visão Geral](#visão-geral)
2. [Arquitetura e Camadas](#arquitetura-e-camadas)
3. [Dependências Instaladas](#dependências-instaladas)
4. [Como Usar a Aplicação](#como-usar-a-aplicação)
5. [Testando Endpoints](#testando-endpoints)

---

## Visão Geral 💡
Este repositório implementa um pequeno serviço para análise de risco de trades (operações financeiras). O objetivo principal é receber informações sobre trades, aplicar regras de negócio para classificá-los em categorias de risco e retornar o resultado. O projeto foi organizado em camadas para separar responsabilidade, facilitar testes e manter o código claro.

> Nota: Este README explica cada tópico existente no template original e dá instruções para executar, testar e entender a arquitetura do projeto.

---

## Arquitetura e Camadas 🔧
O código está dividido em projetos por responsabilidade (clean architecture / camadas simples):

- **RiskAnalisys.API (Apresentação / Controller)**
  - Responsável por expor os endpoints HTTP (API REST), configurar middlewares (ex.: tratamento global de exceções) e documentar a API (Swagger).
  - Entrada principal para requisições e ponto onde se faz o mapeamento entre DTOs e chamadas de serviço.

- **RiskAnalisys.Application (Negócio / Services)**
  - Contém a lógica de aplicação: serviços que implementam regras de negócio, DTOs de requisição/resposta e a injeção de dependências.
  - Ex.: `RiskAnalisysService` contém o fluxo para classificar o risco com base nas regras do domínio.

- **RiskAnalisys.Domain (Modelos / Entidades)**
  - Modelos e tipos do domínio (entities, enums, structs). É onde as regras puras de domínio e estruturas de dados são definidas.
  - Ex.: `Trade`, `ClientSector`, `RiskCategory`, `TradeMetrics`.

- **RiskAnalisys.Infrastructure (Persistência / Infra)**
  - Código relacionado à infraestrutura (por exemplo, persistência, adaptações). No projeto atual a pasta existe para manter separação e facilitar evolução futura.

---

## Dependências Instaladas 📦
Lista das principais bibliotecas usadas (conforme os arquivos `.csproj`):

- **RiskAnalisys.API**
  - `Swashbuckle.AspNetCore` — gera a documentação Swagger/OpenAPI da API.
  - `Newtonsoft.Json` — serialização JSON (configurações ou compatibilidade).
  - `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` — suporte para Docker/containers durante desenvolvimento.

- **RiskAnalisys.Application**
  - `Microsoft.AspNetCore.Mvc.Core` — abstrações usadas pela camada de aplicação.
  - `Microsoft.Extensions.DependencyInjection.Abstractions` e `Microsoft.Extensions.Logging.Abstractions` — injeção de dependência e logging.

> Observação: para ver versões exatas e mudar dependências, verifique os arquivos `*.csproj` em cada projeto.

---

## Como Usar a Aplicação 🚀
Siga estes passos para rodar localmente:

1. Pré-requisitos:
   - .NET SDK 8.0 instalado (ver `global.json` se houver) ✅
   - (Opcional) Docker, se quiser executar via container.

2. Build e execução local (modo rápido):

```bash
# Na raiz do repositório
cd RiskAnalisys.API
dotnet build
dotnet run
```

- A API normalmente sobe em `http://localhost:5000` e `https://localhost:5001` (ver `Properties/launchSettings.json` para configurações específicas da máquina).

3. Executando com Docker (exemplo básico):

```bash
# Usando o Dockerfile em RiskAnalisys.API
docker build -t riskanalysis:local -f RiskAnalisys.API/Dockerfile .
docker run -p 5000:80 --rm riskanalysis:local
```

4. Executando testes unitários:

```bash
dotnet test RiskAnalisys.Tests
```

---

## Testando Endpoints 🧪
A documentação interativa (Swagger) está disponível quando a aplicação estiver rodando:

- Acesse `https://localhost:5001/swagger` ou `http://localhost:5000/swagger` (dependendo do comportamento do Kestrel/HTTPs no ambiente).
- Use o Swagger UI para enviar requisições de teste e ver exemplos de payloads e respostas.

> Dica: o endpoint de classificação aceita um DTO com os dados do trade — ver os DTOs em `RiskAnalisys.Application/DTO/Requests` para o formato esperado.

---

## Observações Finais ✨
- Este README foca em explicar os tópicos já presentes no template e dar instruções práticas para rodar e testar o projeto.
- Se quiser, posso:
  - Adicionar exemplos de requisições para os endpoints no README ✅
  - Incluir badges (build, coverage) ou instruções de CI/CD
  - Documentar convenções de código e padrões de projeto usados

---

**Contato / Contribuição**
- Para contribuir: abra uma issue ou um pull request seguindo o padrão do repositório.
- Autor: repositório base do desafio (ver histórico do Git).
