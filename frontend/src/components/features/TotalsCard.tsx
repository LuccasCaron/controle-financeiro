/**
 * Componente para exibir totais financeiros em formato de card.
 * 
 * Este componente exibe receitas, despesas e saldo de forma visual e organizada,
 * com cores diferentes para valores positivos e negativos.
 */

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { CORES, obterCorSaldo } from '@/utils/colors';

/**
 * Props do componente TotalsCard.
 */
interface TotalsCardProps {
  /** Título do card */
  titulo: string;
  /** Total de receitas */
  totalReceitas: number;
  /** Total de despesas */
  totalDespesas: number;
  /** Saldo (receitas - despesas) */
  saldo: number;
  /** Se true, exibe como card de totais gerais (destaque maior) */
  eTotalGeral?: boolean;
}

/**
 * Formata um valor monetário para exibição.
 * 
 * @param valor - Valor a ser formatado
 * @returns String formatada como moeda brasileira (R$)
 */
function formatarMoeda(valor: number): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(valor);
}

/**
 * Componente para exibir totais financeiros em formato de card.
 * 
 * @param titulo - Título do card
 * @param totalReceitas - Total de receitas
 * @param totalDespesas - Total de despesas
 * @param saldo - Saldo (receitas - despesas)
 * @param eTotalGeral - Se true, exibe como card de totais gerais
 */
export function TotalsCard({
  titulo,
  totalReceitas,
  totalDespesas,
  saldo,
  eTotalGeral = false,
}: TotalsCardProps) {
  // Determina a cor do saldo baseado no valor usando sistema de cores
  const corSaldo = obterCorSaldo(saldo);
  const corBadgeSaldo =
    saldo >= 0
      ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'
      : 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200';

  return (
    <Card>
      <CardHeader>
        <CardTitle className={eTotalGeral ? 'text-xl' : ''}>{titulo}</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4">
        {/* Total de receitas */}
        <div className="flex items-center justify-between">
          <span className="text-sm text-muted-foreground">Total de Receitas:</span>
          <span className={`font-semibold ${CORES.RECEITA}`}>
            {formatarMoeda(totalReceitas)}
          </span>
        </div>

        {/* Total de despesas */}
        <div className="flex items-center justify-between">
          <span className="text-sm text-muted-foreground">Total de Despesas:</span>
          <span className={`font-semibold ${CORES.DESPESA}`}>
            {formatarMoeda(totalDespesas)}
          </span>
        </div>

        {/* Saldo */}
        <div className="flex items-center justify-between pt-2 border-t border-border/20">
          <span className="text-sm font-medium">Saldo:</span>
          <div className="flex items-center gap-2">
            <span className={`font-bold text-lg ${corSaldo}`}>
              {formatarMoeda(saldo)}
            </span>
            <Badge variant="outline" className={corBadgeSaldo}>
              {saldo >= 0 ? 'Positivo' : 'Negativo'}
            </Badge>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}
