# ✈️ Travel Planner - Sistema de Gestão de Viagens (Cloud Edition)

> **Trabalho Prático - Integração de Sistemas de Informação (ISI)** > **Licenciatura em Engenharia de Sistemas Informáticos** > **Ano Letivo:** 2025/2026  

---

## 👤 Identificação do Aluno
* **Nome:** Joel Alexandre Oliveira Faria
* **Número:** a28001

---

## 📋 Descrição do Projeto
O **Travel Planner** é uma aplicação web distribuída que permite aos utilizadores planear e gerir viagens. O sistema foi desenhado com uma arquitetura orientada a serviços, alojada na cloud (**Azure**), integrando diversas tecnologias e APIs externas.

A aplicação permite:
1.  **Registo e Autenticação:** Segurança via JWT.
2.  **Gestão de Viagens:** CRUD completo.
3.  **Integração Meteorológica (REST):** Previsão do tempo via *OpenWeatherMap*.
4.  **Integração de Calendário (REST):** Deteção de conflitos via *Google Calendar API*.
5.  **Integração de Seguros (SOAP):** Serviço "Legacy" para cálculo de orçamentos.
6.  **Notificações:** Envio de emails via SMTP.

---

## ☁️ Infraestrutura Cloud (Azure)

Este projeto não corre apenas localmente; toda a infraestrutura foi migrada para o **Microsoft Azure** para garantir disponibilidade e escalabilidade.

### Recursos Utilizados:
* **Azure App Service:** Alojamento da Web API (Backend) e dos Microserviços (Weather, Calendar, Email).
* **Azure SQL Database:** Base de dados relacional na cloud, substituindo o SQL Server local.
* **Application Insights:** Monitorização e logging de erros em tempo real.

---

## 🏗️ Arquitetura e Serviços

O sistema utiliza uma arquitetura de Microserviços comunicando via HTTP:

### 1. Core Services
* **TravelAPI:** O orquestrador central que comunica com o Frontend e a Base de Dados.

### 2. Integrações Externas (REST)
* **WeatherService:** Proxy para a API OpenWeatherMap.
* **CalendarService:** Conector para a Google Calendar API (Service Account).
* **EmailService:** Serviço de envio de notificações via Gmail SMTP.

### 3. Integração Enterprise (SOAP)
* **InsuranceService (WCF):** Web Service SOAP alojado no Azure, simulando um sistema antigo de seguradora para cálculo de custos.

---

## 🛠️ Tecnologias

* **Cloud:** Microsoft Azure (App Services, SQL DB).
* **Backend:** C# .NET 8, Entity Framework Core, WCF (SOAP).
* **Frontend:** React JS, Bootstrap 5, Axios.
* **Segurança:** JWT Bearer Authentication.
