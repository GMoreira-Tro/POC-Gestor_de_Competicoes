import { Component, ElementRef, Input, OnChanges, ViewChild } from '@angular/core';
import * as go from 'gojs';
import { Confronto } from '../interfaces/Confronto';
import { Competidor } from '../interfaces/Competidor';

@Component({
  selector: 'app-gojs-chaveamento',
  templateUrl: './gojs-chaveamento.component.html',
  styleUrls: ['./gojs-chaveamento.component.css']
})
export class GojsChaveamentoComponent implements OnChanges {
  @Input() chaveamento: any;
   @Input() confrontos: Confronto[] = [];
    @Input() competidores: Competidor[] = [];

  @ViewChild('diagramRef', { static: true }) diagramRef!: ElementRef;

  private diagram!: go.Diagram;

  ngOnChanges(): void {
    if (this.chaveamento) {
      this.initDiagram();
    }
  }

  initDiagram(): void {
    const $ = go.GraphObject.make;

    this.diagram = $(go.Diagram, this.diagramRef.nativeElement, {
      initialAutoScale: go.Diagram.Uniform,
      layout: $(go.LayeredDigraphLayout, { direction: 0 }),
      'undoManager.isEnabled': true
    });

    this.diagram.nodeTemplate = $(
      go.Node, 'Auto',
      $(go.Shape, 'RoundedRectangle', {
        fill: '#3e3e3e',
        stroke: '#555',
        strokeWidth: 1.5
      }),
      $(go.Panel, 'Table',
        { padding: 6 },
        $(go.TextBlock,
          { row: 0, font: 'bold 14px sans-serif', stroke: '#f5f5f5' },
          new go.Binding('text', 'nome')),
        $(go.TextBlock,
          { row: 1, font: '12px sans-serif', stroke: '#ccc' },
          new go.Binding('text', 'round', r => `Rodada ${r}`))
      )
    );

    this.diagram.linkTemplate = $(
      go.Link,
      { routing: go.Link.Orthogonal, corner: 5 },
      $(go.Shape, { stroke: '#1e90ff', strokeWidth: 2 })
    );

    const nodes: any[] = [];
    const links: any[] = [];

    const mapConfrontoToNodeKey: Map<any, string> = new Map();
    let nodeIndex = 0;

    this.chaveamento.rounds.forEach((round: any) => {
      round.confrontos.forEach((confronto: any, i: number) => {
        const nodeId = `round${round.numero}_conf${i}`;
        mapConfrontoToNodeKey.set(confronto, nodeId);

        nodes.push({
          key: nodeId,
          nome: confronto.vencedor?.nome || 'A definir',
          round: round.numero
        });

        if (round.numero > 1) {
          if (confronto.competidorA) {
            const previousNode = this.findPreviousNodeKey(confronto.competidorA.nome);
            if (previousNode) links.push({ from: previousNode, to: nodeId });
          }
          if (confronto.competidorB) {
            const previousNode = this.findPreviousNodeKey(confronto.competidorB.nome);
            if (previousNode) links.push({ from: previousNode, to: nodeId });
          }
        }
      });
    });

    this.diagram.model = new go.GraphLinksModel(nodes, links);
  }

  findPreviousNodeKey(nome: string): string | null {
    const model = this.diagram.model as go.GraphLinksModel;
    const match = model.nodeDataArray.find(n => n['nome'] === nome);
    return match ? match['key'] : null;
  }
}
