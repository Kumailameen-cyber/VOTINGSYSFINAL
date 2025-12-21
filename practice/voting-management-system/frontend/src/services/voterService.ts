export const getVotingHistory = async (token: string) => {
    const response = await fetch('/api/voter/voting-history', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });
    return response.json();
};

export const getCandidates = async (token: string) => {
    const response = await fetch('/api/voter/candidates', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });
    return response.json();
};

export const castVote = async (token: string, candidateId: string) => {
    const response = await fetch('/api/voter/cast-vote', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ candidateId }),
    });
    return response.json();
};