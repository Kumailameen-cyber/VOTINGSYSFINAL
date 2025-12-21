import React, { createContext, useContext, useState, useEffect } from 'react';
import { getToken, setToken, removeToken } from '../utils/tokenManager';
import { loginUser, registerUser } from '../services/authService';

interface AuthContextType {
  user: any;
  login: (username: string, password: string) => Promise<void>;
  register: (username: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [user, setUser] = useState<any>(null);

  useEffect(() => {
    const token = getToken();
    if (token) {
      // Decode token to get user info (you can use a library like jwt-decode)
      const userData = JSON.parse(atob(token.split('.')[1]));
      setUser(userData);
    }
  }, []);

  const login = async (username: string, password: string) => {
    const { token } = await loginUser(username, password);
    setToken(token);
    const userData = JSON.parse(atob(token.split('.')[1]));
    setUser(userData);
  };

  const register = async (username: string, password: string) => {
    const { token } = await registerUser(username, password);
    setToken(token);
    const userData = JSON.parse(atob(token.split('.')[1]));
    setUser(userData);
  };

  const logout = () => {
    removeToken();
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};