import { TestBed } from '@angular/core/testing';
import { ReceiptApiService, Receipt } from './receipt-api.service';
import { HttpService } from '@core/services/http.service';
import { of } from 'rxjs';

describe('ReceiptApiService', () => {
  let service: ReceiptApiService;
  let httpServiceSpy: jasmine.SpyObj<HttpService>;

  beforeEach(() => {
    httpServiceSpy = jasmine.createSpyObj('HttpService', ['post', 'get', 'downloadBlob', 'delete']);

    TestBed.configureTestingModule({
      providers: [
        ReceiptApiService,
        { provide: HttpService, useValue: httpServiceSpy }
      ]
    });

    service = TestBed.inject(ReceiptApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should upload a receipt file', () => {
    const mockFile = new File(['dummy content'], 'receipt.jpg', { type: 'image/jpeg' });
    const expenseId = 'exp123';
    const mockResponse = { success: true };

    httpServiceSpy.post.and.returnValue(of(mockResponse));

    service.uploadReceipt(expenseId, mockFile).subscribe(response => {
      expect(response).toEqual(mockResponse);
    });

    expect(httpServiceSpy.post).toHaveBeenCalledWith(
      `/Receipt/upload/${expenseId}`,
      jasmine.any(FormData)
    );
  });

  it('should get receipts by expenseId', () => {
    const expenseId = 'exp456';
    const mockReceipts: Receipt[] = [
      {
        id: 'r1',
        expenseId,
        fileName: 'file1.pdf',
        contentType: 'application/pdf',
        createdAt: '2023-01-01T00:00:00Z',
      }
    ];

    httpServiceSpy.get.and.returnValue(of({ data: mockReceipts }));

    service.getReceiptsByExpenseId(expenseId).subscribe(result => {
      expect(result.data).toEqual(mockReceipts);
    });

    expect(httpServiceSpy.get).toHaveBeenCalledWith(`/Receipt/expense/${expenseId}`);
  });

  it('should download a receipt as a Blob', () => {
    const id = 'receipt789';
    const blob = new Blob(['sample data'], { type: 'application/pdf' });

    httpServiceSpy.downloadBlob.and.returnValue(of(blob));

    service.downloadReceipt(id).subscribe(result => {
      expect(result).toBe(blob);
    });

    expect(httpServiceSpy.downloadBlob).toHaveBeenCalledWith(`/Receipt/download/${id}`);
  });

  it('should delete a receipt by id', () => {
    const id = 'receipt999';

    httpServiceSpy.delete.and.returnValue(of(undefined));

    service.deleteReceipt(id).subscribe(response => {
      expect(response).toBeUndefined();
    });

    expect(httpServiceSpy.delete).toHaveBeenCalledWith(`/Receipt/${id}`);
  });
});
