/**
 * Página de onboarding (boas-vindas) do sistema de controle de gastos.
 * 
 * Esta página apresenta um dashboard com informações gerais do sistema
 * e estatísticas financeiras, além de navegação rápida para as principais áreas.
 */

import { useNavigate } from 'react-router-dom';
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { TotalsCard } from '@/components/features/TotalsCard';
import { usePersonContext } from '@/contexts/PersonContext';
import { useCategoryContext } from '@/contexts/CategoryContext';
import { useTransactionContext } from '@/contexts/TransactionContext';
import { useTotalsContext } from '@/contexts/TotalsContext';
import { Users, Tag, Receipt, BarChart3, TrendingUp, DollarSign } from 'lucide-react';

/**
 * Componente da página de onboarding.
 * 
 * Exibe informações sobre o sistema e botões de navegação para as funcionalidades principais.
 */
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
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(valor);
}

export function OnboardingPage() {
  const navigate = useNavigate();
  const { pessoas } = usePersonContext();
  const { categorias } = useCategoryContext();
  const { transacoes } = useTransactionContext();
  const { getPersonTotals } = useTotalsContext();

  // Calcula totais gerais
  const { totaisGerais } = getPersonTotals();

  /**
   * Opções de navegação rápida disponíveis no sistema.
   */
  const opcoesNavegacao = [
    {
      titulo: 'Pessoas',
      icone: Users,
      rota: '/pessoas',
      cor: 'bg-blue-500/10 text-blue-600 dark:text-blue-400',
      valor: pessoas.length,
      label: 'cadastradas',
    },
    {
      titulo: 'Categorias',
      icone: Tag,
      rota: '/categorias',
      cor: 'bg-purple-500/10 text-purple-600 dark:text-purple-400',
      valor: categorias.length,
      label: 'criadas',
    },
    {
      titulo: 'Transações',
      icone: Receipt,
      rota: '/transacoes',
      cor: 'bg-green-500/10 text-green-600 dark:text-green-400',
      valor: transacoes.length,
      label: 'registradas',
    },
    {
      titulo: 'Relatórios',
      icone: BarChart3,
      rota: '/relatorios',
      cor: 'bg-orange-500/10 text-orange-600 dark:text-orange-400',
      valor: null,
      label: 'visualizar',
    },
  ];

  return (
    <div className="space-y-8">
      {/* Cabeçalho */}
      <div className="text-center space-y-4">
        <h1 className="text-3xl sm:text-4xl md:text-5xl font-bold bg-gradient-to-r from-primary to-primary/60 bg-clip-text text-transparent">
          Dashboard
        </h1>
        <p className="text-muted-foreground text-sm sm:text-base">
          Visão geral do sistema de controle de gastos
        </p>
      </div>

      {/* Cards de estatísticas rápidas */}
      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        {opcoesNavegacao.map((opcao) => {
          const Icone = opcao.icone;
          return (
            <Card
              key={opcao.rota}
              className="hover:shadow-md transition-all cursor-pointer hover:scale-105"
              onClick={() => navigate(opcao.rota)}
            >
              <CardContent className="p-6">
                <div className="flex items-center justify-between">
                  <div>
                    <p className="text-sm text-muted-foreground mb-1">{opcao.titulo}</p>
                    <p className="text-2xl font-bold">
                      {opcao.valor !== null ? opcao.valor : '—'}
                    </p>
                    <p className="text-xs text-muted-foreground mt-1">{opcao.label}</p>
                  </div>
                  <div className={`${opcao.cor} p-3 rounded-lg`}>
                    <Icone className="w-5 h-5" />
                  </div>
                </div>
              </CardContent>
            </Card>
          );
        })}
      </div>

      {/* Resumo financeiro geral */}
      {transacoes.length > 0 ? (
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <TotalsCard
            titulo="Resumo Financeiro Geral"
            totalReceitas={totaisGerais.totalReceitas}
            totalDespesas={totaisGerais.totalDespesas}
            saldo={totaisGerais.saldoLiquido}
            eTotalGeral
          />

          {/* Card com informações adicionais */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <TrendingUp className="w-5 h-5" />
                Informações
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="space-y-3">
                <div className="flex items-center justify-between">
                  <span className="text-sm text-muted-foreground">Total de Transações:</span>
                  <span className="font-semibold">{transacoes.length}</span>
                </div>
                <div className="flex items-center justify-between">
                  <span className="text-sm text-muted-foreground">Receitas:</span>
                  <span className={`font-semibold ${transacoes.filter(t => t.tipo === 'receita').length > 0 ? 'text-green-600 dark:text-green-400' : ''}`}>
                    {transacoes.filter(t => t.tipo === 'receita').length}
                  </span>
                </div>
                <div className="flex items-center justify-between">
                  <span className="text-sm text-muted-foreground">Despesas:</span>
                  <span className={`font-semibold ${transacoes.filter(t => t.tipo === 'despesa').length > 0 ? 'text-red-600 dark:text-red-400' : ''}`}>
                    {transacoes.filter(t => t.tipo === 'despesa').length}
                  </span>
                </div>
                <div className="flex items-center justify-between pt-2 border-t border-border/20">
                  <span className="text-sm text-muted-foreground">Média por Transação:</span>
                  <span className="font-semibold">
                    {transacoes.length > 0
                      ? formatarMoeda(
                          transacoes.reduce((acc, t) => acc + t.valor, 0) / transacoes.length
                        )
                      : 'R$ 0,00'}
                  </span>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>
      ) : (
        /* Estado vazio - quando não há transações */
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
          <Card className="lg:col-span-2">
            <CardContent className="p-12 text-center">
              <DollarSign className="w-16 h-16 mx-auto mb-4 text-muted-foreground opacity-50" />
              <h3 className="text-xl font-semibold mb-2">Bem-vindo ao Sistema!</h3>
              <p className="text-muted-foreground mb-4 max-w-md mx-auto">
                Comece cadastrando pessoas, criando categorias e registrando suas primeiras transações.
                O dashboard será atualizado automaticamente com suas informações.
              </p>
              <div className="text-sm text-muted-foreground space-y-1">
                <p>• Cadastrar Pessoas</p>
                <p>• Criar Categorias</p>
                <p>• Registrar Transações</p>
              </div>
            </CardContent>
          </Card>
        </div>
      )}

      {/* Acesso rápido aos relatórios */}
      {transacoes.length > 0 && (
        <Card className="bg-gradient-to-r from-primary/5 to-primary/10 border-primary/20">
          <CardContent className="p-6">
            <div className="flex flex-col sm:flex-row items-center justify-between gap-4">
              <div>
                <h3 className="font-semibold text-lg mb-1">Relatórios Detalhados</h3>
                <p className="text-sm text-muted-foreground">
                  Visualize análises completas por pessoa ou categoria
                </p>
              </div>
              <Button onClick={() => navigate('/relatorios')} size="lg">
                <BarChart3 className="w-4 h-4 mr-2" />
                Ver Relatórios
              </Button>
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
