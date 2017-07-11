import { isNullOrUndefined } from 'util';
import { LogLevel } from '../';
import { Pipe, PipeTransform } from '@angular/core';
import { ILog } from '../interfaces/ilog';

@Pipe({
  name: 'filterLogs'
})
export class FilterLogsPipe implements PipeTransform {

  transform(value: ILog[], logLevel?: LogLevel[], text?: string): ILog[] {
    if (!value) {
      return value;
    }
    // filter
    return value.filter(log => {
      if (isNullOrUndefined(logLevel) || logLevel.length === 0) {
        return log.logText.toLowerCase().includes(text.toLowerCase());
      } else {
        return logLevel.indexOf(log.logLevel) !== -1
          && log.logText.toLowerCase().includes(text.toLowerCase());
      }
    });
  }

}
