import React from 'react';
import { Link } from 'react-router-dom';
import './Sidebar.css';

const Sidebar = () => {
    return (
        <div className="sidebar">
            <h2>Voting Management System</h2>
            <ul>
                <li>
                    <Link to="/voter/dashboard">Voter Dashboard</Link>
                </li>
                <li>
                    <Link to="/voter/candidates">Candidates</Link>
                </li>
                <li>
                    <Link to="/voter/voting-booth">Voting Booth</Link>
                </li>
                <li>
                    <Link to="/voter/history">Voting History</Link>
                </li>
                <li>
                    <Link to="/candidate/dashboard">Candidate Dashboard</Link>
                </li>
                <li>
                    <Link to="/candidate/profile">Profile</Link>
                </li>
                <li>
                    <Link to="/candidate/results">Vote Results</Link>
                </li>
            </ul>
        </div>
    );
};

export default Sidebar;