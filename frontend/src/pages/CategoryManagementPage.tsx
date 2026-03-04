/**
 * Página de gerenciamento de categorias.
 * 
 * Esta página permite criar e listar categorias do sistema.
 * Categorias são usadas para classificar transações financeiras.
 */

import { useState } from 'react';
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
import { Badge } from '@/components/ui/badge';
import { Card, CardContent } from '@/components/ui/card';
import { CategoryForm } from '@/components/features/CategoryForm';
import { useCategoryContext } from '@/contexts/CategoryContext';
import { obterCorBadgeFinalidade } from '@/utils/colors';
import { Plus } from 'lucide-react';

/**
 * Componente da página de gerenciamento de categorias.
 */
export function CategoryManagementPage() {
  const { categorias } = useCategoryContext();

  // Estado para controle do dialog
  const [dialogAberto, setDialogAberto] = useState(false);

  /**
   * Abre o dialog para criar uma nova categoria.
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
   * Callback chamado quando uma categoria é criada com sucesso.
   */
  const handleSucesso = () => {
    handleFecharDialog();
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
    <div className="space-y-6">
      {/* Cabeçalho */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
        <div>
          <h1 className="text-2xl sm:text-3xl font-bold">Gerenciar Categorias</h1>
          <p className="text-sm sm:text-base text-muted-foreground">
            Crie categorias para classificar suas transações
          </p>
        </div>
        <Button onClick={handleAbrirDialogCriar} className="w-full sm:w-auto">
          <Plus className="w-4 h-4 mr-2" />
          Nova Categoria
        </Button>
      </div>

        {/* Tabela de categorias */}
        {categorias.length === 0 ? (
          <div className="text-center py-12">
            <p className="text-muted-foreground mb-4">Nenhuma categoria cadastrada ainda.</p>
            <Button onClick={handleAbrirDialogCriar}>
              <Plus className="w-4 h-4 mr-2" />
              Criar Primeira Categoria
            </Button>
          </div>
        ) : (
          <>
            {/* Versão Mobile - Cards */}
            <div className="md:hidden space-y-4">
              {categorias.map((categoria) => (
                <Card key={categoria.id}>
                  <CardContent className="p-4">
                    <div className="flex items-center justify-between">
                      <div className="flex-1">
                        <h3 className="font-semibold text-lg">{categoria.nome}</h3>
                        {categoria.descricao && (
                          <p className="text-sm text-muted-foreground mt-1">
                            {categoria.descricao}
                          </p>
                        )}
                      </div>
                      <Badge className={obterCorBadgeFinalidade(categoria.finalidade)}>
                        {formatarFinalidade(categoria.finalidade)}
                      </Badge>
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
                    <TableHead>Finalidade</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {categorias.map((categoria) => (
                    <TableRow key={categoria.id}>
                      <TableCell className="font-medium">{categoria.nome}</TableCell>
                      <TableCell>
                        <Badge className={obterCorBadgeFinalidade(categoria.finalidade)}>
                          {formatarFinalidade(categoria.finalidade)}
                        </Badge>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          </>
        )}

        {/* Dialog para criar categoria */}
        <Dialog open={dialogAberto} onOpenChange={setDialogAberto}>
          <DialogContent className="max-h-[90vh] overflow-y-auto">
            <DialogHeader>
              <DialogTitle>Nova Categoria</DialogTitle>
              <DialogDescription>
                Preencha os dados para criar uma nova categoria.
              </DialogDescription>
            </DialogHeader>
            <CategoryForm onSucesso={handleSucesso} onCancelar={handleFecharDialog} />
          </DialogContent>
        </Dialog>
    </div>
  );
}
