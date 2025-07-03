export interface Category {
  id: string;
  name: string;
}
export interface CategoryListResponse {
  data: Category[];
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors: any;
}