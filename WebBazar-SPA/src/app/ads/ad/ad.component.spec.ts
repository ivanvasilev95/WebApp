/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { AdComponent } from './ad.component';

describe('AdComponent', () => {
  let component: AdComponent;
  let fixture: ComponentFixture<AdComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
