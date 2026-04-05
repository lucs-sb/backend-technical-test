import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Favorito } from '../../../../models/favorito.model';
import { FavoritoListItem } from '../../../../models/favorito-list-item.model';

@Component({
  selector: 'app-favoritos-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './favoritos-list.component.html',
  styleUrl: './favoritos-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FavoritosListComponent {
  @Input({ required: true }) favoritos: FavoritoListItem[] = [];
  @Input() carregando = false;
  @Input() mensagem = '';
  @Input() erro = '';

  @Output() removerFavorito = new EventEmitter<Favorito>();

  remover(favorito: Favorito): void {
    this.removerFavorito.emit(favorito);
  }
}
