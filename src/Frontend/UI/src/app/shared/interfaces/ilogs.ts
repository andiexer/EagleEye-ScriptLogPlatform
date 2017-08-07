import { IPagination } from './ipagination';
import { ILog } from './ilog';
export interface ILogs {
    pagination: IPagination;
    logs: ILog[];
}
