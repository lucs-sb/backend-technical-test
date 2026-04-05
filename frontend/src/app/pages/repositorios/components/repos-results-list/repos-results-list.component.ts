import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Repositorio } from '../../../../models/repositorio.model';

@Component({
  selector: 'app-repos-results-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './repos-results-list.component.html',
  styleUrl: './repos-results-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReposResultsListComponent {
  @Input({ required: true }) repositorios: Repositorio[] = [];
  @Input({ required: true }) termoPesquisado = '';
  @Input() paginaAtual = 1;
  @Input() totalPaginas = 0;
  @Input() carregando = false;

  @Output() paginaChange = new EventEmitter<number>();

  irParaPagina(pagina: number): void {
    this.paginaChange.emit(pagina);
  }
}
