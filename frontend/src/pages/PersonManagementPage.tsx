/**
 * Página de gerenciamento de pessoas (CRUD completo).
 * 
 * Esta página permite criar, listar, editar e deletar pessoas do sistema.
 * Ao deletar uma pessoa, todas as suas transações também são removidas.
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
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from '@/components/ui/alert-dialog';
import { Card, CardContent } from '@/components/ui/card';
import { PersonForm } from '@/components/features/PersonForm';
import { usePersonContext } from '@/contexts/PersonContext';
import { useTransactionContext } from '@/contexts/TransactionContext';
import { useTotalsContext } from '@/contexts/TotalsContext';
import { CORES, obterCorSaldo } from '@/utils/colors';
import type { Person } from '@/types/person.types';
import { Plus, Pencil, Trash2 } from 'lucide-react';

/**
 * Componente da página de gerenciamento de pessoas.
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
  }).format(valor);
}

export function PersonManagementPage() {
  const { pessoas, deletePerson } = usePersonContext();
  const { deleteTransactionsByPersonId } = useTransactionContext();
  const { getPersonTotals } = useTotalsContext();

  // Calcula totais por pessoa para exibir na tabela
  const { totaisPorPessoa } = useMemo(() => getPersonTotals(), [getPersonTotals]);

  /**
   * Busca os totais de uma pessoa específica.
   * 
   * @param pessoaId - ID da pessoa
   * @returns Totais da pessoa ou valores zerados se não encontrada
   */
  const obterTotaisPessoa = (pessoaId: string) => {
    const totalPessoa = totaisPorPessoa.find((tp) => tp.pessoa.id === pessoaId);
    return totalPessoa || {
      totalReceitas: 0,
      totalDespesas: 0,
      saldo: 0,
    };
  };

  // Estado para controle de modais
  const [dialogAberto, setDialogAberto] = useState(false);
  const [dialogConfirmacaoAberto, setDialogConfirmacaoAberto] = useState(false);
  const [pessoaParaEditar, setPessoaParaEditar] = useState<Person | undefined>(undefined);
  const [pessoaParaDeletar, setPessoaParaDeletar] = useState<Person | undefined>(undefined);

  /**
   * Abre o dialog para criar uma nova pessoa.
   */
  const handleAbrirDialogCriar = () => {
    setPessoaParaEditar(undefined);
    setDialogAberto(true);
  };

  /**
   * Abre o dialog para editar uma pessoa existente.
   * 
   * @param pessoa - Pessoa a ser editada
   */
  const handleAbrirDialogEditar = (pessoa: Person) => {
    setPessoaParaEditar(pessoa);
    setDialogAberto(true);
  };

  /**
   * Fecha o dialog e limpa o estado.
   */
  const handleFecharDialog = () => {
    setDialogAberto(false);
    setPessoaParaEditar(undefined);
  };

  /**
   * Callback chamado quando uma pessoa é salva com sucesso.
   */
  const handleSucesso = () => {
    handleFecharDialog();
  };

  /**
   * Abre o dialog de confirmação para deletar uma pessoa.
   * 
   * @param pessoa - Pessoa a ser deletada
   */
  const handleAbrirDialogDeletar = (pessoa: Person) => {
    setPessoaParaDeletar(pessoa);
    setDialogConfirmacaoAberto(true);
  };

  /**
   * Confirma e executa a deleção de uma pessoa.
   * 
   * Remove a pessoa e todas as suas transações relacionadas.
   */
  const handleConfirmarDeletar = () => {
    if (pessoaParaDeletar) {
      // Remove todas as transações da pessoa primeiro
      deleteTransactionsByPersonId(pessoaParaDeletar.id);
      // Remove a pessoa
      deletePerson(pessoaParaDeletar.id);
      setDialogConfirmacaoAberto(false);
      setPessoaParaDeletar(undefined);
    }
  };

  return (
    <div className="space-y-6">
      {/* Cabeçalho */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl sm:text-3xl font-bold">Gerenciar Pessoas</h1>
          <p className="text-sm sm:text-base text-muted-foreground">
            Cadastre, edite e remova pessoas do sistema
          </p>
        </div>
        <Button onClick={handleAbrirDialogCriar} className="w-full sm:w-auto">
          <Plus className="w-4 h-4 mr-2" />
          Nova Pessoa
        </Button>
      </div>

        {/* Tabela de pessoas */}
        {pessoas.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-muted-foreground mb-4">Nenhuma pessoa cadastrada ainda.</p>
            <Button onClick={handleAbrirDialogCriar}>
              <Plus className="w-4 h-4 mr-2" />
              Criar Primeira Pessoa
            </Button>
          </div>
        ) : (
          <>
            {/* Versão Mobile - Cards */}
            <div className="md:hidden space-y-4">
              {pessoas.map((pessoa) => {
                const totais = obterTotaisPessoa(pessoa.id);
                const corSaldo = obterCorSaldo(totais.saldo);

                return (
                  <Card key={pessoa.id}>
                    <CardContent className="p-4">
                      <div className="space-y-3">
                        <div className="flex items-start justify-between">
                          <div>
                            <h3 className="font-semibold text-lg">{pessoa.nome}</h3>
                            <p className="text-sm text-muted-foreground">{pessoa.idade} anos</p>
                          </div>
                          <div className="flex gap-2">
                            <Button
                              variant="outline"
                              size="icon"
                              onClick={() => handleAbrirDialogEditar(pessoa)}
                            >
                              <Pencil className="w-4 h-4" />
                            </Button>
                            <Button
                              variant="destructive"
                              size="icon"
                              onClick={() => handleAbrirDialogDeletar(pessoa)}
                            >
                              <Trash2 className="w-4 h-4" />
                            </Button>
                          </div>
                        </div>
                        <div className="grid grid-cols-2 gap-3 pt-2 border-t">
                          <div>
                            <p className="text-xs text-muted-foreground">Receitas</p>
                            <p className={`text-sm font-semibold ${CORES.RECEITA}`}>
                              {formatarMoeda(totais.totalReceitas)}
                            </p>
                          </div>
                          <div>
                            <p className="text-xs text-muted-foreground">Despesas</p>
                            <p className={`text-sm font-semibold ${CORES.DESPESA}`}>
                              {formatarMoeda(totais.totalDespesas)}
                            </p>
                          </div>
                        </div>
                        <div className="pt-2 border-t">
                          <p className="text-xs text-muted-foreground mb-1">Saldo</p>
                          <p className={`text-lg font-bold ${corSaldo}`}>
                            {formatarMoeda(totais.saldo)}
                          </p>
                        </div>
                      </div>
                    </CardContent>
                  </Card>
                );
              })}
            </div>

            {/* Versão Desktop - Tabela */}
            <div className="hidden md:block border border-border/20 rounded-lg overflow-x-auto">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Nome</TableHead>
                    <TableHead>Idade</TableHead>
                    <TableHead className="text-right">Receitas</TableHead>
                    <TableHead className="text-right">Despesas</TableHead>
                    <TableHead className="text-right">Saldo</TableHead>
                    <TableHead className="text-right">Ações</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {pessoas.map((pessoa) => {
                    const totais = obterTotaisPessoa(pessoa.id);
                    const corSaldo = obterCorSaldo(totais.saldo);

                    return (
                      <TableRow key={pessoa.id}>
                        <TableCell className="font-medium">{pessoa.nome}</TableCell>
                        <TableCell>{pessoa.idade} anos</TableCell>
                        <TableCell className={`text-right font-semibold ${CORES.RECEITA}`}>
                          {formatarMoeda(totais.totalReceitas)}
                        </TableCell>
                        <TableCell className={`text-right font-semibold ${CORES.DESPESA}`}>
                          {formatarMoeda(totais.totalDespesas)}
                        </TableCell>
                        <TableCell className={`text-right font-bold ${corSaldo}`}>
                          {formatarMoeda(totais.saldo)}
                        </TableCell>
                        <TableCell className="text-right">
                          <div className="flex justify-end gap-2">
                            <Button
                              variant="outline"
                              size="icon"
                              onClick={() => handleAbrirDialogEditar(pessoa)}
                            >
                              <Pencil className="w-4 h-4" />
                            </Button>
                            <Button
                              variant="destructive"
                              size="icon"
                              onClick={() => handleAbrirDialogDeletar(pessoa)}
                            >
                              <Trash2 className="w-4 h-4" />
                            </Button>
                          </div>
                        </TableCell>
                      </TableRow>
                    );
                  })}
                </TableBody>
              </Table>
            </div>
          </>
        )}

        {/* Dialog para criar/editar pessoa */}
        <Dialog open={dialogAberto} onOpenChange={setDialogAberto}>
          <DialogContent className="max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>
                {pessoaParaEditar ? 'Editar Pessoa' : 'Nova Pessoa'}
              </DialogTitle>
              <DialogDescription>
                {pessoaParaEditar
                  ? 'Atualize os dados da pessoa abaixo.'
                  : 'Preencha os dados para criar uma nova pessoa.'}
              </DialogDescription>
            </DialogHeader>
            <PersonForm
              pessoaParaEditar={pessoaParaEditar}
              onSucesso={handleSucesso}
              onCancelar={handleFecharDialog}
            />
          </DialogContent>
        </Dialog>

        {/* Dialog de confirmação para deletar */}
        <AlertDialog open={dialogConfirmacaoAberto} onOpenChange={setDialogConfirmacaoAberto}>
          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>Confirmar exclusão</AlertDialogTitle>
              <AlertDialogDescription className="text-left">
                Tem certeza que deseja deletar a pessoa <strong>"{pessoaParaDeletar?.nome}"</strong>?
                <br />
                <br />
                <span className="text-destructive font-medium">
                  Todas as transações desta pessoa também serão removidas.
                </span>
              </AlertDialogDescription>
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel>Cancelar</AlertDialogCancel>
              <AlertDialogAction onClick={handleConfirmarDeletar} className="bg-destructive text-destructive-foreground">
                Deletar
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
    </div>
  );
}
