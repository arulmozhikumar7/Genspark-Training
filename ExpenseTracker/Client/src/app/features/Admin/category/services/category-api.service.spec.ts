import { TestBed } from '@angular/core/testing';
import { CategoryApiService } from './category-api.service';
import { HttpService } from '@core/services/http.service';
import { of } from 'rxjs';
import { Category, ApiResponse } from '@shared/models/category.model';

describe('CategoryApiService', () => {
  let service: CategoryApiService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  const mockCategory: Category = { id: '1', name: 'Utilities' };
  const mockCategoryList: Category[] = [
    { id: '1', name: 'Utilities' },
    { id: '2', name: 'Groceries' },
  ];

  beforeEach(() => {
    httpServiceSpy = jasmine.createSpyObj('HttpService', ['get', 'post', 'put', 'delete']);

    TestBed.configureTestingModule({
      providers: [
        CategoryApiService,
        { provide: HttpService, useValue: httpServiceSpy },
      ],
    });

    service = TestBed.inject(CategoryApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAllCategories', () => {
    it('should return a list of categories', (done) => {
      const response: ApiResponse<Category[]> = {
        success: true,
        message: 'Fetched',
        data: mockCategoryList,
        errors: null,
      };

      httpServiceSpy.get.and.returnValue(of(response));

      service.getAllCategories().subscribe((res) => {
        expect(res).toEqual(mockCategoryList);
        expect(httpServiceSpy.get).toHaveBeenCalledWith('/Category');
        done();
      });
    });
  });

  describe('getCategoryById', () => {
    it('should return a category by ID', (done) => {
      const response: ApiResponse<Category> = {
        success: true,
        message: 'Fetched',
        data: mockCategory,
        errors: null,
      };

      httpServiceSpy.get.and.returnValue(of(response));

      service.getCategoryById('1').subscribe((res) => {
        expect(res).toEqual(mockCategory);
        expect(httpServiceSpy.get).toHaveBeenCalledWith('/Category/1');
        done();
      });
    });
  });

  describe('createCategory', () => {
    it('should create and return a category', (done) => {
      const categoryData = { name: 'New Category' };
      const response: ApiResponse<Category> = {
        success: true,
        message: 'Created',
        data: mockCategory,
        errors: null,
      };

      httpServiceSpy.post.and.returnValue(of(response));

      service.createCategory(categoryData).subscribe((res) => {
        expect(res).toEqual(mockCategory);
        expect(httpServiceSpy.post).toHaveBeenCalledWith('/Category', categoryData);
        done();
      });
    });
  });

  describe('updateCategory', () => {
    it('should update and return the category', (done) => {
      const response: ApiResponse<Category> = {
        success: true,
        message: 'Updated',
        data: mockCategory,
        errors: null,
      };

      httpServiceSpy.put.and.returnValue(of(response));

      service.updateCategory('1', mockCategory).subscribe((res) => {
        expect(res).toEqual(mockCategory);
        expect(httpServiceSpy.put).toHaveBeenCalledWith('/Category/1', mockCategory);
        done();
      });
    });
  });

  describe('deleteCategory', () => {
    it('should delete the category and return void', (done) => {
      const response: ApiResponse<null> = {
        success: true,
        message: 'Deleted',
        data: null,
        errors: null,
      };

      httpServiceSpy.delete.and.returnValue(of(response));

      service.deleteCategory('1').subscribe((res) => {
        expect(res).toBeUndefined();
        expect(httpServiceSpy.delete).toHaveBeenCalledWith('/Category/1');
        done();
      });
    });
  });
});
