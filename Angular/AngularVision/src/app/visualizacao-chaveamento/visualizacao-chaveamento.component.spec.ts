import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VisualizacaoChaveamentoComponent } from './visualizacao-chaveamento.component';

describe('VisualizacaoChaveamentoComponent', () => {
  let component: VisualizacaoChaveamentoComponent;
  let fixture: ComponentFixture<VisualizacaoChaveamentoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [VisualizacaoChaveamentoComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(VisualizacaoChaveamentoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
