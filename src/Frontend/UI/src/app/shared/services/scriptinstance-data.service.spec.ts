/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ScriptinstanceDataService } from './scriptinstance-data.service';

describe('Service: ScriptinstanceData', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ScriptinstanceDataService]
    });
  });

  it('should ...', inject([ScriptinstanceDataService], (service: ScriptinstanceDataService) => {
    expect(service).toBeTruthy();
  }));
});
