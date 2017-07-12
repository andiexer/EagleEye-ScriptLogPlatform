import { IScript } from './iscript';
import { IHost } from './ihost';
export interface IScriptInstance {
    id: number;
    instanceStatus: string;
    host: IHost;
    script: IScript;
    transactionId: string;
    endDateTime: Date;
    createdDateTime: Date;
    lastModDateTime: Date;
}
