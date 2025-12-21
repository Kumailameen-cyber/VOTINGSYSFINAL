export const validateEmail = (email: string): boolean => {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
};

export const validatePassword = (password: string): boolean => {
    return password.length >= 6;
};

export const validateUsername = (username: string): boolean => {
    const regex = /^[a-zA-Z0-9_]{3,30}$/;
    return regex.test(username);
};

export const validateVote = (candidateId: string): boolean => {
    return candidateId.length > 0;
};