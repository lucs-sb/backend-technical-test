# 🚀 Teste Técnico - Desenvolvedor Backend

## 📌 Contexto

Você recebeu a tarefa de criar uma aplicação que permita:

- Pesquisar repositórios públicos no GitHub
- Marcar repositórios como favoritos
- Organizar os resultados conforme critérios de relevância

A aplicação já possui um esqueleto (backend + frontend), porém propositalmente simples e acoplado.  
O objetivo é aplicar melhorias estruturais, implementar os comportamentos necessários e organizar a solução com foco em boas práticas.

Você tem liberdade para propor a arquitetura, desde que atenda aos requisitos técnicos.

---

## 🎯 Objetivo do Desafio

Avaliar sua capacidade de:

- Escrever código limpo, modular e de fácil manutenção
- Aplicar boas práticas de arquitetura backend
- Projetar uma solução com foco em lógica de negócio
- Resolver problemas com autonomia técnica

---

## 🧩 Descrição do Projeto

Desenvolver uma aplicação web que permita:

- Buscar repositórios públicos do GitHub pelo nome (ou parte do nome)
- Marcar repositórios como favoritos durante a execução
- Exibir uma lista ordenada por relevância (com lógica definida por você)

---

## ⚙️ Funcionalidades Obrigatórias

### 🔍 1. Buscar repositórios públicos

- Permitir pesquisa por qualquer repositório público no GitHub
- Implementar chamada para API oficial:
    ``https://api.github.com``


---

### ⭐ 2. Favoritar repositórios

- Permitir favoritar/desfavoritar repositórios
- Os favoritos devem existir **apenas em memória**
- Não utilizar banco de dados

---

### 🔥 3. Listar por relevância

- Criar endpoint para retornar repositórios ordenados por relevância
- A lógica deve considerar:

  - `stargazers_count`
  - `forks_count`
  - `watchers_count`

- A lógica deve ser explicada no código

---

## 🧱 Requisitos Técnicos

### 🖥️ Backend

- ASP.NET Core (Web API)
- Aplicar boas práticas de arquitetura:

  - Separação por camadas:
    - Domain
    - Application
    - Infrastructure
    - API

- Uso de DTOs para entrada e saída
- Separação de regras de negócio dos controllers
- Implementação de testes unitários (especialmente para relevância)

---

### 🌐 Frontend

- Angular (apenas como apoio)
- Não será avaliado design

Foco em:

- Consumo correto da API
- Separação de responsabilidades
- Uso de tipagem (interfaces)
- Organização do código

---

## 📊 Critérios de Avaliação

| Critério                                      | Peso        |
|----------------------------------------------|------------|
| Organização e arquitetura                    | 🟢 Alto     |
| Clareza e manutenibilidade                   | 🟢 Alto     |
| Separação de responsabilidades               | 🟢 Alto     |
| Boas práticas (SOLID, DDD)                   | 🟡 Médio    |
| Lógica de relevância                         | 🟢 Alto     |
| Testes unitários                             | 🟡 Médio    |
| Documentação no código                       | 🟡 Médio    |
| Funcionalidades funcionando corretamente     | 🟢 Obrigatório |

---