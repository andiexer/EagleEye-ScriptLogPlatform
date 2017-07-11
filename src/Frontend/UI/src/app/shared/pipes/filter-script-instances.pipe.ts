import { Pipe, PipeTransform } from '@angular/core';
import { IScriptInstance } from '../interfaces/iscript-instance';

declare var moment: any;

@Pipe({
  name: 'filterScriptInstances',
  pure: false
})
export class FilterScriptInstancesPipe implements PipeTransform {

  transform(
    value: IScriptInstance[],
    status?: string[],
    hostname?: string,
    scriptname?: string,
    dateFrom?: string,
    dateTo?: string,
    hourFrom?: string,
    hourTo?: string
  ): IScriptInstance[] {
    if (!value) {
      return value;
    }
    // filter
    if (dateFrom === '') { dateFrom = '01.01.1900'; }
    if (dateTo === '') { dateTo = '31.12.2200'; }
    let dateTimeFrom: Date = new Date(moment(dateFrom + ' ' + hourFrom, 'DD.MM.YYYY HH:mm'));
    let dateTimeTo: Date = new Date(moment(dateTo + ' ' + hourTo, 'DD.MM.YYYY HH:mm'));

    if (status.length > 0) {
      return value.filter(scriptInstance => {
        return status.indexOf(scriptInstance.instanceStatus) !== -1
          && scriptInstance.host.hostName.toLowerCase().includes(hostname.toLowerCase())
          && scriptInstance.script.scriptName.toLowerCase().includes(scriptname.toLowerCase())
          && new Date(scriptInstance.startDateTime) > dateTimeFrom
          && new Date(scriptInstance.startDateTime) < dateTimeTo;
      });
    } else {
      return value.filter(scriptInstance => {
        return scriptInstance.host.hostName.toLowerCase().includes(hostname.toLowerCase())
          && scriptInstance.script.scriptName.toLowerCase().includes(scriptname.toLowerCase())
          && new Date(scriptInstance.startDateTime) > dateTimeFrom
          && new Date(scriptInstance.startDateTime) < dateTimeTo;
      });
    }
  }

}
