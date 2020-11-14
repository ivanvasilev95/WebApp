/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { UserFavoritesComponent } from './user-favorites.component';

describe('FavoritesComponent', () => {
  let component: UserFavoritesComponent;
  let fixture: ComponentFixture<UserFavoritesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserFavoritesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserFavoritesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
