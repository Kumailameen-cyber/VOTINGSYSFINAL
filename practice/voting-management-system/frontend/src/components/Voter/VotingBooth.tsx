import React, { useEffect, useState } from 'react';
import { useAuth } from '../../hooks/useAuth';
import { voteService } from '../../services/voterService';
import { Candidate } from '../../models/Candidate';
import './VotingBooth.css';

const VotingBooth: React.FC = () => {
    const { user } = useAuth();
    const [candidates, setCandidates] = useState<Candidate[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCandidates = async () => {
            try {
                const response = await voteService.getCandidates();
                setCandidates(response.data);
            } catch (err) {
                setError('Failed to load candidates');
            } finally {
                setLoading(false);
            }
        };

        fetchCandidates();
    }, []);

    const handleVote = async (candidateId: string) => {
        try {
            await voteService.castVote(candidateId, user.id);
            alert('Vote cast successfully!');
        } catch (err) {
            setError('Failed to cast vote');
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="voting-booth">
            <h2>Voting Booth</h2>
            <ul>
                {candidates.map(candidate => (
                    <li key={candidate.id}>
                        <div className="candidate-card">
                            <h3>{candidate.name}</h3>
                            <button onClick={() => handleVote(candidate.id)}>Vote</button>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default VotingBooth;