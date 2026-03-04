import { api } from './api';
import type { Transaction, TransactionFormData } from '@/types/transaction.types';

interface CriarTransacaoDto {
  pessoaId: string;
  categoriaId: string;
  tipo: string;
  valor: number;
  data: string;
  descricao?: string;
}

interface TransacaoResponse {
  id: string | unknown;
  pessoaId: string | unknown;
  pessoaNome: string;
  categoriaId: string | unknown;
  categoriaNome: string;
  tipo: string | number;
  valor: number;
  data: string | Date;
  descricao?: string;
}

interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

interface ListarTransacoesResponse {
  success: boolean;
  message: string;
  statusCode: number;
  transacoes: PagedResponse<TransacaoResponse>;
}

interface CriarTransacaoResponse {
  id: string;
}

const mapearTipo = (tipo: string | number): Transaction['tipo'] => {
  if (typeof tipo === 'number') {
    return tipo === 1 ? 'receita' : 'despesa';
  }
  const map: Record<string, Transaction['tipo']> = {
    'Receita': 'receita',
    'Despesa': 'despesa',
    'receita': 'receita',
    'despesa': 'despesa',
  };
  return map[tipo] || 'despesa';
};

const mapearTipoParaBackend = (tipo: Transaction['tipo']): string => {
  return tipo === 'receita' ? 'Receita' : 'Despesa';
};

export const transacaoService = {
  async listar(): Promise<Transaction[]> {
    const response = await api.get<ListarTransacoesResponse>('/Transacoes');
    return response.transacoes.items.map(t => ({
      id: typeof t.id === 'string' ? t.id : String(t.id),
      descricao: t.descricao || '',
      valor: t.valor,
      tipo: mapearTipo(t.tipo),
      categoriaId: typeof t.categoriaId === 'string' ? t.categoriaId : String(t.categoriaId),
      pessoaId: typeof t.pessoaId === 'string' ? t.pessoaId : String(t.pessoaId),
    }));
  },

  async criar(dados: TransactionFormData): Promise<Transaction> {
    const dto: CriarTransacaoDto = {
      pessoaId: dados.pessoaId,
      categoriaId: dados.categoriaId,
      tipo: mapearTipoParaBackend(dados.tipo),
      valor: dados.valor,
      data: new Date().toISOString(),
      descricao: dados.descricao,
    };

    const response = await api.post<CriarTransacaoResponse>('/Transacoes', dto);
    
    return {
      id: typeof response.id === 'string' ? response.id : String(response.id),
      descricao: dados.descricao,
      valor: dados.valor,
      tipo: dados.tipo,
      categoriaId: dados.categoriaId,
      pessoaId: dados.pessoaId,
    };
  },
};
