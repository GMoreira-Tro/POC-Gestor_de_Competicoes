import { Component, ElementRef, Input, ViewChild, AfterViewInit, OnChanges, SimpleChanges } from '@angular/core';
import * as go from 'gojs';

@Component({
  selector: 'app-visualizacao-chaveamento',
  template: `<div #diagramaRef style="width:100%; height:500px; background-color: #f0f0f0;"></div>`,
  styleUrls: ['./visualizacao-chaveamento.component.css']
})
export class VisualizacaoChaveamentoComponent implements AfterViewInit, OnChanges {
  @ViewChild('diagramaRef', { static: true }) diagramaRef!: ElementRef;
  @Input() nos: any[] = [];

  private diagram!: go.Diagram;
  private diagramaCriado = false;

  ngAfterViewInit(): void {
    if (this.nos.length > 0 && !this.diagramaCriado) {
      this.inicializarDiagrama();
    }
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['nos'] && !changes['nos'].firstChange && this.diagramaCriado) {
      this.diagram.model = new go.TreeModel(this.nos);
    }
  }

  inicializarDiagrama(): void {
    const $ = go.GraphObject.make;

    this.diagram = $(go.Diagram, this.diagramaRef.nativeElement, {
      layout: $(go.TreeLayout, {
        angle: 180,
        layerSpacing: 50
      }),
      'undoManager.isEnabled': true
    });

    this.diagram.nodeTemplate =
      $(go.Node, 'Auto',
        $(go.Shape, 'RoundedRectangle', { fill: '#ddd', stroke: '#555' }),
        $(go.TextBlock, { margin: 8, font: 'bold 12pt sans-serif' }, new go.Binding('text', 'name'))
      );

    this.diagram.linkTemplate =
      $(go.Link,
        { routing: go.Link.Orthogonal, corner: 5 },
        $(go.Shape, { strokeWidth: 2, stroke: '#1e90ff' })
      );

    this.diagram.model = new go.TreeModel(this.nos);

    this.diagramaCriado = true;
  }

  showNameSelector(node: go.Node, diagram: go.Diagram) {
    const nodeData = node.data;

    // Filhos do nó atual (se houver)
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

      // Se o nó tinha um nome antes, e outros nós também, atualiza todos
      if (oldName && oldName !== newName) {
        const nodeKey = nodeData.key;

        // Encontre todos os pais diretos com o mesmo nome anterior
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
