import { Candidate } from '../models/Candidate';
import { User } from '../models/User';

class CandidateService {
    async createCandidate(candidateData: any): Promise<Candidate> {
        const candidate = new Candidate(candidateData);
        await candidate.save();
        return candidate;
    }

    async getCandidateById(candidateId: string): Promise<Candidate | null> {
        return await Candidate.findById(candidateId).populate('user');
    }

    async getAllCandidates(): Promise<Candidate[]> {
        return await Candidate.find().populate('user');
    }

    async updateCandidate(candidateId: string, updateData: any): Promise<Candidate | null> {
        return await Candidate.findByIdAndUpdate(candidateId, updateData, { new: true });
    }

    async deleteCandidate(candidateId: string): Promise<Candidate | null> {
        return await Candidate.findByIdAndDelete(candidateId);
    }
}

export default new CandidateService();