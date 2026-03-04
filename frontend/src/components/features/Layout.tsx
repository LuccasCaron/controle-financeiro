import { useState } from 'react';
import type { ReactNode } from 'react';
import { Button } from '@/components/ui/button';
import { Sidebar } from './Sidebar';
import { Menu } from 'lucide-react';

interface LayoutProps {
  children: ReactNode;
}

export function Layout({ children }: LayoutProps) {
  const [sidebarAbertoMobile, setSidebarAbertoMobile] = useState(false);

  const toggleSidebar = () => {
    setSidebarAbertoMobile(!sidebarAbertoMobile);
  };

  const fecharSidebar = () => {
    setSidebarAbertoMobile(false);
  };

  return (
    <div className="flex h-screen">
      <Sidebar colapsado={!sidebarAbertoMobile} onFechar={fecharSidebar} />

      <main className="flex-1 overflow-y-auto lg:ml-0">
        <div className="lg:hidden fixed top-4 left-4 z-20">
          <Button
            variant="outline"
            size="icon"
            onClick={toggleSidebar}
            className="bg-background"
          >
            <Menu className="h-5 w-5" />
          </Button>
        </div>

        <div className="container mx-auto p-4 md:p-6 pt-16 lg:pt-6">
          {children}
        </div>
      </main>
    </div>
  );
}
