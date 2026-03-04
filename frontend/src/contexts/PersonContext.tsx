import { createContext, useContext, useState, useCallback, useEffect } from 'react';
import type { ReactNode } from 'react';
import type { Person, PersonFormData } from '@/types/person.types';
import { pessoaService } from '@/services/pessoaService';

interface PersonContextType {
  pessoas: Person[];
  createPerson: (dadosPessoa: PersonFormData) => Promise<Person>;
  updatePerson: (idPessoa: string, dadosPessoa: PersonFormData) => Promise<void>;
  deletePerson: (idPessoa: string) => Promise<void>;
  getAllPersons: () => Person[];
  getPersonById: (idPessoa: string) => Person | undefined;
  carregarPessoas: () => Promise<void>;
}

const PersonContext = createContext<PersonContextType | undefined>(undefined);

interface PersonProviderProps {
  children: ReactNode;
}

export function PersonProvider({ children }: PersonProviderProps) {
  const [pessoas, setPessoas] = useState<Person[]>([]);

  const carregarPessoas = useCallback(async () => {
    try {
      const pessoasCarregadas = await pessoaService.listar();
      setPessoas(pessoasCarregadas);
    } catch (error) {
      console.error('Erro ao carregar pessoas:', error);
    }
  }, []);

  useEffect(() => {
    carregarPessoas();
  }, [carregarPessoas]);

  const createPerson = useCallback(async (dadosPessoa: PersonFormData): Promise<Person> => {
    const novaPessoa = await pessoaService.criar(dadosPessoa);
    setPessoas((pessoasAnteriores) => [...pessoasAnteriores, novaPessoa]);
    return novaPessoa;
  }, []);

  const updatePerson = useCallback(async (idPessoa: string, dadosPessoa: PersonFormData): Promise<void> => {
    await pessoaService.atualizar(idPessoa, dadosPessoa);
    await carregarPessoas();
  }, [carregarPessoas]);

  const deletePerson = useCallback(async (idPessoa: string): Promise<void> => {
    await pessoaService.excluir(idPessoa);
    setPessoas((pessoasAnteriores) => pessoasAnteriores.filter((p) => p.id !== idPessoa));
  }, []);

  const getAllPersons = useCallback((): Person[] => {
    return pessoas;
  }, [pessoas]);

  const getPersonById = useCallback((idPessoa: string): Person | undefined => {
    return pessoas.find((pessoa) => pessoa.id === idPessoa);
  }, [pessoas]);

  const valorContexto: PersonContextType = {
    pessoas,
    createPerson,
    updatePerson,
    deletePerson,
    getAllPersons,
    getPersonById,
    carregarPessoas,
  };

  return (
    <PersonContext.Provider value={valorContexto}>
      {children}
    </PersonContext.Provider>
  );
}

export function usePersonContext(): PersonContextType {
  const context = useContext(PersonContext);
  if (!context) {
    throw new Error('usePersonContext deve ser usado dentro de PersonProvider');
  }
  return context;
}
