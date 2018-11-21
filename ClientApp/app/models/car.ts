import { Contact } from './car'

export interface KeyValuePair {
    // Model and Make's two properties
    id: number;
    name: string;
}

// interface for contact
export interface Contact {
    name: string;
    phone: string;
    email: string;
}

export interface Car {
    id: number;
    model: KeyValuePair; 
    make: KeyValuePair; 
    isRegistered: boolean;
    features: KeyValuePair[];
    contact: Contact;
    lastUpdate: string;
}
export interface SaveCar {
    id: number;
    modelId: number; 
    makeId: number; 
    isRegistered: boolean;
    features: number[];
    contact: Contact;
}