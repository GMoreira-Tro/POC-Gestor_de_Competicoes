import { AfterViewInit, Component } from '@angular/core';
import { jsPlumb } from 'jsplumb';

@Component({
  selector: 'app-arvore-confrontos',
  templateUrl: './arvore-confrontos.component.html',
  styleUrls: ['./arvore-confrontos.component.css']
})
export class ArvoreConfrontosComponent implements AfterViewInit {

  jsPlumbInstance: any;

  ngAfterViewInit() {
    this.jsPlumbInstance = jsPlumb.getInstance({
      Connector: ["Bezier", { curviness: 50 }],
      PaintStyle: { stroke: "#1e90ff", strokeWidth: 3 },
      Endpoint: ["Dot", { radius: 5 }],
      EndpointStyle: { fill: "#1e90ff" },
      Anchors: ["Right", "Left"]
    });

    // Conexões da primeira para a segunda rodada
    this.jsPlumbInstance.connect({
      source: "confronto-1",
      target: "confronto-3"
    });

    this.jsPlumbInstance.connect({
      source: "confronto-2",
      target: "confronto-3"
    });

    // Conexões da segunda para a terceira rodada
    this.jsPlumbInstance.connect({
      source: "confronto-3",
      target: "confronto-4"
    });
  }
}
