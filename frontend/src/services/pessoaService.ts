import { api } from './api';
import type { Person, PersonFormData } from '@/types/person.types';

interface CriarPessoaDto {
  nome: string;
  cpf: string;
  dataNascimento: string;
}

interface AtualizarPessoaDto {
  nome: string;
  dataNascimento: string;
}

interface PessoaResponse {
  id: string | unknown;
  nome: string;
  cpf: string;
  dataNascimento: string | Date;
  idade: number;
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

interface ListarPessoasResponse {
  pessoas: PagedResponse<PessoaResponse>;
  success: boolean;
  message: string;
  statusCode: number;
}

interface CriarPessoaResponse {
  id: string | unknown;
}

export const pessoaService = {
  async listar(): Promise<Person[]> {
    const response = await api.get<ListarPessoasResponse>('/Pessoas');
    return response.pessoas.items.map(p => ({
      id: typeof p.id === 'string' ? p.id : String(p.id),
      nome: p.nome,
      idade: p.idade,
      cpf: p.cpf,
      dataNascimento: typeof p.dataNascimento === 'string' ? p.dataNascimento : new Date(p.dataNascimento).toISOString().split('T')[0],
    }));
  },

  async obterPorId(id: string): Promise<Person | null> {
    try {
      const response = await api.get<{ pessoa: PessoaResponse }>(`/Pessoas/${id}`);
      const p = response.pessoa;
      return {
        id: typeof p.id === 'string' ? p.id : String(p.id),
        nome: p.nome,
        idade: p.idade,
        cpf: p.cpf,
        dataNascimento: typeof p.dataNascimento === 'string' ? p.dataNascimento : new Date(p.dataNascimento).toISOString().split('T')[0],
      };
    } catch (error) {
      return null;
    }
  },

  async criar(dados: PersonFormData): Promise<Person> {
    const dto: CriarPessoaDto = {
      nome: dados.nome,
      cpf: dados.cpf,
      dataNascimento: dados.dataNascimento,
    };

    const response = await api.post<CriarPessoaResponse>('/Pessoas', dto);
    
    // Calcula idade a partir da data de nascimento
    const dataNasc = new Date(dados.dataNascimento);
    const hoje = new Date();
    let idade = hoje.getFullYear() - dataNasc.getFullYear();
    const mes = hoje.getMonth() - dataNasc.getMonth();
    if (mes < 0 || (mes === 0 && hoje.getDate() < dataNasc.getDate())) {
      idade--;
    }
    
    return {
      id: typeof response.id === 'string' ? response.id : String(response.id),
      nome: dados.nome,
      idade,
      cpf: dados.cpf,
      dataNascimento: dados.dataNascimento,
    };
  },

  async atualizar(id: string, dados: PersonFormData): Promise<void> {
    const dto: AtualizarPessoaDto = {
      nome: dados.nome,
      dataNascimento: dados.dataNascimento,
    };

    await api.put(`/Pessoas/${id}`, dto);
  },

  async excluir(id: string): Promise<void> {
    await api.delete(`/Pessoas/${id}`);
  },
};
