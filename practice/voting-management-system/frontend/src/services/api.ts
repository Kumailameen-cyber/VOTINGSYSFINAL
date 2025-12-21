import axios from 'axios';
import { getToken } from '../utils/tokenManager';

const api = axios.create({
  baseURL: 'http://localhost:5000/api', // Adjust the base URL as needed
});

// Request interceptor to add JWT token to headers
api.interceptors.request.use(
  (config) => {
    const token = getToken();
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for handling errors
api.interceptors.response.use(
  (response) => response,
  (error) => {
    // Handle errors globally
    return Promise.reject(error);
  }
);

export default api;