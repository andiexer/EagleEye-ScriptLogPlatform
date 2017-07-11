import { IScript } from './iscript';
import { IHost } from './ihost';
export interface IScriptInstance {
    instanceStatus: string;
    instanceGuid: string;
    startDateTime: Date;
    endDateTime: Date;
    host: IHost;
    script: IScript;
    transactionId: string;
}
