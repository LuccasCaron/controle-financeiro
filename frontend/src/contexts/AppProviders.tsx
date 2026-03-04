import type { ReactNode } from 'react';
import { PersonProvider, usePersonContext } from './PersonContext';
import { CategoryProvider, useCategoryContext } from './CategoryContext';
import { TransactionProvider } from './TransactionContext';
import { TotalsProvider } from './TotalsContext';

interface AppProvidersProps {
  children: ReactNode;
}

function TransactionProviderWrapper({ children }: { children: ReactNode }) {
  const { getPersonById } = usePersonContext();
  const { getCategoryById } = useCategoryContext();

  return (
    <TransactionProvider getPersonById={getPersonById} getCategoryById={getCategoryById}>
      {children}
    </TransactionProvider>
  );
}

export function AppProviders({ children }: AppProvidersProps) {
  return (
    <PersonProvider>
      <CategoryProvider>
        <TransactionProviderWrapper>
          <TotalsProvider>
            {children}
          </TotalsProvider>
        </TransactionProviderWrapper>
      </CategoryProvider>
    </PersonProvider>
  );
}
