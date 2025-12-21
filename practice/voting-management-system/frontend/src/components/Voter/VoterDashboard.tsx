import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../../context/AuthContext';
import { voterService } from '../../services/voterService';
import { Card } from '../Common/Card';
import { VotingHistory } from './VotingHistory';
import { CandidateList } from './CandidateList';

const VoterDashboard: React.FC = () => {
    const { user } = useContext(AuthContext);
    const [votingHistory, setVotingHistory] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchVotingHistory = async () => {
            try {
                const history = await voterService.getVotingHistory(user.id);
                setVotingHistory(history);
            } catch (error) {
                console.error('Error fetching voting history:', error);
            } finally {
                setLoading(false);
            }
        };

        fetchVotingHistory();
    }, [user.id]);

    return (
        <div className="voter-dashboard">
            <h1>Welcome, {user.name}</h1>
            <Card title="Your Voting History">
                {loading ? (
                    <p>Loading...</p>
                ) : (
                    <VotingHistory history={votingHistory} />
                )}
            </Card>
            <Card title="Available Candidates">
                <CandidateList />
            </Card>
        </div>
    );
};

export default VoterDashboard;