/**
 * Sistema de cores consistente para o sistema de controle de gastos.
 * 
 * Define cores padronizadas para receitas, despesas e saldos,
 * garantindo consistência visual em toda a aplicação.
 */

/**
 * Constantes de cores para uso em toda a aplicação.
 * 
 * Receitas são sempre verdes, despesas são sempre vermelhas,
 * e saldos seguem a mesma lógica (positivo = verde, negativo = vermelho).
 */
export const CORES = {
  /** Cor de texto para receitas (verde) */
  RECEITA: 'text-green-600 dark:text-green-400',
  /** Cor de texto para despesas (vermelho) */
  DESPESA: 'text-red-600 dark:text-red-400',
  /** Cor de texto para saldo positivo (verde) */
  SALDO_POSITIVO: 'text-green-600 dark:text-green-400',
  /** Cor de texto para saldo negativo (vermelho) */
  SALDO_NEGATIVO: 'text-red-600 dark:text-red-400',
  /** Estilo de badge para receita (fundo verde claro) */
  BADGE_RECEITA: 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200',
  /** Estilo de badge para despesa (fundo vermelho claro) */
  BADGE_DESPESA: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200',
  /** Estilo de badge para categoria que aceita ambas (fundo azul claro) */
  BADGE_AMBAS: 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200',
} as const;

/**
 * Retorna a cor apropriada para um saldo baseado no valor.
 * 
 * @param saldo - Valor do saldo (positivo ou negativo)
 * @returns Classe CSS para cor do saldo
 */
export function obterCorSaldo(saldo: number): string {
  return saldo >= 0 ? CORES.SALDO_POSITIVO : CORES.SALDO_NEGATIVO;
}

/**
 * Retorna o estilo de badge apropriado para uma finalidade de categoria.
 * 
 * @param finalidade - Finalidade da categoria ('despesa', 'receita' ou 'ambas')
 * @returns Classe CSS para badge
 */
export function obterCorBadgeFinalidade(
  finalidade: 'despesa' | 'receita' | 'ambas'
): string {
  switch (finalidade) {
    case 'despesa':
      return CORES.BADGE_DESPESA;
    case 'receita':
      return CORES.BADGE_RECEITA;
    case 'ambas':
      return CORES.BADGE_AMBAS;
    default:
      return '';
  }
}
