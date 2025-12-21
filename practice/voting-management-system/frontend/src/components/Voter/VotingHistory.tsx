import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../../context/AuthContext';
import { getVotingHistory } from '../../services/voterService';
import Card from '../Common/Card';
import './VotingHistory.css';

const VotingHistory: React.FC = () => {
    const { user } = useContext(AuthContext);
    const [history, setHistory] = useState([]);

    useEffect(() => {
        const fetchVotingHistory = async () => {
            try {
                const response = await getVotingHistory(user.token);
                setHistory(response.data);
            } catch (error) {
                console.error('Error fetching voting history:', error);
            }
        };

        fetchVotingHistory();
    }, [user.token]);

    return (
        <div className="voting-history">
            <h2>Your Voting History</h2>
            {history.length === 0 ? (
                <p>No voting history available.</p>
            ) : (
                history.map((vote) => (
                    <Card key={vote.id} className="vote-card">
                        <p>Candidate: {vote.candidateName}</p>
                        <p>Date: {new Date(vote.date).toLocaleDateString()}</p>
                    </Card>
                ))
            )}
        </div>
    );
};

export default VotingHistory;