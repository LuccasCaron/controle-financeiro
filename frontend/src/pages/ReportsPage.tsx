/**
 * Página unificada de relatórios com abas.
 * 
 * Esta página exibe relatórios financeiros organizados em abas:
 * - Por Pessoa: totais individuais e gerais por pessoa
 * - Por Categoria: totais individuais e gerais por categoria
 */

import { useSearchParams } from 'react-router-dom';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Badge } from '@/components/ui/badge';
import { Card, CardContent } from '@/components/ui/card';
import { TotalsCard } from '@/components/features/TotalsCard';
import { useTotalsContext } from '@/contexts/TotalsContext';
import { CORES, obterCorSaldo, obterCorBadgeFinalidade } from '@/utils/colors';

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
 * Componente da página de relatórios unificada.
 */
export function ReportsPage() {
  const { getPersonTotals, getCategoryTotals } = useTotalsContext();
  const [searchParams, setSearchParams] = useSearchParams();

  // Determina a aba ativa baseada na query string ou padrão
  const abaAtiva = searchParams.get('tab') || 'pessoas';

  /**
   * Manipula a mudança de aba.
   * 
   * @param valor - Valor da aba selecionada
   */
  const handleMudarAba = (valor: string) => {
    setSearchParams({ tab: valor });
  };

  // Calcula os totais
  const { totaisPorPessoa, totaisGerais: totaisGeraisPessoas } = getPersonTotals();
  const { totaisPorCategoria, totaisGerais: totaisGeraisCategorias } = getCategoryTotals();

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
    <div className="space-y-6">
      {/* Cabeçalho */}
      <div>
        <h1 className="text-2xl sm:text-3xl font-bold">Relatórios</h1>
        <p className="text-sm sm:text-base text-muted-foreground">
          Visualize resumos financeiros por pessoa ou por categoria
        </p>
      </div>

      {/* Abas de relatórios */}
      <Tabs value={abaAtiva} onValueChange={handleMudarAba} className="space-y-6">
        <TabsList>
          <TabsTrigger value="pessoas">Por Pessoa</TabsTrigger>
          <TabsTrigger value="categorias">Por Categoria</TabsTrigger>
        </TabsList>

        {/* Aba: Totais por Pessoa */}
        <TabsContent value="pessoas" className="space-y-6">
          {totaisPorPessoa.length === 0 ? (
            <div className="text-center py-12">
              <p className="text-muted-foreground">
                Nenhuma pessoa cadastrada ainda. Cadastre pessoas e transações para ver os totais.
              </p>
            </div>
          ) : (
            <>
              <div className="md:hidden space-y-4">
                {totaisPorPessoa.map((totalPessoa) => {
                  const corSaldo = obterCorSaldo(totalPessoa.saldo);

                  return (
                    <Card key={totalPessoa.pessoa.id}>
                      <CardContent className="p-4">
                        <div className="space-y-3">
                          <div className="flex items-start justify-between">
                            <div className="flex-1">
                              <h3 className="font-semibold text-lg">{totalPessoa.pessoa.nome}</h3>
                              <p className="text-sm text-muted-foreground mt-1">
                                {totalPessoa.pessoa.idade} anos
                              </p>
                            </div>
                          </div>
                          <div className="space-y-2 pt-2 border-t">
                            <div className="flex items-center justify-between">
                              <p className="text-sm text-muted-foreground">Total Receitas</p>
                              <p className={`text-sm font-medium ${CORES.RECEITA}`}>
                                {formatarMoeda(totalPessoa.totalReceitas)}
                              </p>
                            </div>
                            <div className="flex items-center justify-between">
                              <p className="text-sm text-muted-foreground">Total Despesas</p>
                              <p className={`text-sm font-medium ${CORES.DESPESA}`}>
                                {formatarMoeda(totalPessoa.totalDespesas)}
                              </p>
                            </div>
                            <div className="flex items-center justify-between pt-2 border-t">
                              <p className="text-sm font-semibold">Saldo</p>
                              <p className={`text-lg font-bold ${corSaldo}`}>
                                {formatarMoeda(totalPessoa.saldo)}
                              </p>
                            </div>
                          </div>
                        </div>
                      </CardContent>
                    </Card>
                  );
                })}
              </div>

              <div className="hidden md:block border border-border/20 rounded-lg overflow-x-auto">
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
                      const corSaldo = obterCorSaldo(totalPessoa.saldo);

                      return (
                        <TableRow key={totalPessoa.pessoa.id}>
                          <TableCell className="font-medium">
                            {totalPessoa.pessoa.nome}
                          </TableCell>
                          <TableCell>{totalPessoa.pessoa.idade} anos</TableCell>
                          <TableCell className={`text-right ${CORES.RECEITA}`}>
                            {formatarMoeda(totalPessoa.totalReceitas)}
                          </TableCell>
                          <TableCell className={`text-right ${CORES.DESPESA}`}>
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
                totalReceitas={totaisGeraisPessoas.totalReceitas}
                totalDespesas={totaisGeraisPessoas.totalDespesas}
                saldo={totaisGeraisPessoas.saldoLiquido}
                eTotalGeral
              />
            </>
          )}
        </TabsContent>

        {/* Aba: Totais por Categoria */}
        <TabsContent value="categorias" className="space-y-6">
          {totaisPorCategoria.length === 0 ? (
            <div className="text-center py-12">
              <p className="text-muted-foreground">
                Nenhuma categoria cadastrada ainda. Cadastre categorias e transações para ver os totais.
              </p>
            </div>
          ) : (
            <>
              <div className="md:hidden space-y-4">
                {totaisPorCategoria.map((totalCategoria) => {
                  const corSaldo = obterCorSaldo(totalCategoria.saldo);

                  return (
                    <Card key={totalCategoria.categoria.id}>
                      <CardContent className="p-4">
                        <div className="space-y-3">
                          <div className="flex items-start justify-between">
                            <div className="flex-1">
                              <h3 className="font-semibold text-lg">{totalCategoria.categoria.nome}</h3>
                              <div className="mt-2">
                                <Badge
                                  className={obterCorBadgeFinalidade(totalCategoria.categoria.finalidade)}
                                >
                                  {formatarFinalidade(totalCategoria.categoria.finalidade)}
                                </Badge>
                              </div>
                            </div>
                          </div>
                          <div className="space-y-2 pt-2 border-t">
                            <div className="flex items-center justify-between">
                              <p className="text-sm text-muted-foreground">Total Receitas</p>
                              <p className={`text-sm font-medium ${CORES.RECEITA}`}>
                                {formatarMoeda(totalCategoria.totalReceitas)}
                              </p>
                            </div>
                            <div className="flex items-center justify-between">
                              <p className="text-sm text-muted-foreground">Total Despesas</p>
                              <p className={`text-sm font-medium ${CORES.DESPESA}`}>
                                {formatarMoeda(totalCategoria.totalDespesas)}
                              </p>
                            </div>
                            <div className="flex items-center justify-between pt-2 border-t">
                              <p className="text-sm font-semibold">Saldo</p>
                              <p className={`text-lg font-bold ${corSaldo}`}>
                                {formatarMoeda(totalCategoria.saldo)}
                              </p>
                            </div>
                          </div>
                        </div>
                      </CardContent>
                    </Card>
                  );
                })}
              </div>

              <div className="hidden md:block border border-border/20 rounded-lg overflow-x-auto">
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
                      const corSaldo = obterCorSaldo(totalCategoria.saldo);

                      return (
                        <TableRow key={totalCategoria.categoria.id}>
                          <TableCell className="font-medium">
                            {totalCategoria.categoria.nome}
                          </TableCell>
                          <TableCell>
                            <Badge
                              className={obterCorBadgeFinalidade(totalCategoria.categoria.finalidade)}
                            >
                              {formatarFinalidade(totalCategoria.categoria.finalidade)}
                            </Badge>
                          </TableCell>
                          <TableCell className={`text-right ${CORES.RECEITA}`}>
                            {formatarMoeda(totalCategoria.totalReceitas)}
                          </TableCell>
                          <TableCell className={`text-right ${CORES.DESPESA}`}>
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
                totalReceitas={totaisGeraisCategorias.totalReceitas}
                totalDespesas={totaisGeraisCategorias.totalDespesas}
                saldo={totaisGeraisCategorias.saldoLiquido}
                eTotalGeral
              />
            </>
          )}
        </TabsContent>
      </Tabs>
    </div>
  );
}
