export interface User {
    id: string;
    username: string;
    password: string;
    role: 'voter' | 'candidate';
}

export const UserSchema = {
    id: { type: String, required: true },
    username: { type: String, required: true, unique: true },
    password: { type: String, required: true },
    role: { type: String, enum: ['voter', 'candidate'], required: true }
};