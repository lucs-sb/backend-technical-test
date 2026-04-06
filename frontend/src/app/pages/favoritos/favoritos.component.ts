import { AsyncPipe, CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { map } from 'rxjs';
import { FavoritoListItem } from '../../models/favorito-list-item.model';
import { Favorito } from '../../models/favorito.model';
import { FavoritosListComponent } from './components/favoritos-list/favoritos-list.component';
import { FavoritosFacadeService } from '../../services/favoritos-facade.service';

interface FavoritosPageViewModel {
  favoritos: FavoritoListItem[];
  carregando: boolean;
  mensagem: string;
  erro: string;
  erroAcao: string;
}

@Component({
  selector: 'app-favoritos-page',
  standalone: true,
  imports: [CommonModule, AsyncPipe, RouterLink, FavoritosListComponent],
  templateUrl: './favoritos.component.html',
  styleUrl: './favoritos.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class FavoritosComponent {
  private readonly favoritosFacade = inject(FavoritosFacadeService);

  readonly vm$ = this.favoritosFacade.observarPaginaFavoritos().pipe(
    map((state): FavoritosPageViewModel => ({
      favoritos: state.favoritos.map((favorito) => ({
        ...favorito,
        atualizando: state.atualizandoUrls.includes(favorito.htmlUrl)
      })),
      carregando: state.carregandoLista,
      mensagem: this.favoritosFacade.obterMensagemVazia(state),
      erro: state.erroLista,
      erroAcao: state.erroAcao
    }))
  );

  desfavoritar(favorito: Favorito): void {
    this.favoritosFacade.desfavoritar(favorito);
  }
}
