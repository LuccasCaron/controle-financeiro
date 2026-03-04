/**
 * Tipos relacionados ao cadastro de pessoas no sistema de controle de gastos.
 * 
 * Uma pessoa representa um membro da família/residência que pode ter transações
 * financeiras associadas (receitas e despesas).
 */

/**
 * Representa uma pessoa cadastrada no sistema.
 * 
 * @property id - Identificador único gerado automaticamente
 * @property nome - Nome da pessoa (máximo 200 caracteres)
 * @property idade - Idade da pessoa (deve ser um número positivo)
 */
export interface Person {
  id: string;
  nome: string;
  idade: number;
  cpf?: string;
  dataNascimento?: string;
}

/**
 * Dados do formulário para criação ou edição de pessoa.
 * 
 * Não inclui o id, pois é gerado automaticamente na criação.
 * 
 * @property nome - Nome da pessoa (máximo 100 caracteres)
 * @property cpf - CPF da pessoa (11 dígitos numéricos)
 * @property dataNascimento - Data de nascimento no formato YYYY-MM-DD
 */
export interface PersonFormData {
  nome: string;
  cpf: string;
  dataNascimento: string;
}
