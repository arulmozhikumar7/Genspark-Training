import { TestBed } from '@angular/core/testing';
import { of } from 'rxjs';
import { ExpenseApiService } from './expense-api.service';
import { HttpService } from '@core/services/http.service';
import { ExpenseListResponse } from '@shared/models/expense.model'; 
import { Expense } from '@features/expense/models/expense.model';

describe('ExpenseApiService', () => {
  let service: ExpenseApiService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('HttpService', ['get', 'post', 'put', 'delete']);

    TestBed.configureTestingModule({
      providers: [
        ExpenseApiService,
        { provide: HttpService, useValue: spy },
      ],
    });

    service = TestBed.inject(ExpenseApiService);
    httpServiceSpy = TestBed.inject(HttpService) as jasmine.SpyObj<HttpService>;
  });

  it('should get all expenses with given params', () => {
    const mockResponse: ExpenseListResponse = {
      success: true,
      message: 'Fetched successfully',
      data: {
        items: [
          { id: '1', description: 'Test expense', amount: 50 } as Expense,
        ],
        totalCount: 1,
      },
      errors: null,
    };

    httpServiceSpy.get.and.returnValue(of(mockResponse));

    const params = { page: 1, categoryId: 'cat1', minAmount: 10 };

    service.getAll(params).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith('/Expense/filtered', params);
  });

  it('should get expense by id', () => {
    const mockExpense: Expense = { id: '1', description: 'Test expense', amount: 50 } as Expense;

    httpServiceSpy.get.and.returnValue(of(mockExpense));

    service.getById('1').subscribe(expense => {
      expect(expense).toEqual(mockExpense);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith('/Expense/1');
  });

  it('should create expense', () => {
    const payload: Partial<Expense> = { description: 'New expense', amount: 100 };
    const createdExpense: Expense = { id: '2', ...payload } as Expense;

    httpServiceSpy.post.and.returnValue(of(createdExpense));

    service.create(payload).subscribe(expense => {
      expect(expense).toEqual(createdExpense);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith('/Expense', payload);
  });

  it('should update expense', () => {
    const payload: Partial<Expense> = { description: 'Updated expense' };
    const updatedExpense: Expense = { id: '1', description: 'Updated expense', amount: 50 } as Expense;

    httpServiceSpy.put.and.returnValue(of(updatedExpense));

    service.update('1', payload).subscribe(expense => {
      expect(expense).toEqual(updatedExpense);
    });

    expect(httpServiceSpy.put).toHaveBeenCalledWith('/Expense/1', payload);
  });

  it('should delete expense', () => {
    httpServiceSpy.delete.and.returnValue(of(undefined));

    service.delete('1').subscribe(response => {
      expect(response).toBeUndefined();
    });

    expect(httpServiceSpy.delete).toHaveBeenCalledWith('/Expense/1');
  });

  it('should get expense summary with filters', () => {
    const filters = { categoryId: 'cat1' };
    const mockSummary = [
      { categoryName: 'Food', totalAmount: 100 },
      { categoryName: 'Transport', totalAmount: 50 },
    ];

    httpServiceSpy.get.and.returnValue(of(mockSummary));

    service.getExpenseSummary(filters).subscribe(summary => {
      expect(summary).toEqual(mockSummary);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith('/Expense/summary', filters);
  });
});
