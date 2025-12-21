import axios from 'axios';
import { getToken } from '../utils/tokenManager';

const axiosInstance = axios.create({
    baseURL: process.env.REACT_APP_API_URL, // Set your API URL here
});

// Request interceptor to add JWT token to headers
axiosInstance.interceptors.request.use(
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

// Response interceptor to handle errors globally
axiosInstance.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        // Handle specific error responses here
        if (error.response && error.response.status === 401) {
            // Handle unauthorized access (e.g., redirect to login)
        }
        return Promise.reject(error);
    }
);

export default axiosInstance;