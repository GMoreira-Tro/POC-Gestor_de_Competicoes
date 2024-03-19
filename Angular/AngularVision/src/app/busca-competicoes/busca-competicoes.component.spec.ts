import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BuscaCompeticoesComponent } from './busca-competicoes.component';

describe('BuscaCompeticoesComponent', () => {
  let component: BuscaCompeticoesComponent;
  let fixture: ComponentFixture<BuscaCompeticoesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BuscaCompeticoesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BuscaCompeticoesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
