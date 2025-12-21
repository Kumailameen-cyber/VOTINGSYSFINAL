export const validateVote = (req, res, next) => {
    const { voterId, candidateId } = req.body;
    if (!voterId || !candidateId) {
        return res.status(400).json({ message: "Voter ID and Candidate ID are required." });
    }
    next();
};

export const validateUserRegistration = (req, res, next) => {
    const { username, password, role } = req.body;
    if (!username || !password || !role) {
        return res.status(400).json({ message: "Username, password, and role are required." });
    }
    next();
};

export const validateUserLogin = (req, res, next) => {
    const { username, password } = req.body;
    if (!username || !password) {
        return res.status(400).json({ message: "Username and password are required." });
    }
    next();
};