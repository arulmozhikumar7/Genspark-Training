import { TestBed } from '@angular/core/testing';
import { HttpService } from './http.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TokenService } from './token.service';
import { HttpParams } from '@angular/common/http';

class MockTokenService {
  getAccessToken() {
    return 'mocked-access-token';
  }
}

describe('HttpService', () => {
  let service: HttpService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        HttpService,
        { provide: TokenService, useClass: MockTokenService }
      ]
    });

    service = TestBed.inject(HttpService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should perform GET request with headers and params', () => {
    const dummyData = { message: 'Hello' };
    const params = { page: 1, size: 10 };

    service.get('/test', params).subscribe(data => {
      expect(data).toEqual(dummyData);
    });

    const req = httpMock.expectOne('/api/v1/test?page=1&size=10');
    expect(req.request.method).toBe('GET');
    expect(req.request.headers.get('Authorization')).toBe('Bearer mocked-access-token');
    req.flush(dummyData);
  });

  it('should perform POST request with JSON body', () => {
    const body = { name: 'test' };
    const response = { success: true };

    service.post('/test', body).subscribe(data => {
      expect(data).toEqual(response);
    });

    const req = httpMock.expectOne('/api/v1/test');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(body);
    expect(req.request.headers.get('Content-Type')).toBe('application/json');
    req.flush(response);
  });

  it('should perform POST request with FormData', () => {
    const formData = new FormData();
    formData.append('file', new Blob(), 'test.txt');

    service.post('/test', formData).subscribe();

    const req = httpMock.expectOne('/api/v1/test');
    expect(req.request.headers.has('Content-Type')).toBeFalse();
    expect(req.request.body instanceof FormData).toBeTrue();
  });

  it('should perform PUT request', () => {
    const body = { name: 'update' };

    service.put('/test', body).subscribe();

    const req = httpMock.expectOne('/api/v1/test');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(body);
  });

  it('should perform DELETE request', () => {
    service.delete('/test').subscribe();

    const req = httpMock.expectOne('/api/v1/test');
    expect(req.request.method).toBe('DELETE');
  });

  it('should download blob', () => {
    const dummyBlob = new Blob(['test'], { type: 'application/octet-stream' });

    service.downloadBlob('/download').subscribe(blob => {
      expect(blob).toEqual(dummyBlob);
    });

    const req = httpMock.expectOne('/api/v1/download');
    expect(req.request.responseType).toBe('blob');
    req.flush(dummyBlob);
  });
});
