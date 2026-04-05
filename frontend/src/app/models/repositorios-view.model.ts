import { FavoritoListItem } from './favorito-list-item.model';
import { RepositorioListItem } from './repositorio-list-item.model';

export interface RepositoriosViewModel {
  termoBusca: string;
  termoPesquisado: string;
  repositorios: RepositorioListItem[];
  paginaAtual: number;
  totalItens: number;
  totalPaginas: number;
  tamanhoPagina: number;
  carregando: boolean;
  mensagem: string;
  erro: string;
  favoritos: FavoritoListItem[];
  carregandoFavoritos: boolean;
  mensagemFavoritos: string;
  erroFavoritos: string;
  erroAcaoFavoritos: string;
}
