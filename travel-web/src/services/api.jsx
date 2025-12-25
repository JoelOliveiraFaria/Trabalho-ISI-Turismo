/*
 * ===================================================================================
 * TRABALHO PRÁTICO: Integração de Sistemas de Informação (ISI)
 * -----------------------------------------------------------------------------------
 * Nome: Joel Alexandre Oliveira Faria
 * Número: a28001
 * Curso: Engenharia de Sistemas Informáticos
 * Ano Letivo: 2025/2026
 * -----------------------------------------------------------------------------------
 * Ficheiro: api.js
 * Descrição: Configuração central do cliente HTTP (Axios).
 * Define o URL base da API e configura intercetores para injetar o Token JWT.
 * ===================================================================================
 */

import axios from 'axios';

// URL do Backend alojado no Azure (App Service)
const api_url = 'https://travelapi-joel-final.azurewebsites.net';

/**
 * Instância do Axios configurada com o endereço base da API.
 * Será utilizada em toda a aplicação para fazer pedidos HTTP (GET, POST, etc).
 */
const api = axios.create({
    baseURL: api_url,
});

/**
 * Interceptor de Pedidos (Request Interceptor).
 * Executa antes de cada pedido ser enviado para o servidor.
 * Verifica se existe um token de autenticação no LocalStorage e, se existir,
 * adiciona-o ao cabeçalho 'Authorization' (Bearer Token).
 */
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    
    if(token) {
        // Standard JWT: Authorization: Bearer <token>
        config.headers.Authorization = `Bearer ${token}`;
    }
    
    return config;
});    

export default api;