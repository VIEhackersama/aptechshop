import api from './api';

export interface Order {
  id: number;
  date: string;
  totalAmount: number;
  status: string;
  items: any[];
}

export const orderService = {
  getAll: async () => {
    return await api.get<Order[]>('/order'); 
  },
  
  getById: async (id: number) => {
    return await api.get<Order>(`/order/${id}`);
  },

  create: async (data: any) => {
    return await api.post('/order', data);
  }
};
