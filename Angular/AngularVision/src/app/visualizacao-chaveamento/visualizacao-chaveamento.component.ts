import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as go from 'gojs';

@Component({
  selector: 'app-visualizacao-chaveamento',
  templateUrl: './visualizacao-chaveamento.component.html',
  styleUrls: ['./visualizacao-chaveamento.component.css']
})
export class VisualizacaoChaveamentoComponent implements OnInit {
  @ViewChild('diagramaRef', { static: true }) diagramaRef!: ElementRef;

  ngOnInit(): void {
    const $ = go.GraphObject.make;

    const myDiagram = $(go.Diagram, this.diagramaRef.nativeElement, {
      layout: $(go.TreeLayout, {
        angle: 180,
        layerSpacing: 50
      }),
      'undoManager.isEnabled': true
    });

    // Template do nó
    myDiagram.nodeTemplate =
      $(go.Node, 'Auto',
        $(go.Shape, 'RoundedRectangle',
          {
            fill: '#3e3e3e', stroke: '#555',
            portId: '', cursor: 'pointer'
          }),
        $(go.TextBlock,
          { margin: 8, stroke: '#f5f5f5', font: 'bold 12pt sans-serif' },
          new go.Binding('text', 'name'))
      );

    myDiagram.linkTemplate =
      $(go.Link,
        { routing: go.Link.Orthogonal, corner: 5 },
        $(go.Shape, { strokeWidth: 2, stroke: '#1e90ff' }));

    // Modelo com 7 nós (4 folhas, 2 semifinais, 1 final)
    myDiagram.model = new go.TreeModel([
      { key: 1, name: 'Luan', parent: 5 },
      { key: 2, name: 'Bianca', parent: 5 },
      { key: 3, name: 'Alex', parent: 6 },
      { key: 4, name: 'Guilherme', parent: 6 },
      { key: 5, name: '', parent: 7 }, // SF1
      { key: 6, name: '', parent: 7 }, // SF2
      { key: 7, name: '' }             // CAMPEÃO
    ]);


    myDiagram.addDiagramListener('ObjectSingleClicked', (e) => {
      const part = e.subject.part;
      if (part instanceof go.Node) {
        this.showNameSelector(part, myDiagram);
      }
    });

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
