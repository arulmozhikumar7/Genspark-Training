import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders ,HttpParams} from '@angular/common/http';
import { TokenService } from './token.service';
import { Observable } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class HttpService {
  private readonly baseUrl = '/api/v1';

  constructor(private http: HttpClient, private tokenService: TokenService) {}

  private getAuthHeaders(url: string): HttpHeaders {
    const isPublic =
      url.includes('/Auth/login') || url.includes('/User/register');

    const headers = {
      'Content-Type': 'application/json',
      ...(isPublic
        ? {}
        : {
            Authorization: `Bearer ${this.tokenService.getAccessToken() ?? ''}`,
          }),
    };

    return new HttpHeaders(headers);
  }

get<T>(url: string, params?: any): Observable<T> {
  let httpParams = new HttpParams();

  if (params) {
    Object.keys(params).forEach(key => {
      if (params[key] !== undefined && params[key] !== null) {
        httpParams = httpParams.set(key, params[key].toString());
      }
    });
  }

  return this.http.get<T>(`${this.baseUrl}${url}`, {
    headers: this.getAuthHeaders(url),
    params: httpParams,
    observe: 'body'
  });
}


post<T>(url: string, body: any): Observable<T> {
  const isFormData = body instanceof FormData;

  const headers = isFormData
    ? this.getAuthHeaders(url).delete('Content-Type')
    : this.getAuthHeaders(url);

  return this.http.post<T>(`${this.baseUrl}${url}`, body, { headers });
}


  put<T>(url: string, body: any) {
    return this.http.put<T>(`${this.baseUrl}${url}`, body, {
      headers: this.getAuthHeaders(url),
    });
  }

  delete<T>(url: string) {
    return this.http.delete<T>(`${this.baseUrl}${url}`, {
      headers: this.getAuthHeaders(url),
    });
  }

  downloadBlob(url: string, params?: HttpParams) {
  return this.http.get<Blob>(`${this.baseUrl}${url}`, {
    headers: this.getAuthHeaders(url),
    params,
    responseType: 'blob' as 'json' 
  });
}
}
