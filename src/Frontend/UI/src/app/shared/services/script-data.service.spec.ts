/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ScriptDataService } from './script-data.service';

describe('ScriptDataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ScriptDataService]
    });
  });

  it('should ...', inject([ScriptDataService], (service: ScriptDataService) => {
    expect(service).toBeTruthy();
  }));
});
