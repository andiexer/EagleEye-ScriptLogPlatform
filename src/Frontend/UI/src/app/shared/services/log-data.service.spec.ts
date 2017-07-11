/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LogDataService } from './log-data.service';

describe('Service: LogData', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LogDataService]
    });
  });

  it('should ...', inject([LogDataService], (service: LogDataService) => {
    expect(service).toBeTruthy();
  }));
});
