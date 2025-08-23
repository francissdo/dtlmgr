import apiClient from './apiClient';

// Auth API
export const authApi = {
  login: async (credentials) => {
    const response = await apiClient.post('/auth/login', credentials);
    return response.data;
  },
  
  register: async (userData) => {
    const response = await apiClient.post('/auth/register', userData);
    return response.data;
  },
};

// Products API
export const productsApi = {
  getAll: async () => {
    const response = await apiClient.get('/products');
    return response.data;
  },
  
  getById: async (id) => {
    const response = await apiClient.get(`/products/${id}`);
    return response.data;
  },
  
  create: async (product) => {
    const response = await apiClient.post('/products', product);
    return response.data;
  },
  
  update: async (id, product) => {
    const response = await apiClient.put(`/products/${id}`, product);
    return response.data;
  },
  
  delete: async (id) => {
    await apiClient.delete(`/products/${id}`);
  },
  
  search: async (name) => {
    const response = await apiClient.get(`/products/search?name=${encodeURIComponent(name)}`);
    return response.data;
  },
};

// Categories API
export const categoriesApi = {
  getAll: async () => {
    const response = await apiClient.get('/categories');
    return response.data;
  },
  
  getById: async (id) => {
    const response = await apiClient.get(`/categories/${id}`);
    return response.data;
  },
  
  create: async (category) => {
    const response = await apiClient.post('/categories', category);
    return response.data;
  },
  
  update: async (id, category) => {
    const response = await apiClient.put(`/categories/${id}`, category);
    return response.data;
  },
  
  delete: async (id) => {
    await apiClient.delete(`/categories/${id}`);
  },
  
  getActive: async () => {
    const response = await apiClient.get('/categories/active');
    return response.data;
  },
};
