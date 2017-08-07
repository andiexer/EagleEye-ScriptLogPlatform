import { Pipe, PipeTransform } from '@angular/core';
import { LogLevel } from '../';

@Pipe({
  name: 'logLevelIcon'
})
export class LogLevelIconPipe implements PipeTransform {

  transform(value: string, logLevel: string): string {
    var result: string;
    switch (value) {
      case 'Fatal': 
        result = 'error';
        break;
      case 'Error':
        result = 'error';
        break;
      case 'Warning':
        result = 'warning';
        break;
      default:
        result = 'info';
    }
    return result;
  }

}
