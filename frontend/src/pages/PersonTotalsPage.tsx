/**
 * Página de consulta de totais por pessoa.
 * 
 * Esta página exibe um resumo financeiro de cada pessoa cadastrada,
 * incluindo totais de receitas, despesas e saldo individual.
 * Também exibe os totais gerais de todas as pessoas.
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
import { TotalsCard } from '@/components/features/TotalsCard';
import { useTotalsContext } from '@/contexts/TotalsContext';
import { ArrowLeft } from 'lucide-react';

/**
 * Componente da página de totais por pessoa.
 */
export function PersonTotalsPage() {
  const navigate = useNavigate();
  const { getPersonTotals } = useTotalsContext();

  // Calcula os totais
  const { totaisPorPessoa, totaisGerais } = getPersonTotals();

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

  return (
    <div className="min-h-screen bg-background p-4 md:p-8">
      <div className="max-w-6xl mx-auto space-y-6">
        {/* Cabeçalho */}
        <div className="flex items-center gap-4">
          <Button variant="ghost" size="icon" onClick={() => navigate('/')}>
            <ArrowLeft className="w-5 h-5" />
          </Button>
          <div>
            <h1 className="text-3xl font-bold">Totais por Pessoa</h1>
            <p className="text-muted-foreground">
              Visualize o resumo financeiro de cada pessoa
            </p>
          </div>
        </div>

        {/* Tabela de totais por pessoa */}
        {totaisPorPessoa.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-muted-foreground">
              Nenhuma pessoa cadastrada ainda. Cadastre pessoas e transações para ver os totais.
            </p>
          </div>
        ) : (
          <>
            <div className="border rounded-lg">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Pessoa</TableHead>
                    <TableHead>Idade</TableHead>
                    <TableHead className="text-right">Total Receitas</TableHead>
                    <TableHead className="text-right">Total Despesas</TableHead>
                    <TableHead className="text-right">Saldo</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {totaisPorPessoa.map((totalPessoa) => {
                    const corSaldo =
                      totalPessoa.saldo >= 0
                        ? 'text-green-600 dark:text-green-400'
                        : 'text-red-600 dark:text-red-400';

                    return (
                      <TableRow key={totalPessoa.pessoa.id}>
                        <TableCell className="font-medium">
                          {totalPessoa.pessoa.nome}
                        </TableCell>
                        <TableCell>{totalPessoa.pessoa.idade} anos</TableCell>
                        <TableCell className="text-right text-green-600 dark:text-green-400">
                          {formatarMoeda(totalPessoa.totalReceitas)}
                        </TableCell>
                        <TableCell className="text-right text-red-600 dark:text-red-400">
                          {formatarMoeda(totalPessoa.totalDespesas)}
                        </TableCell>
                        <TableCell className={`text-right font-bold ${corSaldo}`}>
                          {formatarMoeda(totalPessoa.saldo)}
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
