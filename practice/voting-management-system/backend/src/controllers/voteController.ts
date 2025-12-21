export class VoteController {
    constructor(private voteService: any) {}

    async castVote(req: any, res: any) {
        try {
            const { voterId, candidateId } = req.body;
            const vote = await this.voteService.castVote(voterId, candidateId);
            res.status(201).json({ message: 'Vote cast successfully', vote });
        } catch (error) {
            res.status(500).json({ message: 'Error casting vote', error: error.message });
        }
    }

    async getVoteResults(req: any, res: any) {
        try {
            const results = await this.voteService.getVoteResults();
            res.status(200).json(results);
        } catch (error) {
            res.status(500).json({ message: 'Error retrieving vote results', error: error.message });
        }
    }
}