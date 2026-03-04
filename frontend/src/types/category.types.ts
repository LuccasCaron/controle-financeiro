/**
 * Tipos relacionados ao cadastro de categorias no sistema de controle de gastos.
 * 
 * Categorias são usadas para classificar transações e podem ter diferentes finalidades:
 * - despesa: apenas para transações de despesa
 * - receita: apenas para transações de receita
 * - ambas: pode ser usada tanto para despesas quanto receitas
 */

/**
 * Define a finalidade de uma categoria, ou seja, para quais tipos de transação ela pode ser usada.
 * 
 * - 'despesa': categoria pode ser usada apenas em transações do tipo despesa
 * - 'receita': categoria pode ser usada apenas em transações do tipo receita
 * - 'ambas': categoria pode ser usada tanto em despesas quanto em receitas
 */
export type CategoryFinality = 'despesa' | 'receita' | 'ambas';

/**
 * Representa uma categoria cadastrada no sistema.
 * 
 * @property id - Identificador único gerado automaticamente
 * @property nome - Nome da categoria (máximo 100 caracteres)
 * @property descricao - Descrição da categoria (opcional, máximo 500 caracteres)
 * @property finalidade - Define para quais tipos de transação a categoria pode ser usada
 */
export interface Category {
  id: string;
  nome: string;
  descricao?: string;
  finalidade: CategoryFinality;
}

/**
 * Dados do formulário para criação de categoria.
 * 
 * Não inclui o id, pois é gerado automaticamente na criação.
 * 
 * @property nome - Nome da categoria (máximo 100 caracteres)
 * @property descricao - Descrição da categoria (opcional, máximo 500 caracteres)
 * @property finalidade - Define para quais tipos de transação a categoria pode ser usada
 */
export interface CategoryFormData {
  nome: string;
  descricao?: string;
  finalidade: CategoryFinality;
}
