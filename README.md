# 🧪 Testes Automatizados  Minhas Finanças

## 📌 Objetivo

Este repositório contém a implementação da estratégia de testes automatizados para o sistema **Minhas Finanças**, com foco na validação das regras de negócio existentes sem alteração do código original da aplicação.

---

## 🧱 Estrutura do Projeto

A organização segue o conceito de **pirâmide de testes**, separando responsabilidades por tipo:

```
backend/
├── unit/
│   ├── TransacaoServiceTests.cs
│   ├── PessoaServiceTests.cs
│   └── CategoriaServiceTests.cs
│
├── integration/
│   ├── PessoaIntegrationTests.cs
│   └── TransacaoIntegrationTests.cs

frontend/
├── unit/
│   └── transacao.test.ts
│
└── e2e/
    └── fluxo-transacao.spec.ts

```

---

## 🧪 Estratégia de Testes

Foi adotada a abordagem da **pirâmide de testes**:

### 🔹 Testes Unitários

* Focados na camada de **Application (Services)**
* Validam regras de negócio isoladamente
* Uso de mocks para dependências (`IUnitOfWork`)

### 🔹 Testes de Integração

* Validam fluxo completo da API
* Testam persistência e comportamento real
* Utilizam `WebApplicationFactory`

### 🔹 Testes End-to-End (E2E)

* Simulam o comportamento do usuário final
* Validam fluxos completos via interface
* Implementados com **Playwright**

### 🔹 Testes de Frontend (Unit)

* Validam regras básicas no client
* Utilizam **Vitest**

---

## ▶️ Como executar os testes

### 🔧 Backend

```bash
dotnet test
```

---

### 🌐 Frontend

Instalar dependências:

```bash
npm install
```

Rodar testes unitários:

```bash
npm run test
```

Rodar testes E2E:

```bash
npx playwright test
```

---

## 🐞 Bugs Identificados

Durante a análise e criação dos testes, foram identificadas falhas relevantes:

### ❌ BUG 001 — Menor de idade pode ter receita

* Regra esperada: menores não podem possuir receitas
* Comportamento atual: permitido pelo sistema
* Impacto: violação de regra de negócio crítica

---

### ❌ BUG 002 — Categoria não respeita finalidade

* Regra esperada: categorias devem ser usadas conforme tipo (receita/despesa)
* Comportamento atual: não há validação
* Impacto: inconsistência de dados

---

## 🧠 Justificativa das Escolhas

* Prioridade foi dada às **regras de negócio críticas**
* Testes unitários concentram maior volume (base da pirâmide)
* Testes de integração validam comportamento real da API
* Testes E2E validam experiência do usuário
* Bugs foram documentados com base em testes falhos

---

## 🎯 Foco da Implementação

* Não foi objetivo atingir 100% de cobertura
* Foco em:

  * Validação de regras de negócio
  * Identificação de falhas
  * Clareza e organização dos testes

---

## 🚀 Considerações Finais

A estratégia adotada permite:

* Detectar falhas críticas de negócio
* Garantir confiabilidade das funcionalidades principais
* Facilitar manutenção e evolução do sistema

---

## 📎 Observações

* O código da aplicação original **não foi alterado**
* Este repositório contém **exclusivamente testes**
* Dependências externas são simuladas quando necessário
