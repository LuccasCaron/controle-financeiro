import { createContext, useContext, useState, useCallback, useEffect } from 'react';
import type { ReactNode } from 'react';
import type { Category, CategoryFormData } from '@/types/category.types';
import { categoriaService } from '@/services/categoriaService';

interface CategoryContextType {
  categorias: Category[];
  createCategory: (dadosCategoria: CategoryFormData) => Promise<Category>;
  getAllCategories: () => Category[];
  getCategoryById: (idCategoria: string) => Category | undefined;
  getCategoriesByFinality: (finalidade: Category['finalidade']) => Category[];
  carregarCategorias: () => Promise<void>;
}

const CategoryContext = createContext<CategoryContextType | undefined>(undefined);

interface CategoryProviderProps {
  children: ReactNode;
}

export function CategoryProvider({ children }: CategoryProviderProps) {
  const [categorias, setCategorias] = useState<Category[]>([]);

  const carregarCategorias = useCallback(async () => {
    try {
      const categoriasCarregadas = await categoriaService.listar();
      setCategorias(categoriasCarregadas);
    } catch (error) {
      console.error('Erro ao carregar categorias:', error);
    }
  }, []);

  useEffect(() => {
    carregarCategorias();
  }, [carregarCategorias]);

  const validarDadosCategoria = useCallback((dadosCategoria: CategoryFormData): void => {
    if (!dadosCategoria.nome || dadosCategoria.nome.trim().length === 0) {
      throw new Error('O nome da categoria é obrigatório');
    }

    if (dadosCategoria.nome.length > 100) {
      throw new Error('O nome da categoria não pode exceder 100 caracteres');
    }

    if (dadosCategoria.descricao && dadosCategoria.descricao.length > 500) {
      throw new Error('A descrição da categoria não pode exceder 500 caracteres');
    }

    const finalidadesPermitidas: Category['finalidade'][] = ['despesa', 'receita', 'ambas'];
    if (!finalidadesPermitidas.includes(dadosCategoria.finalidade)) {
      throw new Error('A finalidade da categoria deve ser: despesa, receita ou ambas');
    }
  }, []);

  const createCategory = useCallback(async (dadosCategoria: CategoryFormData): Promise<Category> => {
    validarDadosCategoria(dadosCategoria);

    const novaCategoria = await categoriaService.criar(dadosCategoria);
    
    await carregarCategorias();

    return novaCategoria;
  }, [validarDadosCategoria, carregarCategorias]);

  const getAllCategories = useCallback((): Category[] => {
    return categorias;
  }, [categorias]);

  const getCategoryById = useCallback((idCategoria: string): Category | undefined => {
    return categorias.find((categoria) => categoria.id === idCategoria);
  }, [categorias]);

  const getCategoriesByFinality = useCallback(
    (finalidade: Category['finalidade']): Category[] => {
      return categorias.filter((categoria) => {
        if (categoria.finalidade === 'ambas') {
          return true;
        }
        return categoria.finalidade === finalidade;
      });
    },
    [categorias]
  );

  const valorContexto: CategoryContextType = {
    categorias,
    createCategory,
    getAllCategories,
    getCategoryById,
    getCategoriesByFinality,
    carregarCategorias,
  };

  return (
    <CategoryContext.Provider value={valorContexto}>
      {children}
    </CategoryContext.Provider>
  );
}

export function useCategoryContext(): CategoryContextType {
  const contexto = useContext(CategoryContext);

  if (contexto === undefined) {
    throw new Error('useCategoryContext deve ser usado dentro de um CategoryProvider');
  }

  return contexto;
}
