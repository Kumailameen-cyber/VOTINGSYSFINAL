import React, { useEffect, useState } from 'react';
import { Candidate } from '../../models/Candidate';
import { getCandidates } from '../../services/candidateService';
import Card from '../Common/Card';
import './CandidateList.css';

const CandidateList: React.FC = () => {
    const [candidates, setCandidates] = useState<Candidate[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const fetchCandidates = async () => {
            try {
                const response = await getCandidates();
                setCandidates(response.data);
            } catch (err) {
                setError('Failed to fetch candidates');
            } finally {
                setLoading(false);
            }
        };

        fetchCandidates();
    }, []);

    if (loading) {
        return <div>Loading candidates...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="candidate-list">
            <h2>Available Candidates</h2>
            <div className="candidate-cards">
                {candidates.map(candidate => (
                    <Card key={candidate.id} title={candidate.name} description={candidate.description} />
                ))}
            </div>
        </div>
    );
};

export default CandidateList;