import { Injectable, inject } from '@angular/core';
import { Observable, Subject, catchError, concatMap, map, merge, of, scan, share, shareReplay, startWith, switchMap } from 'rxjs';
import { Favorito } from '../models/favorito.model';
import { FavoritoRequest } from '../models/favorito-request.model';
import { Repositorio } from '../models/repositorio.model';
import { FavoritosApiService } from './favoritos-api.service';

const MENSAGEM_SEM_FAVORITOS = 'Nenhum favorito adicionado até o momento.';
const MENSAGEM_ERRO_FAVORITOS = 'Não foi possível carregar os favoritos agora. Tente novamente em instantes.';
const MENSAGEM_ERRO_FAVORITAR = 'Não foi possível favoritar o repositório agora. Tente novamente.';
const MENSAGEM_ERRO_DESFAVORITAR = 'Não foi possível desfavoritar o repositório agora. Tente novamente.';

export interface FavoritosState {
  favoritos: Favorito[];
  carregandoLista: boolean;
  erroLista: string;
  erroAcao: string;
  atualizandoUrls: string[];
}

type FavoritosReducer = (state: FavoritosState) => FavoritosState;

const ESTADO_INICIAL_FAVORITOS: FavoritosState = {
  favoritos: [],
  carregandoLista: false,
  erroLista: '',
  erroAcao: '',
  atualizandoUrls: []
};

@Injectable({
  providedIn: 'root'
})
export class FavoritosFacadeService {
  private readonly favoritosApiService = inject(FavoritosApiService);
  private readonly carregarFavoritosSubject = new Subject<void>();
  private readonly favoritarSubject = new Subject<Repositorio>();
  private readonly desfavoritarSubject = new Subject<Favorito>();

  readonly state$ = criarEstadoFavoritos(
    this.favoritosApiService,
    this.carregarFavoritosSubject.asObservable(),
    this.favoritarSubject.asObservable(),
    this.desfavoritarSubject.asObservable()
  );

  carregarFavoritos(): void {
    this.carregarFavoritosSubject.next();
  }

  observarPaginaFavoritos(): Observable<FavoritosState> {
    return new Observable<FavoritosState>((subscriber) => {
      const subscription = this.state$.subscribe(subscriber);
      this.carregarFavoritos();

      return () => subscription.unsubscribe();
    });
  }

  favoritar(repositorio: Repositorio): void {
    this.favoritarSubject.next(repositorio);
  }

  desfavoritar(favorito: Favorito): void {
    this.desfavoritarSubject.next(favorito);
  }

  obterMensagemVazia(state: FavoritosState): string {
    if (state.erroLista || state.carregandoLista || state.favoritos.length > 0) {
      return '';
    }

    return MENSAGEM_SEM_FAVORITOS;
  }
}

function criarEstadoFavoritos(
  favoritosApiService: FavoritosApiService,
  carregarFavoritos$: Observable<void>,
  favoritar$: Observable<Repositorio>,
  desfavoritar$: Observable<Favorito>
) {
  const favoritarResultado$ = favoritar$.pipe(
    concatMap((repositorio) =>
      favoritosApiService.adicionarFavorito(mapearFavoritoRequest(repositorio)).pipe(
        map(() => ({ sucesso: true as const, repositorio })),
        catchError(() => of({ sucesso: false as const, repositorio }))
      )
    ),
    share()
  );

  const desfavoritarResultado$ = desfavoritar$.pipe(
    concatMap((favorito) =>
      favoritosApiService.removerFavorito(favorito.id).pipe(
        map(() => ({ sucesso: true as const, favorito })),
        catchError(() => of({ sucesso: false as const, favorito }))
      )
    ),
    share()
  );

  const carregarFavoritosReducer$ = carregarFavoritos$.pipe(
    switchMap(() =>
      favoritosApiService.listarFavoritos().pipe(
        map((favoritos) => criarReducerSucessoCarregamentoFavoritos(favoritos)),
        startWith(criarReducerCarregamentoFavoritos()),
        catchError(() => of(criarReducerErroCarregamentoFavoritos()))
      )
    )
  );

  return merge(
    favoritar$.pipe(map((repositorio) => criarReducerAdicaoOtimista(repositorio))),
    favoritarResultado$.pipe(map((resultado) => resultado.sucesso
      ? criarReducerFinalizacaoAtualizacao(resultado.repositorio.htmlUrl)
      : criarReducerErroAdicao(resultado.repositorio)
    )),
    desfavoritar$.pipe(map((favorito) => criarReducerRemocaoOtimista(favorito))),
    desfavoritarResultado$.pipe(map((resultado) => resultado.sucesso
      ? criarReducerFinalizacaoAtualizacao(resultado.favorito.htmlUrl)
      : criarReducerErroRemocao(resultado.favorito)
    )),
    carregarFavoritosReducer$
  ).pipe(
    scan((state, reducer) => reducer(state), ESTADO_INICIAL_FAVORITOS),
    startWith(ESTADO_INICIAL_FAVORITOS),
    shareReplay({ bufferSize: 1, refCount: true })
  );
}

