import { ITenant } from './itenant';
export interface IHost {
    id?: number;
    hostName: string;
    domain: string;
    cloudZone: string;
    apiKey?: string;
    powershellVersion: string;
    tenant: ITenant;
}
