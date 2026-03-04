/**
 * Página de consulta de totais por categoria.
 * 
 * Esta página exibe um resumo financeiro de cada categoria cadastrada,
 * incluindo totais de receitas, despesas e saldo por categoria.
 * Também exibe os totais gerais de todas as categorias.
 */

import { useNavigate } from 'react-router-dom';
import { Button } from '@/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { TotalsCard } from '@/components/features/TotalsCard';
import { useTotalsContext } from '@/contexts/TotalsContext';
import { ArrowLeft } from 'lucide-react';

/**
 * Componente da página de totais por categoria.
 */
export function CategoryTotalsPage() {
  const navigate = useNavigate();
  const { getCategoryTotals } = useTotalsContext();

  // Calcula os totais
  const { totaisPorCategoria, totaisGerais } = getCategoryTotals();

  /**
   * Formata um valor monetário para exibição.
   * 
   * @param valor - Valor a ser formatado
   * @returns String formatada como moeda brasileira (R$)
   */
  const formatarMoeda = (valor: number): string => {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
    }).format(valor);
  };

  /**
   * Retorna a cor do badge baseado na finalidade da categoria.
   * 
   * @param finalidade - Finalidade da categoria
   * @returns Classe CSS para o badge
   */
  const obterCorBadge = (finalidade: string) => {
    switch (finalidade) {
      case 'despesa':
        return 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200';
      case 'receita':
        return 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200';
      case 'ambas':
        return 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200';
      default:
        return '';
    }
  };

  /**
   * Retorna o texto formatado da finalidade.
   * 
   * @param finalidade - Finalidade da categoria
   * @returns Texto formatado
   */
  const formatarFinalidade = (finalidade: string) => {
    return finalidade.charAt(0).toUpperCase() + finalidade.slice(1);
  };

  return (
    <div className="min-h-screen bg-background p-4 md:p-8">
      <div className="max-w-6xl mx-auto space-y-6">
        {/* Cabeçalho */}
        <div className="flex items-center gap-4">
          <Button variant="ghost" size="icon" onClick={() => navigate('/')}>
            <ArrowLeft className="w-5 h-5" />
          </Button>
          <div>
            <h1 className="text-3xl font-bold">Totais por Categoria</h1>
            <p className="text-muted-foreground">
              Analise os gastos e receitas por categoria
            </p>
          </div>
        </div>

        {/* Tabela de totais por categoria */}
        {totaisPorCategoria.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-muted-foreground">
              Nenhuma categoria cadastrada ainda. Cadastre categorias e transações para ver os totais.
            </p>
          </div>
        ) : (
          <>
            <div className="border rounded-lg">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Categoria</TableHead>
                    <TableHead>Finalidade</TableHead>
                    <TableHead className="text-right">Total Receitas</TableHead>
                    <TableHead className="text-right">Total Despesas</TableHead>
                    <TableHead className="text-right">Saldo</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {totaisPorCategoria.map((totalCategoria) => {
                    const corSaldo =
                      totalCategoria.saldo >= 0
                        ? 'text-green-600 dark:text-green-400'
                        : 'text-red-600 dark:text-red-400';

                    return (
                      <TableRow key={totalCategoria.categoria.id}>
                        <TableCell className="font-medium">
                          {totalCategoria.categoria.nome}
                        </TableCell>
                        <TableCell>
                          <Badge className={obterCorBadge(totalCategoria.categoria.finalidade)}>
                            {formatarFinalidade(totalCategoria.categoria.finalidade)}
                          </Badge>
                        </TableCell>
                        <TableCell className="text-right text-green-600 dark:text-green-400">
                          {formatarMoeda(totalCategoria.totalReceitas)}
                        </TableCell>
                        <TableCell className="text-right text-red-600 dark:text-red-400">
                          {formatarMoeda(totalCategoria.totalDespesas)}
                        </TableCell>
                        <TableCell className={`text-right font-bold ${corSaldo}`}>
                          {formatarMoeda(totalCategoria.saldo)}
                        </TableCell>
                      </TableRow>
                    );
                  })}
                </TableBody>
              </Table>
            </div>

            {/* Card com totais gerais */}
            <TotalsCard
              titulo="Totais Gerais"
              totalReceitas={totaisGerais.totalReceitas}
              totalDespesas={totaisGerais.totalDespesas}
              saldo={totaisGerais.saldoLiquido}
              eTotalGeral
            />
          </>
        )}
      </div>
    </div>
  );
}
