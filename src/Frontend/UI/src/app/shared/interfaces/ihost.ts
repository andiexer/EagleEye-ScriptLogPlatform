export interface IHost {
    id?: number;
    hostName: string;
    fqdn: string;
    apiKey?: string;
    createdDateTime: Date;
    lastModDateTime: Date;
}
