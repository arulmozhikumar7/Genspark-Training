import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { CategoryApiService } from './category-api.service';
import { HttpService } from '@core/services/http.service';
import { CategoryListResponse, Category } from '@shared/models/category.model';

describe('CategoryApiService', () => {
  let service: CategoryApiService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['get']);

    TestBed.configureTestingModule({
      providers: [
        CategoryApiService,
        { provide: HttpService, useValue: spy },
      ],
    });

    service = TestBed.inject(CategoryApiService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should fetch categories', () => {
    const mockResponse: CategoryListResponse = {
      data: [
        { id: 'cat1', name: 'Food' },
        { id: 'cat2', name: 'Travel' },
      ],
    };

    httpServiceSpy.get.and.returnValue(of(mockResponse));

    service.getCategories().subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith('/Category');
  });
});
