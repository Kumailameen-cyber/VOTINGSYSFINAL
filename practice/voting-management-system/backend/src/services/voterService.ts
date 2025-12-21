import { Voter } from '../models/Voter';
import { Vote } from '../models/Vote';

export class VoterService {
    async getVoterDashboard(voterId: string) {
        // Logic to retrieve voter's dashboard data
        const voter = await Voter.findById(voterId);
        return voter;
    }

    async getVotingHistory(voterId: string) {
        // Logic to retrieve voter's voting history
        const votes = await Vote.find({ voter: voterId }).populate('candidate');
        return votes;
    }

    async castVote(voterId: string, candidateId: string) {
        // Logic to cast a vote
        const vote = new Vote({ voter: voterId, candidate: candidateId });
        await vote.save();
        return vote;
    }
}