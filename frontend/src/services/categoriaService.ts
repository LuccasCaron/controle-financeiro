import { api } from './api';
import type { Category, CategoryFormData } from '@/types/category.types';

interface CriarCategoriaDto {
  nome: string;
  descricao: string | null;
  finalidade: string;
}

interface CategoriaResponse {
  id: string | unknown;
  nome: string;
  descricao: string;
  finalidade: string | number;
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

interface ListarCategoriasResponse {
  success: boolean;
  message: string;
  statusCode: number;
  categorias: PagedResponse<CategoriaResponse>;
}

interface CriarCategoriaResponse {
  id: string | unknown;
}

const mapearFinalidade = (finalidade: string | number): Category['finalidade'] => {
  if (typeof finalidade === 'number') {
    const map: Record<number, Category['finalidade']> = {
      1: 'receita',
      2: 'despesa',
      3: 'ambas',
    };
    return map[finalidade] || 'ambas';
  }
  
  const map: Record<string, Category['finalidade']> = {
    'Receita': 'receita',
    'Despesa': 'despesa',
    'Ambas': 'ambas',
    'receita': 'receita',
    'despesa': 'despesa',
    'ambas': 'ambas',
  };
  return map[finalidade] || 'ambas';
};

const mapearFinalidadeParaBackend = (finalidade: Category['finalidade']): string => {
  const map: Record<Category['finalidade'], string> = {
    'receita': 'Receita',
    'despesa': 'Despesa',
    'ambas': 'Ambas',
  };
  return map[finalidade];
};

export const categoriaService = {
  async listar(): Promise<Category[]> {
    const response = await api.get<ListarCategoriasResponse>('/Categorias');
    return response.categorias.items.map(c => ({
      id: typeof c.id === 'string' ? c.id : String(c.id),
      nome: c.nome,
      descricao: c.descricao,
      finalidade: mapearFinalidade(c.finalidade),
    }));
  },

  async criar(dados: CategoryFormData): Promise<Category> {
    const dto: CriarCategoriaDto = {
      nome: dados.nome,
      descricao: dados.descricao || null,
      finalidade: mapearFinalidadeParaBackend(dados.finalidade),
    };

    const response = await api.post<CriarCategoriaResponse>('/Categorias', dto);
    
    return {
      id: typeof response.id === 'string' ? response.id : String(response.id),
      nome: dados.nome,
      descricao: dados.descricao,
      finalidade: dados.finalidade,
    };
  },
};
