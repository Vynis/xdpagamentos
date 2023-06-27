/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { ContaPagarListaComponent } from './conta-pagar-lista.component';

describe('ContaPagarListaComponent', () => {
  let component: ContaPagarListaComponent;
  let fixture: ComponentFixture<ContaPagarListaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ContaPagarListaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ContaPagarListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
