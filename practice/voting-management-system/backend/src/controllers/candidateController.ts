export class CandidateController {
    async getCandidateProfile(req, res) {
        const candidateId = req.params.id;
        try {
            const candidate = await Candidate.findById(candidateId);
            if (!candidate) {
                return res.status(404).json({ message: 'Candidate not found' });
            }
            res.status(200).json(candidate);
        } catch (error) {
            res.status(500).json({ message: 'Server error', error });
        }
    }

    async getCandidateDashboard(req, res) {
        const candidateId = req.user.id; // Assuming user ID is stored in JWT
        try {
            const candidate = await Candidate.findById(candidateId);
            if (!candidate) {
                return res.status(404).json({ message: 'Candidate not found' });
            }
            // Fetch additional data for the dashboard if needed
            res.status(200).json({ candidate });
        } catch (error) {
            res.status(500).json({ message: 'Server error', error });
        }
    }

    async updateCandidateProfile(req, res) {
        const candidateId = req.user.id; // Assuming user ID is stored in JWT
        const updates = req.body;
        try {
            const candidate = await Candidate.findByIdAndUpdate(candidateId, updates, { new: true });
            if (!candidate) {
                return res.status(404).json({ message: 'Candidate not found' });
            }
            res.status(200).json(candidate);
        } catch (error) {
            res.status(500).json({ message: 'Server error', error });
        }
    }
}