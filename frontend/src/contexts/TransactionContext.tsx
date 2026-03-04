import { createContext, useContext, useState, useCallback } from 'react';
import type { ReactNode } from 'react';
import type { Transaction, TransactionFormData } from '@/types/transaction.types';
import type { Person } from '@/types/person.types';
import type { Category } from '@/types/category.types';

interface TransactionContextType {
  transacoes: Transaction[];
  createTransaction: (dadosTransacao: TransactionFormData) => Transaction;
  getAllTransactions: () => Transaction[];
  getTransactionById: (idTransacao: string) => Transaction | undefined;
  getTransactionsByPersonId: (idPessoa: string) => Transaction[];
  deleteTransactionsByPersonId: (idPessoa: string) => void;
}

const TransactionContext = createContext<TransactionContextType | undefined>(undefined);

interface TransactionProviderProps {
  children: ReactNode;
  getPersonById: (idPessoa: string) => Person | undefined;
  getCategoryById: (idCategoria: string) => Category | undefined;
}

export function TransactionProvider({
  children,
  getPersonById,
  getCategoryById,
}: TransactionProviderProps) {
  const [transacoes, setTransacoes] = useState<Transaction[]>([]);

  const gerarIdUnico = useCallback((): string => {
    return `transacao-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`;
  }, []);

  const validarDadosTransacao = useCallback(
    (dadosTransacao: TransactionFormData): void => {
      if (!dadosTransacao.descricao || dadosTransacao.descricao.trim().length === 0) {
        throw new Error('A descrição da transação é obrigatória');
      }

      if (dadosTransacao.descricao.length > 400) {
        throw new Error('A descrição da transação não pode exceder 400 caracteres');
      }

      if (dadosTransacao.valor <= 0) {
        throw new Error('O valor da transação deve ser um número positivo');
      }

      const pessoa = getPersonById(dadosTransacao.pessoaId);
      if (!pessoa) {
        throw new Error('Pessoa não encontrada');
      }

      if (pessoa.idade < 18 && dadosTransacao.tipo === 'receita') {
        throw new Error('Menores de 18 anos só podem cadastrar despesas');
      }

      const categoria = getCategoryById(dadosTransacao.categoriaId);
      if (!categoria) {
        throw new Error('Categoria não encontrada');
      }

      if (categoria.finalidade !== 'ambas' && categoria.finalidade !== dadosTransacao.tipo) {
        throw new Error(
          `Esta categoria só pode ser usada para transações do tipo "${categoria.finalidade}"`
        );
      }
    },
    [getPersonById, getCategoryById]
  );

  const createTransaction = useCallback(
    (dadosTransacao: TransactionFormData): Transaction => {
      validarDadosTransacao(dadosTransacao);

      const novaTransacao: Transaction = {
        id: gerarIdUnico(),
        descricao: dadosTransacao.descricao.trim(),
        valor: dadosTransacao.valor,
        tipo: dadosTransacao.tipo,
        categoriaId: dadosTransacao.categoriaId,
        pessoaId: dadosTransacao.pessoaId,
      };

      setTransacoes((transacoesAnteriores) => [...transacoesAnteriores, novaTransacao]);

      return novaTransacao;
    },
    [gerarIdUnico, validarDadosTransacao]
  );

  const getAllTransactions = useCallback((): Transaction[] => {
    return transacoes;
  }, [transacoes]);

  const getTransactionById = useCallback(
    (idTransacao: string): Transaction | undefined => {
      return transacoes.find((transacao) => transacao.id === idTransacao);
    },
    [transacoes]
  );

  const getTransactionsByPersonId = useCallback(
    (idPessoa: string): Transaction[] => {
      return transacoes.filter((transacao) => transacao.pessoaId === idPessoa);
    },
    [transacoes]
  );

  const deleteTransactionsByPersonId = useCallback((idPessoa: string): void => {
    setTransacoes((transacoesAnteriores) =>
      transacoesAnteriores.filter((transacao) => transacao.pessoaId !== idPessoa)
    );
  }, []);

  const valorContexto: TransactionContextType = {
    transacoes,
    createTransaction,
    getAllTransactions,
    getTransactionById,
    getTransactionsByPersonId,
    deleteTransactionsByPersonId,
  };

  return (
    <TransactionContext.Provider value={valorContexto}>
      {children}
    </TransactionContext.Provider>
  );
}

export function useTransactionContext(): TransactionContextType {
  const contexto = useContext(TransactionContext);

  if (contexto === undefined) {
    throw new Error('useTransactionContext deve ser usado dentro de um TransactionProvider');
  }

  return contexto;
}