function mapearFavoritoRequest(repositorio: Repositorio): FavoritoRequest {
  return {
    nome: repositorio.nome,
    htmlUrl: repositorio.htmlUrl,
    quantidadeEstrelas: repositorio.quantidadeEstrelas,
    quantidadeForks: repositorio.quantidadeForks,
    quantidadeObservadores: repositorio.quantidadeObservadores
  };
}

function criarReducerAdicaoOtimista(repositorio: Repositorio): FavoritosReducer {
  return (state) => {
    const favoritos = state.favoritos.some((favorito) => favorito.htmlUrl === repositorio.htmlUrl)
      ? state.favoritos
      : [...state.favoritos, criarFavoritoTemporario(repositorio)];

    return {
      ...state,
      favoritos,
      erroAcao: '',
      atualizandoUrls: adicionarUrlAtualizando(state.atualizandoUrls, repositorio.htmlUrl)
    };
  };
}

function criarReducerErroAdicao(repositorio: Repositorio): FavoritosReducer {
  return (state) => ({
    ...state,
    favoritos: state.favoritos.filter((favorito) => favorito.htmlUrl !== repositorio.htmlUrl),
    erroAcao: MENSAGEM_ERRO_FAVORITAR,
    atualizandoUrls: removerUrlAtualizando(state.atualizandoUrls, repositorio.htmlUrl)
  });
}

function criarReducerRemocaoOtimista(favorito: Favorito): FavoritosReducer {
  return (state) => ({
    ...state,
    favoritos: state.favoritos.filter((item) => item.htmlUrl !== favorito.htmlUrl),
    erroAcao: '',
    atualizandoUrls: adicionarUrlAtualizando(state.atualizandoUrls, favorito.htmlUrl)
  });
}

function criarReducerErroRemocao(favorito: Favorito): FavoritosReducer {
  return (state) => ({
    ...state,
    favoritos: normalizarFavoritos([...state.favoritos, favorito]),
    erroAcao: MENSAGEM_ERRO_DESFAVORITAR,
    atualizandoUrls: removerUrlAtualizando(state.atualizandoUrls, favorito.htmlUrl)
  });
}

function criarReducerFinalizacaoAtualizacao(htmlUrl: string): FavoritosReducer {
  return (state) => ({
    ...state,
    erroAcao: '',
    atualizandoUrls: removerUrlAtualizando(state.atualizandoUrls, htmlUrl)
  });
}

function criarReducerCarregamentoFavoritos(): FavoritosReducer {
  return (state) => ({
    ...state,
    carregandoLista: true,
    erroLista: ''
  });
}

function criarReducerSucessoCarregamentoFavoritos(favoritos: Favorito[]): FavoritosReducer {
  return (state) => ({
    ...state,
    favoritos: normalizarFavoritos(favoritos),
    carregandoLista: false,
    erroLista: '',
    erroAcao: ''
  });
}

function criarReducerErroCarregamentoFavoritos(): FavoritosReducer {
  return (state) => ({
    ...state,
    carregandoLista: false,
    erroLista: MENSAGEM_ERRO_FAVORITOS
  });
}

function criarFavoritoTemporario(repositorio: Repositorio): Favorito {
  return {
    id: `temp-${repositorio.htmlUrl}`,
    nome: repositorio.nome,
    htmlUrl: repositorio.htmlUrl
  };
}

function adicionarUrlAtualizando(urls: string[], htmlUrl: string): string[] {
  return urls.includes(htmlUrl) ? urls : [...urls, htmlUrl];
}

function removerUrlAtualizando(urls: string[], htmlUrl: string): string[] {
  return urls.filter((url) => url !== htmlUrl);
}

function normalizarFavoritos(favoritos: Favorito[]): Favorito[] {
  const favoritosPorUrl = new Map<string, Favorito>();

  for (const favorito of favoritos) {
    favoritosPorUrl.set(favorito.htmlUrl, favorito);
  }

  return Array.from(favoritosPorUrl.values());
}
