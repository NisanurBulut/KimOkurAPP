import { Photo } from './Photo';

export interface User {
    id: number;
    username: string;
    age: number;
    knownAs: string;
    created: Date;
    gender: string;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string;
    introduction?: string;
    lookingFor?: string;
    photos: Photo[];
}
