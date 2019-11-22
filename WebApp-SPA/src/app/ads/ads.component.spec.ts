/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdsComponent } from './ads.component';

describe('AdsComponent', () => {
  let component: AdsComponent;
  let fixture: ComponentFixture<AdsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
