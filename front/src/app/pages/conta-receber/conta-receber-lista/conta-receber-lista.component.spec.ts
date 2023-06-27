/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ContaReceberListaComponent } from './conta-receber-lista.component';

describe('ContaReceberListaComponent', () => {
  let component: ContaReceberListaComponent;
  let fixture: ComponentFixture<ContaReceberListaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContaReceberListaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContaReceberListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
