import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalhesCompeticaoComponent } from './detalhes-competicao.component';

describe('DetalhesCompeticaoComponent', () => {
  let component: DetalhesCompeticaoComponent;
  let fixture: ComponentFixture<DetalhesCompeticaoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalhesCompeticaoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DetalhesCompeticaoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
