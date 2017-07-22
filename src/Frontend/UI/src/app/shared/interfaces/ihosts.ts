import { IHost } from './ihost';
import { IPagination } from './ipagination';
export interface IHosts {
    pagination: IPagination;
    hosts: IHost[];
}
