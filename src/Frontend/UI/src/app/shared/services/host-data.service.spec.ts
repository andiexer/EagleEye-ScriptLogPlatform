/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { HostDataService } from './host-data.service';

describe('Service: HostData', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HostDataService]
    });
  });

  it('should ...', inject([HostDataService], (service: HostDataService) => {
    expect(service).toBeTruthy();
  }));
});
