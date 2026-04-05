import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Favorito } from '../../../../models/favorito.model';
import { Repositorio } from '../../../../models/repositorio.model';
import { RepositorioListItem } from '../../../../models/repositorio-list-item.model';

@Component({
  selector: 'app-repos-results-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './repos-results-list.component.html',
  styleUrl: './repos-results-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReposResultsListComponent {
  @Input({ required: true }) repositorios: RepositorioListItem[] = [];
  @Input({ required: true }) termoPesquisado = '';
  @Input() paginaAtual = 1;
  @Input() totalPaginas = 0;
  @Input() carregando = false;

  @Output() paginaChange = new EventEmitter<number>();
  @Output() favoritar = new EventEmitter<Repositorio>();
  @Output() desfavoritar = new EventEmitter<Favorito>();

  irParaPagina(pagina: number): void {
    this.paginaChange.emit(pagina);
  }

  favoritarRepositorio(repositorio: Repositorio): void {
    this.favoritar.emit(repositorio);
  }

  desfavoritarRepositorio(favorito: Favorito): void {
    this.desfavoritar.emit(favorito);
  }
}
