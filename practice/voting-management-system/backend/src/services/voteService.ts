import { Vote } from '../models/Vote';
import { Voter } from '../models/Voter';
import { Candidate } from '../models/Candidate';

export class VoteService {
    async castVote(voterId: string, candidateId: string): Promise<Vote> {
        const vote = new Vote({ voter: voterId, candidate: candidateId });
        await vote.save();
        return vote;
    }

    async getVoteResults(candidateId: string): Promise<Vote[]> {
        return await Vote.find({ candidate: candidateId });
    }

    async getVotingHistory(voterId: string): Promise<Vote[]> {
        return await Vote.find({ voter: voterId }).populate('candidate');
    }
}