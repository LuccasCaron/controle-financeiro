/**
 * Componente principal da aplicação.
 * 
 * Configura o roteamento e os providers necessários para o funcionamento do sistema.
 */

import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AppProviders } from '@/contexts/AppProviders';
import { Layout } from '@/components/features/Layout';
import { OnboardingPage } from '@/pages/OnboardingPage';
import { PersonManagementPage } from '@/pages/PersonManagementPage';
import { CategoryManagementPage } from '@/pages/CategoryManagementPage';
import { TransactionManagementPage } from '@/pages/TransactionManagementPage';
import { ReportsPage } from '@/pages/ReportsPage';

/**
 * Componente wrapper que aplica Layout às páginas que precisam.
 * 
 * @param children - Componente da página
 */
function PageWithLayout({ children }: { children: React.ReactNode }) {
  return <Layout>{children}</Layout>;
}

/**
 * Componente raiz da aplicação.
 * 
 * Configura todas as rotas e envolve a aplicação com os providers necessários.
 */
function App() {
  return (
    <AppProviders>
      <BrowserRouter>
        <Routes>
          {/* Todas as páginas com layout (incluindo onboarding) */}
          <Route
            path="/"
            element={
              <PageWithLayout>
                <OnboardingPage />
              </PageWithLayout>
            }
          />
          
          {/* Páginas com layout */}
          <Route
            path="/pessoas"
            element={
              <PageWithLayout>
                <PersonManagementPage />
              </PageWithLayout>
            }
          />
          <Route
            path="/categorias"
            element={
              <PageWithLayout>
                <CategoryManagementPage />
              </PageWithLayout>
            }
          />
          <Route
            path="/transacoes"
            element={
              <PageWithLayout>
                <TransactionManagementPage />
              </PageWithLayout>
            }
          />
          
          {/* Página de relatórios unificada */}
          <Route
            path="/relatorios"
            element={
              <PageWithLayout>
                <ReportsPage />
              </PageWithLayout>
            }
          />
          
          {/* Rotas antigas redirecionam para relatórios */}
          <Route path="/totais/pessoas" element={<Navigate to="/relatorios?tab=pessoas" replace />} />
          <Route path="/totais/categorias" element={<Navigate to="/relatorios?tab=categorias" replace />} />
        </Routes>
      </BrowserRouter>
    </AppProviders>
  );
}

export default App;
