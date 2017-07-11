import { Pipe, PipeTransform } from '@angular/core';
import { IHost } from '../interfaces/ihost';

@Pipe({
  name: 'filterHosts',
  pure: false
})
export class FilterHostsPipe implements PipeTransform {

  transform(value: IHost[], hostname?: string, tenantName?: string): IHost[] {
    if (!value) {
      return value;
    }
    // filter
    return value.filter(host => {
      return host.hostName.toLowerCase().includes(hostname.toLowerCase())
              && host.tenant.tenantName.toLowerCase().includes(tenantName.toLowerCase());
    });
  }

}
