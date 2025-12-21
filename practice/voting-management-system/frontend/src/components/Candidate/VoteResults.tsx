import React, { useEffect, useState } from 'react';
import { getVoteResults } from '../../services/candidateService';
import Card from '../../Common/Card';
import './VoteResults.css';

const VoteResults = () => {
    const [results, setResults] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchResults = async () => {
            try {
                const data = await getVoteResults();
                setResults(data);
            } catch (err) {
                setError('Failed to fetch vote results');
            } finally {
                setLoading(false);
            }
        };

        fetchResults();
    }, []);

    if (loading) {
        return <div>Loading...</div>;
    }

    if (error) {
        return <div>{error}</div>;
    }

    return (
        <div className="vote-results">
            <h2>Vote Results</h2>
            {results.map((result) => (
                <Card key={result.candidateId} className="result-card">
                    <h3>{result.candidateName}</h3>
                    <p>Votes: {result.voteCount}</p>
                </Card>
            ))}
        </div>
    );
};

export default VoteResults;