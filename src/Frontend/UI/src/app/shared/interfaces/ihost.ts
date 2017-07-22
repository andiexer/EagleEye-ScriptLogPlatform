export interface IHost {
    id?: number;
    hostname: string;
    fqdn: string;
    apiKey?: string;
    createdDateTime: Date;
    lastModDateTime: Date;
}
