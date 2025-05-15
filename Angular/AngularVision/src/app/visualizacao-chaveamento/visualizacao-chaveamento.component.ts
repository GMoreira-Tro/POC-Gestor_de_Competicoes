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
        angle: 0, // Horizontal
        layerSpacing: 40
      }),
      'undoManager.isEnabled': true
    });

    myDiagram.nodeTemplate =
      $(go.Node, 'Auto',
        $(go.Shape, 'RoundedRectangle',
          { fill: '#3e3e3e', stroke: '#555' }),
        $(go.TextBlock,
          { margin: 8, stroke: '#f5f5f5', font: 'bold 12pt sans-serif' },
          new go.Binding('text', 'name'))
      );

    myDiagram.linkTemplate =
      $(go.Link,
        { routing: go.Link.Orthogonal, corner: 5 },
        $(go.Shape, { strokeWidth: 2, stroke: '#1e90ff' }));

    myDiagram.model = new go.TreeModel([
      { key: 1, name: 'Guilherme' },
      { key: 2, name: 'Alex', parent: 1 },
      { key: 3, name: 'Bianca', parent: 1 },
      { key: 4, name: 'Luan', parent: 2 }
    ]);
  }
}
