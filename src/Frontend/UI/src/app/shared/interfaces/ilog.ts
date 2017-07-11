import { LogLevel } from '../enums/log-level.enum';
export interface ILog {
    logLevel: LogLevel;
    logDateTime: Date;
    logText: string;
    scriptInstance: string;
    scriptName: string;
    scriptVersion: string;
    hostName: string;
}
