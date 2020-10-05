/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LikeService } from './like.service';

describe('Service: Like', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LikeService]
    });
  });

  it('should ...', inject([LikeService], (service: LikeService) => {
    expect(service).toBeTruthy();
  }));
});
