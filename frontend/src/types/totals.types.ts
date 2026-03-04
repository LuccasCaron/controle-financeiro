/**
 * Tipos relacionados às consultas de totais no sistema de controle de gastos.
 * 
 * Estes tipos são usados para exibir resumos financeiros agrupados por pessoa
 * ou por categoria, incluindo totais de receitas, despesas e saldo.
 */

import type { Person } from './person.types';
import type { Category } from './category.types';

/**
 * Representa os totais financeiros de uma pessoa específica.
 * 
 * @property pessoa - Dados da pessoa
 * @property totalReceitas - Soma de todas as receitas da pessoa
 * @property totalDespesas - Soma de todas as despesas da pessoa
 * @property saldo - Diferença entre receitas e despesas (receitas - despesas)
 */
export interface PersonTotals {
  pessoa: Person;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

/**
 * Representa os totais financeiros de uma categoria específica.
 * 
 * @property categoria - Dados da categoria
 * @property totalReceitas - Soma de todas as receitas da categoria
 * @property totalDespesas - Soma de todas as despesas da categoria
 * @property saldo - Diferença entre receitas e despesas (receitas - despesas)
 */
export interface CategoryTotals {
  categoria: Category;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}

/**
 * Representa os totais gerais de todas as pessoas ou categorias.
 * 
 * @property totalReceitas - Soma de todas as receitas
 * @property totalDespesas - Soma de todas as despesas
 * @property saldoLiquido - Saldo líquido geral (receitas - despesas)
 */
export interface GeneralTotals {
  totalReceitas: number;
  totalDespesas: number;
  saldoLiquido: number;
}
