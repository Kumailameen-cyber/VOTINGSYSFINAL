import React, { useEffect, useState, useContext } from 'react';
import { AuthContext } from '../../context/AuthContext';
import { candidateService } from '../../services/candidateService';
import Card from '../Common/Card';
import './CandidateDashboard.css';

const CandidateDashboard = () => {
    const { user } = useContext(AuthContext);
    const [candidates, setCandidates] = useState([]);

    useEffect(() => {
        const fetchCandidates = async () => {
            try {
                const response = await candidateService.getCandidates();
                setCandidates(response.data);
            } catch (error) {
                console.error('Error fetching candidates:', error);
            }
        };

        fetchCandidates();
    }, []);

    return (
        <div className="candidate-dashboard">
            <h1>Welcome, {user.name}</h1>
            <h2>Your Candidates</h2>
            <div className="candidate-list">
                {candidates.map(candidate => (
                    <Card key={candidate.id} title={candidate.name} description={candidate.description} />
                ))}
            </div>
        </div>
    );
};

export default CandidateDashboard;