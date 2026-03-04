import { type ClassValue, clsx } from "clsx"
import { twMerge } from "tailwind-merge"

/**
 * Combina classes CSS usando clsx e tailwind-merge.
 * 
 * Útil para combinar classes do Tailwind CSS de forma segura,
 * evitando conflitos e garantindo que as classes corretas sejam aplicadas.
 * 
 * @param inputs - Classes CSS a serem combinadas
 * @returns String com classes combinadas
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}
