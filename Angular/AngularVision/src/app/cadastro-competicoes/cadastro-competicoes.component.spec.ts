import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CadastroCompeticoesComponent } from './cadastro-competicoes.component';

describe('CadastroCompeticoesComponent', () => {
  let component: CadastroCompeticoesComponent;
  let fixture: ComponentFixture<CadastroCompeticoesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CadastroCompeticoesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CadastroCompeticoesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
