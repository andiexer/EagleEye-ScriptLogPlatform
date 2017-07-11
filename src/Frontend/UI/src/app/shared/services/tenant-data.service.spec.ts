/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TenantDataService } from './tenant-data.service';

describe('Service: TenantData', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TenantDataService]
    });
  });

  it('should ...', inject([TenantDataService], (service: TenantDataService) => {
    expect(service).toBeTruthy();
  }));
});
