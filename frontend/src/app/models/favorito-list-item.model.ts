import { Favorito } from './favorito.model';

export interface FavoritoListItem extends Favorito {
  atualizando: boolean;
}
