/**
 * Tipos relacionados ao cadastro de transações no sistema de controle de gastos.
 * 
 * Transações representam movimentações financeiras (receitas ou despesas) associadas
 * a uma pessoa e classificadas por uma categoria.
 */

/**
 * Define o tipo de transação financeira.
 * 
 * - 'despesa': representa uma saída de dinheiro (gasto)
 * - 'receita': representa uma entrada de dinheiro (ganho)
 */
export type TransactionType = 'despesa' | 'receita';

/**
 * Representa uma transação cadastrada no sistema.
 * 
 * @property id - Identificador único gerado automaticamente
 * @property descricao - Descrição da transação (máximo 400 caracteres)
 * @property valor - Valor da transação (deve ser um número positivo)
 * @property tipo - Tipo da transação (despesa ou receita)
 * @property categoriaId - Identificador da categoria associada à transação
 * @property pessoaId - Identificador da pessoa associada à transação
 */
export interface Transaction {
  id: string;
  descricao: string;
  valor: number;
  tipo: TransactionType;
  categoriaId: string;
  pessoaId: string;
}

/**
 * Dados do formulário para criação de transação.
 * 
 * Não inclui o id, pois é gerado automaticamente na criação.
 * 
 * @property descricao - Descrição da transação (máximo 400 caracteres)
 * @property valor - Valor da transação (deve ser um número positivo)
 * @property tipo - Tipo da transação (despesa ou receita)
 * @property categoriaId - Identificador da categoria associada à transação
 * @property pessoaId - Identificador da pessoa associada à transação
 */
export interface TransactionFormData {
  descricao: string;
  valor: number;
  tipo: TransactionType;
  categoriaId: string;
  pessoaId: string;
}
