import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CRUDComponent } from './crud.component';

describe('CRUDComponent', () => {
  let component: CRUDComponent;
  let fixture: ComponentFixture<CRUDComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CRUDComponent]
    });
    fixture = TestBed.createComponent(CRUDComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
