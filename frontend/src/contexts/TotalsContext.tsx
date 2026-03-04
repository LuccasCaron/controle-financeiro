/**
 * Context API para cálculos de totais no sistema de controle de gastos.
 * 
 * Este contexto fornece funções para calcular totais financeiros agrupados
 * por pessoa ou por categoria, incluindo totais gerais.
 */

import { createContext, useContext, useMemo } from 'react';
import type { ReactNode } from 'react';
import type { PersonTotals, CategoryTotals, GeneralTotals } from '@/types/totals.types';
import { usePersonContext } from './PersonContext';
import { useCategoryContext } from './CategoryContext';
import { useTransactionContext } from './TransactionContext';

/**
 * Interface que define a estrutura do contexto de totais.
 */
interface TotalsContextType {
  /** Calcula e retorna totais agrupados por pessoa */
  getPersonTotals: () => {
    totaisPorPessoa: PersonTotals[];
    totaisGerais: GeneralTotals;
  };
  /** Calcula e retorna totais agrupados por categoria */
  getCategoryTotals: () => {
    totaisPorCategoria: CategoryTotals[];
    totaisGerais: GeneralTotals;
  };
}

/**
 * Contexto React para cálculos de totais.
 * Inicializado como undefined e será preenchido pelo Provider.
 */
const TotalsContext = createContext<TotalsContextType | undefined>(undefined);

/**
 * Props do Provider de totais.
 */
interface TotalsProviderProps {
  /** Componentes filhos que terão acesso ao contexto */
  children: ReactNode;
}

/**
 * Provider do contexto de totais.
 * 
 * Fornece funções para calcular totais financeiros baseados nos dados
 * dos contexts de Person, Category e Transaction.
 * 
 * @param children - Componentes filhos que terão acesso ao contexto
 */
export function TotalsProvider({ children }: TotalsProviderProps) {
  // Acessa os contexts necessários para cálculos
  const { pessoas } = usePersonContext();
  const { categorias } = useCategoryContext();
  const { transacoes } = useTransactionContext();

  /**
   * Calcula totais financeiros agrupados por pessoa.
   * 
   * Para cada pessoa, calcula:
   * - Total de receitas (soma de todas as transações do tipo "receita")
   * - Total de despesas (soma de todas as transações do tipo "despesa")
   * - Saldo (receitas - despesas)
   * 
   * Também calcula os totais gerais de todas as pessoas.
   * 
   * @returns Objeto contendo totais por pessoa e totais gerais
   */
  const getPersonTotals = useMemo(() => {
    return (): {
      totaisPorPessoa: PersonTotals[];
      totaisGerais: GeneralTotals;
    } => {
      // Calcula totais para cada pessoa
      const totaisPorPessoa: PersonTotals[] = pessoas.map((pessoa) => {
        // Filtra transações da pessoa
        const transacoesDaPessoa = transacoes.filter(
          (transacao) => transacao.pessoaId === pessoa.id
        );

        // Calcula total de receitas (transações do tipo "receita")
        const totalReceitas = transacoesDaPessoa
          .filter((transacao) => transacao.tipo === 'receita')
          .reduce((soma, transacao) => soma + transacao.valor, 0);

        // Calcula total de despesas (transações do tipo "despesa")
        const totalDespesas = transacoesDaPessoa
          .filter((transacao) => transacao.tipo === 'despesa')
          .reduce((soma, transacao) => soma + transacao.valor, 0);

        // Calcula saldo (receitas - despesas)
        const saldo = totalReceitas - totalDespesas;

        return {
          pessoa,
          totalReceitas,
          totalDespesas,
          saldo,
        };
      });

      // Calcula totais gerais (soma de todas as pessoas)
      const totaisGerais: GeneralTotals = totaisPorPessoa.reduce(
        (acumulador, totalPessoa) => ({
          totalReceitas: acumulador.totalReceitas + totalPessoa.totalReceitas,
          totalDespesas: acumulador.totalDespesas + totalPessoa.totalDespesas,
          saldoLiquido:
            acumulador.saldoLiquido + (totalPessoa.totalReceitas - totalPessoa.totalDespesas),
        }),
        { totalReceitas: 0, totalDespesas: 0, saldoLiquido: 0 }
      );

      return {
        totaisPorPessoa,
        totaisGerais,
      };
    };
  }, [pessoas, transacoes]);

  /**
   * Calcula totais financeiros agrupados por categoria.
   * 
   * Para cada categoria, calcula:
   * - Total de receitas (soma de todas as transações do tipo "receita")
   * - Total de despesas (soma de todas as transações do tipo "despesa")
   * - Saldo (receitas - despesas)
   * 
   * Também calcula os totais gerais de todas as categorias.
   * 
   * @returns Objeto contendo totais por categoria e totais gerais
   */
  const getCategoryTotals = useMemo(() => {
    return (): {
      totaisPorCategoria: CategoryTotals[];
      totaisGerais: GeneralTotals;
    } => {
      // Calcula totais para cada categoria
      const totaisPorCategoria: CategoryTotals[] = categorias.map((categoria) => {
        // Filtra transações da categoria
        const transacoesDaCategoria = transacoes.filter(
          (transacao) => transacao.categoriaId === categoria.id
        );

        // Calcula total de receitas (transações do tipo "receita")
        const totalReceitas = transacoesDaCategoria
          .filter((transacao) => transacao.tipo === 'receita')
          .reduce((soma, transacao) => soma + transacao.valor, 0);

        // Calcula total de despesas (transações do tipo "despesa")
        const totalDespesas = transacoesDaCategoria
          .filter((transacao) => transacao.tipo === 'despesa')
          .reduce((soma, transacao) => soma + transacao.valor, 0);

        // Calcula saldo (receitas - despesas)
        const saldo = totalReceitas - totalDespesas;

        return {
          categoria,
          totalReceitas,
          totalDespesas,
          saldo,
        };
      });

      // Calcula totais gerais (soma de todas as categorias)
      const totaisGerais: GeneralTotals = totaisPorCategoria.reduce(
        (acumulador, totalCategoria) => ({
          totalReceitas: acumulador.totalReceitas + totalCategoria.totalReceitas,
          totalDespesas: acumulador.totalDespesas + totalCategoria.totalDespesas,
          saldoLiquido:
            acumulador.saldoLiquido +
            (totalCategoria.totalReceitas - totalCategoria.totalDespesas),
        }),
        { totalReceitas: 0, totalDespesas: 0, saldoLiquido: 0 }
      );

      return {
        totaisPorCategoria,
        totaisGerais,
      };
    };
  }, [categorias, transacoes]);

  // Valor do contexto com todas as funções
  const valorContexto: TotalsContextType = {
    getPersonTotals,
    getCategoryTotals,
  };

  return (
    <TotalsContext.Provider value={valorContexto}>
      {children}
    </TotalsContext.Provider>
  );
}

/**
 * Hook customizado para acessar o contexto de totais.
 * 
 * @returns O contexto de totais
 * @throws Error se usado fora do TotalsProvider
 */
export function useTotalsContext(): TotalsContextType {
  const contexto = useContext(TotalsContext);

  if (contexto === undefined) {
    throw new Error('useTotalsContext deve ser usado dentro de um TotalsProvider');
  }

  return contexto;
}
