import { Pipe, PipeTransform } from '@angular/core';
import { IScript } from '../interfaces/iscript';

@Pipe({
  name: 'filterScripts',
  pure: false
})
export class FilterScriptsPipe implements PipeTransform {

  transform(value: IScript[], scriptname?: string): IScript[] {
    if (!value) {
      return value;
    };
    return value.filter(script => {
      return script.scriptName.toLowerCase().includes(scriptname.toLowerCase());
    });
  }

}
