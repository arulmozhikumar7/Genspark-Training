import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { BudgetApiService} from './budget-api.service';
import { Budget } from '@shared/models/budget.model';
import { HttpService } from '@core/services/http.service';

describe('BudgetApiService', () => {
  let service: BudgetApiService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['get', 'post', 'put', 'delete']);

    TestBed.configureTestingModule({
      providers: [
        BudgetApiService,
        { provide: HttpService, useValue: spy },
      ],
    });

    service = TestBed.inject(BudgetApiService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should get all budgets with correct params', () => {
    const mockBudgets: Budget[] = [
      {
        id: '1',
        name: 'Budget 1',
        categoryId: 'cat1',
        categoryName: 'Category 1',
        limitAmount: 1000,
        balanceAmount: 500,
        startDate: '2025-01-01',
        endDate: '2025-01-31',
      },
    ];

    const mockResponse = {
      data: {
        items: mockBudgets,
        totalCount: mockBudgets.length,
      },
    };

    httpServiceSpy.get.and.returnValue(of(mockResponse));

    const params = { page: 1, pageSize: 10, categoryId: 'cat1' };

    service.getAll(params).subscribe(response => {
      expect(response).toEqual(mockResponse);
      expect(response.data.items.length).toBe(1);
      expect(response.data.totalCount).toBe(1);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith('/Budget', params);
  });

  it('should create a budget', () => {
    const budgetPayload: Partial<Budget> = {
      categoryId: 'cat1',
      categoryName: 'Category 1',
      limitAmount: 1000,
      balanceAmount: 1000,
      startDate: '2025-01-01',
      endDate: '2025-01-31',
    };

    const createdBudget: Budget = {
      id: '1',
      ...budgetPayload,
      name: 'New Budget',
    } as Budget;

    httpServiceSpy.post.and.returnValue(of(createdBudget));

    service.create(budgetPayload).subscribe(result => {
      expect(result).toEqual(createdBudget);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith('/Budget', budgetPayload);
  });

  it('should update a budget', () => {
    const id = '1';
    const updatePayload: Partial<Budget> = { limitAmount: 2000 };

    const updatedBudget: Budget = {
      id,
      categoryId: 'cat1',
      categoryName: 'Category 1',
      limitAmount: 2000,
      balanceAmount: 1000,
      startDate: '2025-01-01',
      endDate: '2025-01-31',
      name: 'Updated Budget',
    };

    httpServiceSpy.put.and.returnValue(of(updatedBudget));

    service.update(id, updatePayload).subscribe(result => {
      expect(result).toEqual(updatedBudget);
    });

    expect(httpServiceSpy.put).toHaveBeenCalledWith(`/Budget/${id}`, updatePayload);
  });

  it('should delete a budget', () => {
    const id = '1';

    httpServiceSpy.delete.and.returnValue(of(void 0));

    service.delete(id).subscribe(result => {
      expect(result).toBeUndefined();
    });

    expect(httpServiceSpy.delete).toHaveBeenCalledWith(`/Budget/${id}`);
  });

  it('should get budget by id', () => {
    const id = '1';
    const mockBudget: Budget = {
      id,
      categoryId: 'cat1',
      categoryName: 'Category 1',
      limitAmount: 1000,
      balanceAmount: 1000,
      startDate: '2025-01-01',
      endDate: '2025-01-31',
      name: 'Sample Budget',
    };

    const mockResponse = { data: mockBudget };

    httpServiceSpy.get.and.returnValue(of(mockResponse));

    service.getById(id).subscribe(result => {
      expect(result).toEqual(mockResponse);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith(`/Budget/${id}`);
  });
});
