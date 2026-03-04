/**
 * Página de gerenciamento de transações.
 * 
 * Esta página permite criar e listar transações financeiras (receitas e despesas).
 * Inclui validações dinâmicas baseadas em regras de negócio.
 */

import { useState, useMemo } from 'react';
import { Button } from '@/components/ui/button';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/dialog';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Badge } from '@/components/ui/badge';
import { Card, CardContent } from '@/components/ui/card';
import { TransactionForm } from '@/components/features/TransactionForm';
import { useTransactionContext } from '@/contexts/TransactionContext';
import { usePersonContext } from '@/contexts/PersonContext';
import { useCategoryContext } from '@/contexts/CategoryContext';
import { CORES } from '@/utils/colors';
import { Plus } from 'lucide-react';

/**
 * Componente da página de gerenciamento de transações.
 */
export function TransactionManagementPage() {
  const { transacoes } = useTransactionContext();
  const { getAllPersons } = usePersonContext();
  const { getAllCategories } = useCategoryContext();

  // Estado para controle do dialog
  const [dialogAberto, setDialogAberto] = useState(false);

  // Estado para filtros
  const [filtroPessoaId, setFiltroPessoaId] = useState<string>('todas');
  const [filtroTipo, setFiltroTipo] = useState<string>('todos');
  const [filtroCategoriaId, setFiltroCategoriaId] = useState<string>('todas');

  // Listas para filtros
  const todasPessoas = getAllPersons();
  const todasCategorias = getAllCategories();

  /**
   * Filtra as transações baseado nos filtros selecionados.
   */
  const transacoesFiltradas = useMemo(() => {
    return transacoes.filter((transacao) => {
      // Filtro por pessoa
      if (filtroPessoaId !== 'todas' && transacao.pessoaId !== filtroPessoaId) {
        return false;
      }

      // Filtro por tipo
      if (filtroTipo !== 'todos' && transacao.tipo !== filtroTipo) {
        return false;
      }

      // Filtro por categoria
      if (filtroCategoriaId !== 'todas' && transacao.categoriaId !== filtroCategoriaId) {
        return false;
      }

      return true;
    });
  }, [transacoes, filtroPessoaId, filtroTipo, filtroCategoriaId]);

  /**
   * Abre o dialog para criar uma nova transação.
   */
  const handleAbrirDialogCriar = () => {
    setDialogAberto(true);
  };

  /**
   * Fecha o dialog.
   */
  const handleFecharDialog = () => {
    setDialogAberto(false);
  };

  /**
   * Callback chamado quando uma transação é criada com sucesso.
   */
  const handleSucesso = () => {
    handleFecharDialog();
  };

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
   * Busca o nome de uma pessoa pelo ID.
   * 
   * @param pessoaId - ID da pessoa
   * @returns Nome da pessoa ou 'Desconhecida'
   */
  const obterNomePessoa = (pessoaId: string): string => {
    const pessoa = todasPessoas.find((p) => p.id === pessoaId);
    return pessoa?.nome || 'Desconhecida';
  };

  /**
   * Busca a descrição de uma categoria pelo ID.
   * 
   * @param categoriaId - ID da categoria
   * @returns Descrição da categoria ou 'Desconhecida'
   */
  const obterDescricaoCategoria = (categoriaId: string): string => {
    const categoria = todasCategorias.find((c) => c.id === categoriaId);
    return categoria?.nome || 'Desconhecida';
  };

  return (
    <div className="space-y-6">
      {/* Cabeçalho */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl sm:text-3xl font-bold">Gerenciar Transações</h1>
          <p className="text-sm sm:text-base text-muted-foreground">
            Registre receitas e despesas do dia a dia
          </p>
        </div>
        <Button onClick={handleAbrirDialogCriar} className="w-full sm:w-auto">
          <Plus className="w-4 h-4 mr-2" />
          Nova Transação
        </Button>
      </div>

        {/* Filtros */}
        <div className="flex flex-wrap gap-4 p-4 border border-border/20 rounded-lg bg-muted/50">
          <div className="flex-1 min-w-[200px]">
            <Select value={filtroPessoaId} onValueChange={setFiltroPessoaId}>
              <SelectTrigger>
                <SelectValue placeholder="Filtrar por pessoa" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="todas">Todas as pessoas</SelectItem>
                {todasPessoas.map((pessoa) => (
                  <SelectItem key={pessoa.id} value={pessoa.id}>
                    {pessoa.nome}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
          <div className="flex-1 min-w-[200px]">
            <Select value={filtroTipo} onValueChange={setFiltroTipo}>
              <SelectTrigger>
                <SelectValue placeholder="Filtrar por tipo" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="todos">Todos os tipos</SelectItem>
                <SelectItem value="receita">Receita</SelectItem>
                <SelectItem value="despesa">Despesa</SelectItem>
              </SelectContent>
            </Select>
          </div>
          <div className="flex-1 min-w-[200px]">
            <Select value={filtroCategoriaId} onValueChange={setFiltroCategoriaId}>
              <SelectTrigger>
                <SelectValue placeholder="Filtrar por categoria" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="todas">Todas as categorias</SelectItem>
                {todasCategorias.map((categoria) => (
                  <SelectItem key={categoria.id} value={categoria.id}>
                    {categoria.nome}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>
        </div>

        {/* Tabela de transações */}
        {transacoesFiltradas.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-muted-foreground mb-4">
              {transacoes.length === 0
                ? 'Nenhuma transação cadastrada ainda.'
                : 'Nenhuma transação encontrada com os filtros selecionados.'}
            </p>
            <Button onClick={handleAbrirDialogCriar}>
              <Plus className="w-4 h-4 mr-2" />
              Criar Primeira Transação
            </Button>
          </div>
        ) : (
          <>
            {/* Versão Mobile - Cards */}
            <div className="md:hidden space-y-4">
              {transacoesFiltradas.map((transacao) => (
                <Card key={transacao.id}>
                  <CardContent className="p-4">
                    <div className="space-y-3">
                      <div className="flex items-start justify-between">
                        <div className="flex-1">
                          <h3 className="font-semibold text-lg">{transacao.descricao}</h3>
                          <p className="text-sm text-muted-foreground mt-1">
                            {obterNomePessoa(transacao.pessoaId)}
                          </p>
                        </div>
                        <Badge
                          variant="outline"
                          className={
                            transacao.tipo === 'receita'
                              ? CORES.BADGE_RECEITA
                              : CORES.BADGE_DESPESA
                          }
                        >
                          {transacao.tipo === 'receita' ? 'Receita' : 'Despesa'}
                        </Badge>
                      </div>
                      <div className="flex items-center justify-between pt-2 border-t">
                        <div>
                          <p className="text-xs text-muted-foreground">Categoria</p>
                          <p className="text-sm font-medium">{obterDescricaoCategoria(transacao.categoriaId)}</p>
                        </div>
                        <div className="text-right">
                          <p className="text-xs text-muted-foreground">Valor</p>
                          <p className="text-lg font-bold">{formatarMoeda(transacao.valor)}</p>
                        </div>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              ))}
            </div>

            {/* Versão Desktop - Tabela */}
            <div className="hidden md:block border border-border/20 rounded-lg overflow-x-auto">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Descrição</TableHead>
                    <TableHead>Pessoa</TableHead>
                    <TableHead>Categoria</TableHead>
                    <TableHead>Tipo</TableHead>
                    <TableHead className="text-right">Valor</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {transacoesFiltradas.map((transacao) => (
                    <TableRow key={transacao.id}>
                      <TableCell className="font-medium">{transacao.descricao}</TableCell>
                      <TableCell>{obterNomePessoa(transacao.pessoaId)}</TableCell>
                      <TableCell>{obterDescricaoCategoria(transacao.categoriaId)}</TableCell>
                      <TableCell>
                        <Badge
                          variant="outline"
                          className={
                            transacao.tipo === 'receita'
                              ? CORES.BADGE_RECEITA
                              : CORES.BADGE_DESPESA
                          }
                        >
                          {transacao.tipo === 'receita' ? 'Receita' : 'Despesa'}
                        </Badge>
                      </TableCell>
                      <TableCell className="text-right font-semibold">
                        {formatarMoeda(transacao.valor)}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          </>
        )}

        {/* Dialog para criar transação */}
        <Dialog open={dialogAberto} onOpenChange={setDialogAberto}>
          <DialogContent className="max-w-2xl max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>Nova Transação</DialogTitle>
              <DialogDescription>
                Preencha os dados para criar uma nova transação.
              </DialogDescription>
            </DialogHeader>
            <TransactionForm onSucesso={handleSucesso} onCancelar={handleFecharDialog} />
          </DialogContent>
        </Dialog>
    </div>
  );
}
