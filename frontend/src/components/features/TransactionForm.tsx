import { useState, useEffect, type FormEvent } from 'react';
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
import { useTransactionContext } from '@/contexts/TransactionContext';
import { usePersonContext } from '@/contexts/PersonContext';
import { useCategoryContext } from '@/contexts/CategoryContext';
import type { TransactionFormData, TransactionType } from '@/types/transaction.types';
import type { Person } from '@/types/person.types';
import type { Category } from '@/types/category.types';

interface TransactionFormProps {
  onSucesso: () => void;
  onCancelar: () => void;
}

export function TransactionForm({ onSucesso, onCancelar }: TransactionFormProps) {
  const { createTransaction } = useTransactionContext();
  const { getAllPersons } = usePersonContext();
  const { getCategoriesByFinality } = useCategoryContext();

  const [descricaoTransacao, setDescricaoTransacao] = useState('');
  const [valorFormatado, setValorFormatado] = useState('');
  const [tipoTransacao, setTipoTransacao] = useState<TransactionType>('despesa');
  const [categoriaIdSelecionada, setCategoriaIdSelecionada] = useState<string>('');
  const [pessoaIdSelecionada, setPessoaIdSelecionada] = useState<string>('');
  const [mensagemErro, setMensagemErro] = useState<string | null>(null);
  const [estaProcessando, setEstaProcessando] = useState(false);

  const [pessoaSelecionada, setPessoaSelecionada] = useState<Person | undefined>(undefined);
  const [categoriasDisponiveis, setCategoriasDisponiveis] = useState<Category[]>([]);

  const todasPessoas = getAllPersons();

  useEffect(() => {
    const categoriasFiltradas = getCategoriesByFinality(tipoTransacao);
    setCategoriasDisponiveis(categoriasFiltradas);

    if (
      categoriaIdSelecionada &&
      !categoriasFiltradas.some((cat) => cat.id === categoriaIdSelecionada)
    ) {
      setCategoriaIdSelecionada('');
    }
  }, [tipoTransacao, categoriaIdSelecionada, getCategoriesByFinality]);

  useEffect(() => {
    const pessoa = todasPessoas.find((p) => p.id === pessoaIdSelecionada);
    setPessoaSelecionada(pessoa);

    if (pessoa && pessoa.idade < 18 && tipoTransacao === 'receita') {
      setTipoTransacao('despesa');
      setMensagemErro('Menores de 18 anos só podem cadastrar despesas');
    } else {
      setMensagemErro(null);
    }
  }, [pessoaIdSelecionada, todasPessoas, tipoTransacao]);

  const validarFormulario = (): boolean => {
    setMensagemErro(null);

    if (!descricaoTransacao.trim()) {
      setMensagemErro('A descrição da transação é obrigatória');
      return false;
    }

    if (descricaoTransacao.length > 400) {
      setMensagemErro('A descrição da transação não pode exceder 400 caracteres');
      return false;
    }

    const valorNumerico = converterMoedaParaNumero(valorFormatado);
    if (isNaN(valorNumerico) || valorNumerico <= 0) {
      setMensagemErro('O valor da transação deve ser um número positivo');
      return false;
    }

    if (!pessoaIdSelecionada) {
      setMensagemErro('Selecione uma pessoa');
      return false;
    }

    if (pessoaSelecionada && pessoaSelecionada.idade < 18 && tipoTransacao === 'receita') {
      setMensagemErro('Menores de 18 anos só podem cadastrar despesas');
      return false;
    }

    if (!categoriaIdSelecionada) {
      setMensagemErro('Selecione uma categoria');
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
      const valorNumerico = converterMoedaParaNumero(valorFormatado);
      const dadosTransacao: TransactionFormData = {
        descricao: descricaoTransacao.trim(),
        valor: valorNumerico,
        tipo: tipoTransacao,
        categoriaId: categoriaIdSelecionada,
        pessoaId: pessoaIdSelecionada,
      };

      createTransaction(dadosTransacao);

      onSucesso();
    } catch (erro) {
      setMensagemErro(
        erro instanceof Error ? erro.message : 'Ocorreu um erro ao criar a transação'
      );
    } finally {
      setEstaProcessando(false);
    }
  };

  const formatarMoeda = (valor: string): string => {
    const apenasNumeros = valor.replace(/\D/g, '');
    if (apenasNumeros === '') return '';
    
    const valorNumerico = parseFloat(apenasNumeros) / 100;
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    }).format(valorNumerico);
  };

  const converterMoedaParaNumero = (valorFormatado: string): number => {
    const apenasNumeros = valorFormatado.replace(/\D/g, '');
    if (apenasNumeros === '') return 0;
    return parseFloat(apenasNumeros) / 100;
  };

  const handleValorChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const valor = e.target.value;
    const apenasNumeros = valor.replace(/\D/g, '');
    
    setValorFormatado(formatarMoeda(apenasNumeros));
  };

  const pessoaMenorDeIdade = pessoaSelecionada && pessoaSelecionada.idade < 18;

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <div className="space-y-2">
        <Label htmlFor="descricao-transacao">Descrição</Label>
        <Input
          id="descricao-transacao"
          type="text"
          value={descricaoTransacao}
          onChange={(e) => setDescricaoTransacao(e.target.value)}
          placeholder="Digite a descrição da transação"
          maxLength={400}
          disabled={estaProcessando}
          required
        />
        <p className="text-xs text-muted-foreground">
          Máximo de 400 caracteres
        </p>
      </div>

      <div className="space-y-2">
        <Label htmlFor="valor-transacao">Valor</Label>
        <Input
          id="valor-transacao"
          type="text"
          value={valorFormatado}
          onChange={handleValorChange}
          placeholder="R$ 0,00"
          disabled={estaProcessando}
          required
        />
        <p className="text-xs text-muted-foreground">
          Digite o valor em reais (R$)
        </p>
      </div>

      <div className="space-y-2">
        <Label htmlFor="tipo-transacao">Tipo</Label>
        <Select
          value={tipoTransacao}
          onValueChange={(valor) => setTipoTransacao(valor as TransactionType)}
          disabled={estaProcessando || pessoaMenorDeIdade}
        >
          <SelectTrigger id="tipo-transacao">
            <SelectValue placeholder="Selecione o tipo" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="despesa">Despesa</SelectItem>
            <SelectItem value="receita" disabled={pessoaMenorDeIdade}>
              Receita
            </SelectItem>
          </SelectContent>
        </Select>
        {pessoaMenorDeIdade && (
          <p className="text-xs text-amber-500">
            Menores de 18 anos só podem cadastrar despesas
          </p>
        )}
      </div>

      <div className="space-y-2">
        <Label htmlFor="pessoa-transacao">Pessoa</Label>
        <Select
          value={pessoaIdSelecionada}
          onValueChange={setPessoaIdSelecionada}
          disabled={estaProcessando}
        >
          <SelectTrigger id="pessoa-transacao">
            <SelectValue placeholder="Selecione uma pessoa" />
          </SelectTrigger>
          <SelectContent>
            {todasPessoas.map((pessoa) => (
              <SelectItem key={pessoa.id} value={pessoa.id}>
                {pessoa.nome} ({pessoa.idade} anos)
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
      </div>

      <div className="space-y-2">
        <Label htmlFor="categoria-transacao">Categoria</Label>
        <Select
          value={categoriaIdSelecionada}
          onValueChange={setCategoriaIdSelecionada}
          disabled={estaProcessando || categoriasDisponiveis.length === 0}
        >
          <SelectTrigger id="categoria-transacao">
            <SelectValue placeholder="Selecione uma categoria" />
          </SelectTrigger>
          <SelectContent>
            {categoriasDisponiveis.map((categoria) => (
              <SelectItem key={categoria.id} value={categoria.id}>
                {categoria.nome}
              </SelectItem>
            ))}
          </SelectContent>
        </Select>
        {categoriasDisponiveis.length === 0 && (
          <p className="text-xs text-amber-500">
            Nenhuma categoria disponível para este tipo de transação
          </p>
        )}
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
        <Button type="submit" disabled={estaProcessando || categoriasDisponiveis.length === 0}>
          {estaProcessando ? 'Criando...' : 'Criar'}
        </Button>
      </div>
    </form>
  );
}
