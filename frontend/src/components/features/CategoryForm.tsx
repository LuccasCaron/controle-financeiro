import { useState, type FormEvent } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { useCategoryContext } from '@/contexts/CategoryContext';
import type { CategoryFormData, CategoryFinality } from '@/types/category.types';

interface CategoryFormProps {
  onSucesso: () => void;
  onCancelar: () => void;
}

export function CategoryForm({ onSucesso, onCancelar }: CategoryFormProps) {
  const { createCategory } = useCategoryContext();

  const [nomeCategoria, setNomeCategoria] = useState('');
  const [descricaoCategoria, setDescricaoCategoria] = useState('');
  const [finalidadeCategoria, setFinalidadeCategoria] = useState<CategoryFinality>('ambas');
  const [mensagemErro, setMensagemErro] = useState<string | null>(null);
  const [estaProcessando, setEstaProcessando] = useState(false);

  const validarFormulario = (): boolean => {
    setMensagemErro(null);

    if (!nomeCategoria.trim()) {
      setMensagemErro('O nome da categoria é obrigatório');
      return false;
    }

    if (nomeCategoria.length > 100) {
      setMensagemErro('O nome da categoria não pode exceder 100 caracteres');
      return false;
    }

    if (descricaoCategoria.length > 500) {
      setMensagemErro('A descrição da categoria não pode exceder 500 caracteres');
      return false;
    }

    return true;
  };

  const handleSubmit = async (evento: FormEvent<HTMLFormElement>) => {
    evento.preventDefault();

    if (!validarFormulario()) {
      return;
    }

    setEstaProcessando(true);
    setMensagemErro(null);

    try {
      const dadosCategoria: CategoryFormData = {
        nome: nomeCategoria.trim(),
        descricao: descricaoCategoria.trim() || undefined,
        finalidade: finalidadeCategoria,
      };

      await createCategory(dadosCategoria);

      onSucesso();
    } catch (erro) {
      setMensagemErro(
        erro instanceof Error ? erro.message : 'Ocorreu um erro ao criar a categoria'
      );
    } finally {
      setEstaProcessando(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="nome-categoria">Nome</Label>
        <Input
          id="nome-categoria"
          type="text"
          value={nomeCategoria}
          onChange={(e) => setNomeCategoria(e.target.value)}
          placeholder="Digite o nome da categoria"
          maxLength={100}
          disabled={estaProcessando}
          required
        />
        <p className="text-xs text-muted-foreground">
          Máximo de 100 caracteres
        </p>
      </div>

      <div className="space-y-2">
        <Label htmlFor="descricao-categoria">Descrição (opcional)</Label>
        <Input
          id="descricao-categoria"
          type="text"
          value={descricaoCategoria}
          onChange={(e) => setDescricaoCategoria(e.target.value)}
          placeholder="Digite a descrição da categoria"
          maxLength={500}
          disabled={estaProcessando}
        />
        <p className="text-xs text-muted-foreground">
          Máximo de 500 caracteres
        </p>
      </div>

      <div className="space-y-2">
        <Label htmlFor="finalidade-categoria">Finalidade</Label>
        <Select
          value={finalidadeCategoria}
          onValueChange={(valor) => setFinalidadeCategoria(valor as CategoryFinality)}
          disabled={estaProcessando}
        >
          <SelectTrigger id="finalidade-categoria">
            <SelectValue placeholder="Selecione a finalidade" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="despesa">Despesa</SelectItem>
            <SelectItem value="receita">Receita</SelectItem>
            <SelectItem value="ambas">Ambas</SelectItem>
          </SelectContent>
        </Select>
        <p className="text-xs text-muted-foreground">
          Define para quais tipos de transação a categoria pode ser usada
        </p>
      </div>

      {mensagemErro && (
        <Alert variant="destructive">
          <AlertDescription>{mensagemErro}</AlertDescription>
        </Alert>
      )}

      <div className="flex justify-end gap-2">
        <Button type="button" variant="outline" onClick={onCancelar} disabled={estaProcessando}>
          Cancelar
        </Button>
        <Button type="submit" disabled={estaProcessando}>
          {estaProcessando ? 'Criando...' : 'Criar'}
        </Button>
      </div>
    </form>
  );
}
