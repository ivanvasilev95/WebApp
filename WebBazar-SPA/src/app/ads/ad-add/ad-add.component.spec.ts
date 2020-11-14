/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { NewAddComponent } from './ad-add.component';

describe('AdAddComponent', () => {
  let component: NewAddComponent;
  let fixture: ComponentFixture<NewAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NewAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
