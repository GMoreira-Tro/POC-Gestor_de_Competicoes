import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filtrarPorCategoria'
})
export class FiltrarPorCategoriaPipe implements PipeTransform {
  transform(items: any[], categoriaId: number): any[] {
    if (!items || !categoriaId) {
      return items;
    }
    return items.filter(item => item.categoriaId === categoriaId);
  }
}