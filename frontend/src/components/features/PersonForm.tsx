import { useState, type FormEvent } from 'react';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { usePersonContext } from '@/contexts/PersonContext';
import type { Person, PersonFormData } from '@/types/person.types';

interface PersonFormProps {
  pessoaParaEditar?: Person;
  onSucesso: () => void;
  onCancelar: () => void;
}

export function PersonForm({ pessoaParaEditar, onSucesso, onCancelar }: PersonFormProps) {
  const { createPerson, updatePerson } = usePersonContext();

  const [nomePessoa, setNomePessoa] = useState(pessoaParaEditar?.nome || '');
  const [cpfPessoa, setCpfPessoa] = useState(pessoaParaEditar?.cpf || '');
  const [dataNascimento, setDataNascimento] = useState(
    pessoaParaEditar?.dataNascimento 
      ? new Date(pessoaParaEditar.dataNascimento).toISOString().split('T')[0]
      : ''
  );
  const [mensagemErro, setMensagemErro] = useState<string | null>(null);
  const [estaProcessando, setEstaProcessando] = useState(false);

  const validarFormulario = (): boolean => {
    setMensagemErro(null);

    if (!nomePessoa.trim()) {
      setMensagemErro('O nome da pessoa é obrigatório');
      return false;
    }

    if (nomePessoa.length > 100) {
      setMensagemErro('O nome da pessoa não pode exceder 100 caracteres');
      return false;
    }

    if (!cpfPessoa.trim()) {
      setMensagemErro('O CPF da pessoa é obrigatório');
      return false;
    }

    if (!dataNascimento) {
      setMensagemErro('A data de nascimento é obrigatória');
      return false;
    }

    const dataNasc = new Date(dataNascimento);
    if (dataNasc > new Date()) {
      setMensagemErro('A data de nascimento não pode ser no futuro');
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
      const dadosPessoa: PersonFormData = {
        nome: nomePessoa.trim(),
        cpf: cpfPessoa.replace(/[^\d]/g, ''),
        dataNascimento: dataNascimento,
      };

      if (pessoaParaEditar) {
        await updatePerson(pessoaParaEditar.id, dadosPessoa);
      } else {
        await createPerson(dadosPessoa);
      }

      onSucesso();
    } catch (erro) {
      setMensagemErro(
        erro instanceof Error ? erro.message : 'Ocorreu um erro ao salvar a pessoa'
      );
    } finally {
      setEstaProcessando(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4 pt-2">
      <div className="space-y-2">
        <Label htmlFor="nome-pessoa">Nome</Label>
        <Input
          id="nome-pessoa"
          type="text"
          value={nomePessoa}
          onChange={(e) => setNomePessoa(e.target.value)}
          placeholder="Digite o nome da pessoa"
          maxLength={100}
          disabled={estaProcessando}
          required
        />
        <p className="text-xs text-muted-foreground">
          Máximo de 100 caracteres
        </p>
      </div>

      <div className="space-y-2">
        <Label htmlFor="cpf-pessoa">CPF</Label>
        <Input
          id="cpf-pessoa"
          type="text"
          value={cpfPessoa}
          onChange={(e) => setCpfPessoa(e.target.value.replace(/[^\d]/g, ''))}
          placeholder="Digite o CPF (somente números)"
          maxLength={11}
          disabled={estaProcessando}
          required
        />
        <p className="text-xs text-muted-foreground">
          Digite apenas os 11 dígitos numéricos
        </p>
      </div>

      <div className="space-y-2">
        <Label htmlFor="data-nascimento">Data de Nascimento</Label>
        <Input
          id="data-nascimento"
          type="date"
          value={dataNascimento}
          onChange={(e) => setDataNascimento(e.target.value)}
          disabled={estaProcessando}
          required
        />
        <p className="text-xs text-muted-foreground">
          A data de nascimento não pode ser no futuro
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
          {estaProcessando
            ? 'Salvando...'
            : pessoaParaEditar
              ? 'Atualizar'
              : 'Criar'}
        </Button>
      </div>
    </form>
  );
}
