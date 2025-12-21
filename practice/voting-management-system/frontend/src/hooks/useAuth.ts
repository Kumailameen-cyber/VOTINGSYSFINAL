import { useState, useEffect, useContext } from 'react';
import { AuthContext } from '../context/AuthContext';
import { login as loginService, register as registerService } from '../services/authService';
import { useHistory } from 'react-router-dom';

const useAuth = () => {
    const { setAuthData } = useContext(AuthContext);
    const [error, setError] = useState(null);
    const history = useHistory();

    const login = async (username, password) => {
        try {
            const response = await loginService(username, password);
            setAuthData(response.data);
            history.push('/voter/dashboard'); // Redirect to voter dashboard or candidate dashboard based on role
        } catch (err) {
            setError(err.response.data.message || 'Login failed');
        }
    };

    const register = async (userData) => {
        try {
            await registerService(userData);
            history.push('/login');
        } catch (err) {
            setError(err.response.data.message || 'Registration failed');
        }
    };

    const logout = () => {
        setAuthData(null);
        localStorage.removeItem('token'); // Clear token from local storage
        history.push('/login');
    };

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token) {
            setAuthData({ token }); // Set token in context if it exists
        }
    }, [setAuthData]);

    return { login, register, logout, error };
};

export default useAuth;