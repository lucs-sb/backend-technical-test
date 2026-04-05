import { AsyncPipe, CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { BehaviorSubject, Subject, catchError, combineLatest, map, merge, of, shareReplay, startWith, switchMap } from 'rxjs';
import { BuscaRepositoriosParams } from '../../models/busca-repositorios.model';
import { Favorito } from '../../models/favorito.model';
import { PaginacaoResultado } from '../../models/paginacao-resultado.model';
import { Repositorio } from '../../models/repositorio.model';
import { RepositorioListItem } from '../../models/repositorio-list-item.model';
import { RepositoriosViewModel } from '../../models/repositorios-view.model';
import { ReposResultsListComponent } from './components/repos-results-list/repos-results-list.component';
import { ReposSearchFormComponent } from './components/repos-search-form/repos-search-form.component';
import { FavoritosFacadeService } from '../../services/favoritos-facade.service';
import { RepositorioService } from '../../services/repositorio.service';

const TAMANHO_PAGINA_PADRAO = 10;
const MENSAGEM_BUSCA_VAZIA = 'Informe um termo para buscar repositórios.';
const MENSAGEM_SEM_RESULTADOS = 'Nenhum repositório foi encontrado para a busca informada.';
const MENSAGEM_ERRO_API = 'Não foi possível carregar os repositórios agora. Tente novamente em instantes.';

interface BuscaState {
  termoPesquisado: string;
  repositorios: Repositorio[];
  paginaAtual: number;
  totalItens: number;
  totalPaginas: number;
  tamanhoPagina: number;
  carregando: boolean;
  mensagem: string;
  erro: string;
}

const ESTADO_INICIAL_BUSCA: BuscaState = {
  termoPesquisado: '',
  repositorios: [],
  paginaAtual: 1,
  totalItens: 0,
  totalPaginas: 0,
  tamanhoPagina: TAMANHO_PAGINA_PADRAO,
  carregando: false,
  mensagem: MENSAGEM_BUSCA_VAZIA,
  erro: ''
};

@Component({
  selector: 'app-repositorios',
  standalone: true,
  imports: [CommonModule, AsyncPipe, RouterLink, ReposSearchFormComponent, ReposResultsListComponent],
  templateUrl: './repos.component.html',
  styleUrls: ['./repos.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RepositoriosComponent {
  private readonly repositorioService = inject(RepositorioService);
  private readonly favoritosFacade = inject(FavoritosFacadeService);
  private readonly termoBuscaSubject = new BehaviorSubject<string>('');
  private readonly comandoBuscaSubject = new Subject<BuscaRepositoriosParams>();
  private readonly estadoManualSubject = new Subject<BuscaState>();

  private readonly estadoBusca$ = mergeEstadoBusca(
    this.comandoBuscaSubject,
    this.estadoManualSubject,
    this.repositorioService
  );

  private readonly estadoFavoritos$ = this.favoritosFacade.state$;

  readonly vm$ = combineLatest([
    this.termoBuscaSubject,
    this.estadoBusca$,
    this.estadoFavoritos$
  ]).pipe(
    map(([termoBusca, estadoBusca, estadoFavoritos]) => ({
      termoBusca,
      ...estadoBusca,
      repositorios: mapearRepositoriosParaListagem(estadoBusca.repositorios, estadoFavoritos),
      favoritos: [],
      carregandoFavoritos: estadoFavoritos.carregandoLista,
      mensagemFavoritos: this.favoritosFacade.obterMensagemVazia(estadoFavoritos),
      erroFavoritos: estadoFavoritos.erroLista,
      erroAcaoFavoritos: estadoFavoritos.erroAcao
    })),
    shareReplay({ bufferSize: 1, refCount: true })
  );

  atualizarTermoBusca(termo: string): void {
    this.termoBuscaSubject.next(termo);
  }

  buscarRepositorios(): void {
    const termo = this.obterTermoNormalizado();

    if (!termo) {
      this.estadoManualSubject.next({
        ...ESTADO_INICIAL_BUSCA,
        mensagem: MENSAGEM_BUSCA_VAZIA
      });
      return;
    }

    this.executarBusca(1, termo);
  }

  mudarPagina(pagina: number): void {
    const termo = this.obterTermoNormalizado();

    if (!termo || pagina < 1) {
      return;
    }

    this.executarBusca(pagina, termo);
  }

  favoritarRepositorio(repositorio: Repositorio): void {
    this.favoritosFacade.favoritar(repositorio);
  }

  desfavoritarRepositorio(favorito: Favorito): void {
    this.favoritosFacade.desfavoritar(favorito);
  }

  private executarBusca(pagina: number, nome: string): void {
    this.comandoBuscaSubject.next({
      nome,
      pagina,
      tamanhoPagina: TAMANHO_PAGINA_PADRAO
    });
  }

  private obterTermoNormalizado(): string {
    return this.termoBuscaSubject.value.trim();
  }
}

function mergeEstadoBusca(
  comandoBusca$: Subject<BuscaRepositoriosParams>,
  estadoManual$: Subject<BuscaState>,
  repositorioService: RepositorioService
) {
  return merge(
    estadoManual$,
    comandoBusca$.pipe(
      switchMap((params) =>
        repositorioService.buscarRepositorios(params).pipe(
          map((resultado) => mapearSucessoBusca(resultado, params)),
          startWith(mapearCarregandoBusca(params)),
          catchError(() => of(mapearErroBusca(params)))
        )
      ),
    )
  ).pipe(
    startWith(ESTADO_INICIAL_BUSCA)
  );
}

function mapearCarregandoBusca(params: BuscaRepositoriosParams): BuscaState {
  return {
    termoPesquisado: params.nome,
    repositorios: [],
    paginaAtual: params.pagina,
    totalItens: 0,
    totalPaginas: 0,
    tamanhoPagina: params.tamanhoPagina,
    carregando: true,
    mensagem: '',
    erro: ''
  };
}

function mapearSucessoBusca(
  resultado: PaginacaoResultado<Repositorio>,
  params: BuscaRepositoriosParams
): BuscaState {
  return {
    termoPesquisado: params.nome,
    repositorios: resultado.itens,
    paginaAtual: resultado.pagina,
    totalItens: resultado.totalItens,
    totalPaginas: calcularTotalPaginas(resultado.totalItens, resultado.tamanhoPagina),
    tamanhoPagina: resultado.tamanhoPagina,
    carregando: false,
    mensagem: resultado.itens.length ? '' : MENSAGEM_SEM_RESULTADOS,
    erro: ''
  };
}

function mapearErroBusca(params: BuscaRepositoriosParams): BuscaState {
  return {
    termoPesquisado: params.nome,
    repositorios: [],
    paginaAtual: 1,
    totalItens: 0,
    totalPaginas: 0,
    tamanhoPagina: params.tamanhoPagina,
    carregando: false,
    mensagem: '',
    erro: MENSAGEM_ERRO_API
  };
}

function calcularTotalPaginas(totalItens: number, tamanhoPagina: number): number {
  return totalItens > 0 ? Math.ceil(totalItens / tamanhoPagina) : 0;
}

interface FavoritosState {
  favoritos: Favorito[];
  carregandoLista: boolean;
  erroLista: string;
  erroAcao: string;
  atualizandoUrls: string[];
}

function mapearRepositoriosParaListagem(
  repositorios: Repositorio[],
  estadoFavoritos: FavoritosState
): RepositorioListItem[] {
  const favoritosPorUrl = new Map(estadoFavoritos.favoritos.map((favorito) => [favorito.htmlUrl, favorito]));

  return repositorios.map((repositorio) => {
    const favorito = favoritosPorUrl.get(repositorio.htmlUrl) ?? null;

    return {
      ...repositorio,
      favoritado: !!favorito,
      favorito,
      atualizandoFavorito: estadoFavoritos.atualizandoUrls.includes(repositorio.htmlUrl)
    };
  });
}
