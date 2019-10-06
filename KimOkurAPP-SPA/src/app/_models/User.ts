import { Photo } from './Photo';

export interface User {
    id: number;
    username: string;
    age: number;
    knownAs: string;
    city: string;
    country: string;
    created: Date;
    gender: string;
    lastActive: Date;
    photoUrl: string;
    interest?: string;
    introduction?: string;
    lookingFor?: string;
    photos: Photo[];
}
