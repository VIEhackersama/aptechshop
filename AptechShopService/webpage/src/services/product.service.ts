import api from './api';

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  imageUrl?: string;
  categoryId?: number;
}

export const productService = {
  getAll: async () => {
    return await api.get<Product[]>('/products');
  },
  
  getById: async (id: number) => {
    return await api.get<Product>(`/products/${id}`);
  },

  create: async (data: any) => {
    return await api.post('/products', data);
  },

  update: async (id: number, data: any) => {
    return await api.put(`/products/${id}`, data);
  },

  delete: async (id: number) => {
    return await api.delete(`/products/${id}`);
  }
};
