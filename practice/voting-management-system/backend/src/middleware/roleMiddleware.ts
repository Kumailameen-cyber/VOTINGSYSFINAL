export const roleMiddleware = (roles: string[]) => {
    return (req: any, res: any, next: any) => {
        const userRole = req.user.role; // Assuming req.user is set by authMiddleware

        if (!roles.includes(userRole)) {
            return res.status(403).json({ message: "Access denied." });
        }

        next();
    };
};