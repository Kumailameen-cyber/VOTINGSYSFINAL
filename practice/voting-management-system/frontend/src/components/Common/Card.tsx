import React from 'react';
import './Card.css';

interface CardProps {
    title: string;
    content: React.ReactNode;
    footer?: React.ReactNode;
}

const Card: React.FC<CardProps> = ({ title, content, footer }) => {
    return (
        <div className="card">
            <div className="card-header">
                <h3>{title}</h3>
            </div>
            <div className="card-content">
                {content}
            </div>
            {footer && <div className="card-footer">{footer}</div>}
        </div>
    );
};

export default Card;