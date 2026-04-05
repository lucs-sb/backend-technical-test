import { Favorito } from './favorito.model';
import { Repositorio } from './repositorio.model';

export interface RepositorioListItem extends Repositorio {
  favoritado: boolean;
  favorito: Favorito | null;
  atualizandoFavorito: boolean;
}
