/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdManagementComponent } from './ad-management.component';

describe('AdManagementComponent', () => {
  let component: AdManagementComponent;
  let fixture: ComponentFixture<AdManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
