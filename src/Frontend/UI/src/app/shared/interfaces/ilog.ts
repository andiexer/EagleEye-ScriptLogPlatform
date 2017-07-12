import { LogLevel } from '../enums/log-level.enum';
export interface ILog {
    logLevel: LogLevel;
    logText: string;
    logDateTime: Date;
    scriptInstance: number;
}
