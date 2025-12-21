import React, { useEffect, useState, useContext } from 'react';
import { useParams } from 'react-router-dom';
import { AuthContext } from '../../context/AuthContext';
import { getCandidateProfile } from '../../services/candidateService';
import './CandidateProfile.css';

const CandidateProfile = () => {
    const { id } = useParams();
    const { token } = useContext(AuthContext);
    const [candidate, setCandidate] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    useEffect(() => {
        const fetchCandidateProfile = async () => {
            try {
                const profile = await getCandidateProfile(id, token);
                setCandidate(profile);
            } catch (err) {
                setError('Failed to fetch candidate profile');
            } finally {
                setLoading(false);
            }
        };

        fetchCandidateProfile();
    }, [id, token]);

    if (loading) return <div>Loading...</div>;
    if (error) return <div>{error}</div>;

    return (
        <div className="candidate-profile">
            <h1>{candidate.name}</h1>
            <div className="profile-details">
                <p><strong>Party:</strong> {candidate.party}</p>
                <p><strong>Biography:</strong> {candidate.biography}</p>
                <p><strong>Campaign Goals:</strong> {candidate.campaignGoals}</p>
            </div>
        </div>
    );
};

export default CandidateProfile;