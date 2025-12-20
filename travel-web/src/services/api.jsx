import axios from 'axios';

const api_url = 'https://localhost:7139/';

const api = axios.create({
    baseURL: api_url,
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if(token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});    

export default api;