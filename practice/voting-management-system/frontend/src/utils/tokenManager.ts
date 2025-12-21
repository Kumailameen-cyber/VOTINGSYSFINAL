export const setToken = (token: string) => {
    localStorage.setItem('jwtToken', token);
};

export const getToken = (): string | null => {
    return localStorage.getItem('jwtToken');
};

export const removeToken = () => {
    localStorage.removeItem('jwtToken');
};

export const isTokenExpired = (token: string): boolean => {
    if (!token) return true;
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp * 1000 < Date.now();
};