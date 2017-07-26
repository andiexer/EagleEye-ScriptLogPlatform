import { IScriptInstance } from './iscript-instance';
import { IPagination } from './ipagination';
export interface IScriptInstances {
    pagination: IPagination;
    scriptInstances: IScriptInstance[];
}
