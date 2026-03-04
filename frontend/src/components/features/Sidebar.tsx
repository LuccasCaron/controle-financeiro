import { useNavigate, useLocation } from 'react-router-dom';
import { cn } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { Users, Tag, Receipt, BarChart3, Home, X } from 'lucide-react';

interface SidebarProps {
  colapsado?: boolean;
  onFechar?: () => void;
}

interface NavItem {
  rota: string;
  icone: React.ComponentType<{ className?: string }>;
  label: string;
}

export function Sidebar({ colapsado = false, onFechar }: SidebarProps) {
  const navigate = useNavigate();
  const location = useLocation();

  const itensNavegacao: NavItem[] = [
    { rota: '/', icone: Home, label: 'Início' },
    { rota: '/pessoas', icone: Users, label: 'Pessoas' },
    { rota: '/categorias', icone: Tag, label: 'Categorias' },
    { rota: '/transacoes', icone: Receipt, label: 'Transações' },
    { rota: '/relatorios', icone: BarChart3, label: 'Relatórios' },
  ];

  const isRotaAtiva = (rota: string): boolean => {
    if (rota === '/') {
      return location.pathname === '/';
    }
    return location.pathname.startsWith(rota);
  };

  const handleNavegar = (rota: string) => {
    navigate(rota);
    if (onFechar) {
      onFechar();
    }
  };

  return (
    <>
      {!colapsado && onFechar && (
        <div
          className="fixed inset-0 z-30 bg-black/50 lg:hidden"
          onClick={onFechar}
        />
      )}
      
      <aside
        className={cn(
          'fixed left-0 top-0 z-40 h-screen w-64 border-r border-border/20 bg-background transition-transform duration-300',
          'lg:translate-x-0 lg:relative lg:z-auto',
          colapsado ? '-translate-x-full' : 'translate-x-0'
        )}
      >
        <div className="flex h-full flex-col">
          <div className="flex h-16 items-center justify-between border-b border-border/20 px-6">
            <h2 className="text-lg font-semibold">Controle de Gastos</h2>
            <Button
              variant="ghost"
              size="icon"
              className="lg:hidden"
              onClick={onFechar}
            >
              <X className="h-5 w-5" />
            </Button>
          </div>

        <nav className="flex-1 space-y-1 p-4">
          {itensNavegacao.map((item) => {
            const Icone = item.icone;
            const ativo = isRotaAtiva(item.rota);

            return (
              <button
                key={item.rota}
                onClick={() => handleNavegar(item.rota)}
                className={cn(
                  'flex w-full items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors',
                  'hover:bg-accent hover:text-accent-foreground',
                  ativo
                    ? 'bg-accent text-accent-foreground'
                    : 'text-muted-foreground'
                )}
              >
                <Icone className="h-5 w-5" />
                <span>{item.label}</span>
              </button>
            );
          })}
        </nav>
      </div>
    </aside>
    </>
  );
}
