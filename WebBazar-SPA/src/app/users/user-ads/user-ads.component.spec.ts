/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UserAdsComponent } from './user-ads.component';

describe('UserAdsComponent', () => {
  let component: UserAdsComponent;
  let fixture: ComponentFixture<UserAdsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserAdsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserAdsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
