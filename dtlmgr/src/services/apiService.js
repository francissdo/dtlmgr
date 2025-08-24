import axios from 'axios';

class ApiService {
  constructor() {
    this.baseURL = 'http://localhost:5272/api';
    
    this.api = axios.create({
      baseURL: this.baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Add auth token to requests if available
    this.api.interceptors.request.use((config) => {
      const token = localStorage.getItem('authToken');
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    });

    // Handle auth errors
    this.api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('authToken');
          localStorage.removeItem('username');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // Auth methods
  async login(request) {
    const response = await this.api.post('/auth/login', request);
    return response.data;
  }

  // Product methods
  async getProducts() {
    const response = await this.api.get('/products');
    return response.data;
  }

  async getProduct(id) {
    const response = await this.api.get(`/products/${id}`);
    return response.data;
  }

  async createProduct(product) {
    const response = await this.api.post('/products', product);
    return response.data;
  }

  async updateProduct(id, product) {
    const response = await this.api.put(`/products/${id}`, product);
    return response.data;
  }

  async deleteProduct(id) {
    await this.api.delete(`/products/${id}`);
  }

  // Utility methods
  setAuthToken(token) {
    localStorage.setItem('authToken', token);
  }

  getAuthToken() {
    return localStorage.getItem('authToken');
  }

  removeAuthToken() {
    localStorage.removeItem('authToken');
    localStorage.removeItem('username');
  }

  isAuthenticated() {
    return !!this.getAuthToken();
  }
}

const apiService = new ApiService();
export default apiService;
