export const generateToken = (user: any, secret: string, expiresIn: string) => {
    const payload = {
        id: user.id,
        username: user.username,
        role: user.role,
    };
    return jwt.sign(payload, secret, { expiresIn });
};

export const verifyToken = (token: string, secret: string) => {
    try {
        return jwt.verify(token, secret);
    } catch (error) {
        return null;
    }
};