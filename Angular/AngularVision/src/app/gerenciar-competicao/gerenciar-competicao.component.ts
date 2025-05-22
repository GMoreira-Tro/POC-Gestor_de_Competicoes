import { Component, ElementRef, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CategoriaService } from '../services/categoria.service';
import { InscricaoService } from '../services/inscricao.service';
import { CompeticaoService } from '../services/competicao.service';
import { UserService } from '../services/user.service';
import { CompetidorService } from '../services/competidor.service';
import * as go from 'gojs';

@Component({
  selector: 'app-gerenciar-competicao',
  templateUrl: './gerenciar-competicao.component.html',
  styleUrls: ['./gerenciar-competicao.component.css']
})
export class GerenciarCompeticaoComponent implements OnInit, AfterViewInit {
  @ViewChild('diagramaRef', { static: false }) diagramaRef!: ElementRef;

  competicaoId!: number;
  categorias: any[] = [];
  categoriaSelecionada: any;
  inscricoes: any[] = [];
  carregouInscricao: boolean = false;
  chaveamentoSelecionado: any = null;
  chaveamentos: any[] = [];
  chaveamentosPorCategoria: { [categoriaId: number]: any[] } = {};
  diagram: go.Diagram | undefined;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private competicaoService: CompeticaoService,
    private userService: UserService,
    private competidorService: CompetidorService,
    private categoriaService: CategoriaService,
    private inscricaoService: InscricaoService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.competicaoId = +id;

    this.competicaoService.getCompeticao(this.competicaoId).subscribe(
      competicao => {
        this.userService.getUsuarioLogado().subscribe(usuario => {
          const userId = usuario.id;
          if (competicao.criadorUsuarioId !== userId) {
            console.log("Usuário tentou editar uma competição que não lhe pertence.");
            this.router.navigate(['/']);
            return;
          }
        });
      },
      error => console.log('Erro ao carregar competição:', error)
    );
    this.carregarCategorias();
  }

  ngAfterViewInit() {
    this.inicializarDiagram();
  }

  carregarCategorias(): void {
    this.categoriaService.getCategoriasPorCompeticao(this.competicaoId).subscribe(
      res => this.categorias = res,
      err => console.error('Erro ao carregar categorias', err)
    );
  }

  onCategoriaSelecionada(): void {
    if (!this.categoriaSelecionada) return;

    this.inscricoes = [];
    this.carregouInscricao = false;

    if (this.chaveamentosPorCategoria[this.categoriaSelecionada]) {
      this.chaveamentos = this.chaveamentosPorCategoria[this.categoriaSelecionada];
    } else {
      this.chaveamentos = [];
      this.chaveamentosPorCategoria[this.categoriaSelecionada] = this.chaveamentos;
    }

    this.chaveamentoSelecionado = null;

    this.inscricaoService.getInscricoesPorCategoria(this.categoriaSelecionada).subscribe(
      res => {
        this.inscricoes = res.filter(i => i.status === 1);
        const inscricoesQuant = this.inscricoes.length;
        let inscricoesCarregadas = 0;

        this.inscricoes.forEach(inscricao => {
          this.inscricaoService.getInfoInscricao(inscricao.id).subscribe(inscricaoInfo => {
            inscricao.inscricaoInfo = inscricaoInfo;
            this.competidorService.obterCompetidor(inscricao.competidorId).subscribe(competidor => {
              inscricao.inscricaoInfo.competidor = competidor;
            });
            inscricoesCarregadas++;
            if (inscricoesCarregadas === inscricoesQuant) {
              this.carregouInscricao = true;
              this.inicializarDiagram();
            }
          });
        });
      },
      err => console.error('Erro ao carregar inscrições', err)
    );
  }

  inicializarDiagram(): void {
    if (!this.diagramaRef) return;

    const $ = go.GraphObject.make;

    const diagram = $(go.Diagram, this.diagramaRef.nativeElement, {
      layout: $(go.TreeLayout, { angle: 180, layerSpacing: 50 }),
      'undoManager.isEnabled': true
    });

    diagram.nodeTemplate =
      $(go.Node, 'Auto',
        $(go.Shape, 'RoundedRectangle',
          { fill: '#3e3e3e', stroke: '#555', portId: '', cursor: 'pointer' }),
        $(go.TextBlock,
          { margin: 8, stroke: '#f5f5f5', font: 'bold 12pt sans-serif' },
          new go.Binding('text', 'name'))
      );

    diagram.linkTemplate =
      $(go.Link,
        { routing: go.Link.Orthogonal, corner: 5 },
        $(go.Shape, { strokeWidth: 2, stroke: '#1e90ff' })
      );

    diagram.model = new go.TreeModel([
      { key: 1, name: 'Luan', parent: 5 },
      { key: 2, name: 'Bianca', parent: 5 },
      { key: 3, name: 'Alex', parent: 6 },
      { key: 4, name: 'Guilherme', parent: 6 },
      { key: 5, name: '', parent: 7 },
      { key: 6, name: '', parent: 7 },
      { key: 7, name: '' }
    ]);

    diagram.addDiagramListener('ObjectSingleClicked', (e) => {
      const part = e.subject.part;
      if (part instanceof go.Node) {
        this.showNameSelector(part, diagram);
      }
    });

    this.diagram = diagram;
  }

  showNameSelector(node: go.Node, diagram: go.Diagram) {
    const nodeData = node.data;
    const children = diagram.model.nodeDataArray.filter(n => n['parent'] === nodeData.key);
    const options = children.map(c => c['name']).filter(n => n);
    const allNames = [...new Set(diagram.model.nodeDataArray.map(n => n['name']).filter(n => n))];
    const availableNames = options.length >= 2 ? options : allNames;

    if (availableNames.length === 0) return;

    const select = document.createElement('select');
    select.style.position = 'absolute';
    select.style.zIndex = '100';
    select.style.left = `${node.actualBounds.centerX - 50}px`;
    select.style.top = `${node.actualBounds.centerY - 10}px`;

    const emptyOption = document.createElement('option');
    emptyOption.value = '';
    emptyOption.text = 'Selecione...';
    select.appendChild(emptyOption);

    availableNames.forEach(name => {
      const option = document.createElement('option');
      option.value = name;
      option.text = name;
      select.appendChild(option);
    });

    select.onchange = (e: any) => {
      const newName = e.target.value;
      const model = diagram.model;
      const oldName = nodeData.name;

      model.startTransaction('atualizar nomes');
      model.setDataProperty(nodeData, 'name', newName);

      if (oldName && oldName !== newName) {
        const nodeKey = nodeData.key;
        const paisComMesmoNome = diagram.model.nodeDataArray.filter(n => {
          const filhos = diagram.model.nodeDataArray.filter(f => f['parent'] === n['key']);
          return filhos.some(f => f['key'] === nodeKey) && n['name'] === oldName;
        });

        paisComMesmoNome.forEach(pai => {
          model.setDataProperty(pai, 'name', newName);
        });
      }

      model.commitTransaction('atualizar nomes');
      document.body.removeChild(select);
    };

    select.onblur = () => {
      if (document.body.contains(select)) {
        document.body.removeChild(select);
      }
    };

    document.body.appendChild(select);
    select.focus();
  }
}