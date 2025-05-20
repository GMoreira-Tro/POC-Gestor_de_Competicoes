import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import * as go from 'gojs';

@Component({
  selector: 'app-visualizacao-chaveamento-botao',
  templateUrl: './visualizacao-chaveamento-botao.component.html',
  styleUrls: ['./visualizacao-chaveamento-botao.component.css']
})
export class VisualizacaoChaveamentoBotaoComponent implements OnInit {
  @ViewChild('diagramaRef', { static: true }) diagramaRef!: ElementRef;

  diagram!: go.Diagram;
  participantes: string[] = [];
  nomesDisponiveis: string[] = ['Luan', 'Bianca', 'Alex', 'Guilherme', 'Maria', 'José', 'Ana', 'Carlos'];

  selecionando = false;
  opcoesVencedor: string[] = [];
  nodoSelecionado: any = null;
  posX = 0;
  posY = 0;

  vencedores: { [key: number]: string } = {}; // <--- Salva os vencedores manualmente


  ngOnInit(): void {
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
        $(go.Shape, 'RoundedRectangle',
          {
            fill: '#3e3e3e', stroke: '#555',
            portId: '', cursor: 'pointer'
          }),
        $(go.TextBlock,
          { margin: 8, stroke: '#f5f5f5', font: 'bold 12pt sans-serif' },
          new go.Binding('text', 'name'))
      );

    this.diagram.linkTemplate =
      $(go.Link,
        { routing: go.Link.Orthogonal, corner: 5 },
        $(go.Shape, { strokeWidth: 2, stroke: '#1e90ff' }));

    this.diagram.addDiagramListener('ObjectSingleClicked', (e) => {
      const part = e.subject.part;
      const data = part.data;

      if (!data) return;

      // Se for um nó de confronto (sem nome) e com dois filhos
      const filhos = this.diagram.model.nodeDataArray.filter(n => n['parent'] === data.key);
      if (filhos.length === 2 &&
        filhos[0]?.['name'] &&
        filhos[1]?.['name']
      ) {
        this.nodoSelecionado = data;
        this.opcoesVencedor = filhos.map(f => f['name']);
        this.posX = e.diagram.lastInput.documentPoint.x;
        this.posY = e.diagram.lastInput.documentPoint.y;
        this.selecionando = true;
      } else {
        this.selecionando = false;
      }
    });


    this.atualizarModelo();
  }

  limparAscendentes(filhoKey: number) {
    let atualKey = filhoKey;

    while (true) {
      // Busca o pai do nó atual
      const pai = this.diagram.model.nodeDataArray.find(n => n['key'] === atualKey)?.['parent'];
      if (pai === undefined) break;

      // Limpa o nome do pai e remove da lista de vencedores
      const paiNode = this.diagram.model.nodeDataArray.find(n => n['key'] === pai);
      if (paiNode) {
        this.diagram.model.startTransaction('limpar ascendentes');
        this.diagram.model.setDataProperty(paiNode, 'name', '');
        this.diagram.model.commitTransaction('limpar ascendentes');
        delete this.vencedores[pai];
      }

      atualKey = pai;
    }
  }

  adicionarParticipante() {
    if (this.nomesDisponiveis.length === 0) return;

    const nome = this.nomesDisponiveis.shift()!;
    this.participantes.push(nome);

    this.atualizarModelo();
  }

  atualizarModelo() {
    if (this.participantes.length === 0) {
      this.diagram.model = new go.TreeModel([]);
      return;
    }

    // Função que constrói a árvore de confrontos justa, segundo tua lógica
    function construirArvore(participantes: string[]) {
      let keyCounter = 1;

      // Cria os pares e os solitários na base
      const baseNodes: any[] = [];
      let i = 0;
      while (i < participantes.length) {
        if (i + 1 < participantes.length) {
          // par
          baseNodes.push({
            key: keyCounter++,
            name: '',
            left: { key: keyCounter++, name: participantes[i++] },
            right: { key: keyCounter++, name: participantes[i++] }
          });
        } else {
          // solitário
          baseNodes.push({
            key: keyCounter++,
            name: participantes[i++]
          });
        }
      }

      // Função para intercalar solitários com pares para a próxima rodada
      function intercalar(pares: any[], solitarios: any[]) {
        const result: any[] = [];
        let s = 0;
        for (let p = 0; p < pares.length; p++) {
          result.push(pares[p]);
          if (s < solitarios.length) {
            result.push(solitarios[s++]);
          }
        }
        // Se sobrou algum solitário, joga no final
        while (s < solitarios.length) {
          result.push(solitarios[s++]);
        }
        return result;
      }

      // Separar pares e solitários na base
      const pares = baseNodes.filter(n => n.left && n.right);
      const solitarios = baseNodes.filter(n => !n.left || !n.right);

      // Intercalar para construir as camadas superiores
      let camadaAtual = intercalar(pares, solitarios);

      // Subir na árvore até sobrar um só
      while (camadaAtual.length > 1) {
        const proxCamada: any[] = [];
        for (let i = 0; i < camadaAtual.length; i += 2) {
          if (i + 1 < camadaAtual.length) {
            proxCamada.push({
              key: keyCounter++,
              name: '',
              left: camadaAtual[i],
              right: camadaAtual[i + 1]
            });
          } else {
            // Caso ímpar, sobe solitário para próxima camada
            proxCamada.push(camadaAtual[i]);
          }
        }
        camadaAtual = proxCamada;
      }

      return camadaAtual[0];
    }

    const raiz = construirArvore([...this.participantes]);
    const modelo = this.linearizar(raiz);

    this.diagram.model = new go.TreeModel(modelo);
  }

  linearizar(nodo: any, parentKey?: number, lista: any[] = []): any[] {
    lista.push({ key: nodo.key, name: nodo.name || '', parent: parentKey });
    if (nodo.left) this.linearizar(nodo.left, nodo.key, lista);
    if (nodo.right) this.linearizar(nodo.right, nodo.key, lista);

    return lista.map(n => {
      if (n.name === '' && this.vencedores[n.key]) {
        return { ...n, name: this.vencedores[n.key] };
      }
      return n;
    });
  }


  definirVencedor(nome: string) {
    if (!this.nodoSelecionado) return;

    const key = this.nodoSelecionado.key;

    this.limparAscendentes(key); // <--- limpa os nós acima antes de colocar o novo nome
    this.vencedores[key] = nome;

    const model = this.diagram.model as go.TreeModel;
    model.startTransaction('define vencedor');
    model.setDataProperty(this.nodoSelecionado, 'name', nome);
    model.commitTransaction('define vencedor');

    this.selecionando = false;
  }

}
