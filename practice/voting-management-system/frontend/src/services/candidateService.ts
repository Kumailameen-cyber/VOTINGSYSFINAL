export const getCandidates = async (token: string) => {
    const response = await fetch('/api/candidates', {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) {
        throw new Error('Failed to fetch candidates');
    }
    return await response.json();
};

export const getCandidateProfile = async (id: string, token: string) => {
    const response = await fetch(`/api/candidates/${id}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) {
        throw new Error('Failed to fetch candidate profile');
    }
    return await response.json();
};

export const createCandidate = async (candidateData: any, token: string) => {
    const response = await fetch('/api/candidates', {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(candidateData),
    });
    if (!response.ok) {
        throw new Error('Failed to create candidate');
    }
    return await response.json();
};

export const updateCandidate = async (id: string, candidateData: any, token: string) => {
    const response = await fetch(`/api/candidates/${id}`, {
        method: 'PUT',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(candidateData),
    });
    if (!response.ok) {
        throw new Error('Failed to update candidate');
    }
    return await response.json();
};

export const deleteCandidate = async (id: string, token: string) => {
    const response = await fetch(`/api/candidates/${id}`, {
        method: 'DELETE',
        headers: {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) {
        throw new Error('Failed to delete candidate');
    }
    return await response.json();
};