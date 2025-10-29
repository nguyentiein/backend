import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { map, Observable } from 'rxjs';
import { DataResponse } from '../data-response.model';

@Injectable({
  providedIn: 'root'
})
export class Customer extends ApiService{
    constructor(protected override http: HttpClient) {
       super(http);
    }
getListCustomer(page: number, pageSize: number): Observable<any> {
  const url = `${environment.apiUrl}/api/v1/Customers`;
  const params = { page: page, pageSize: pageSize };

  return this.http.get<any>(url, { params });
}



  getCustomerById(id: string): Observable<Customer> {
    const url = `/api/v1/Customers/${id}`;
    return super.getEntity(url).pipe(
      map(res => {
        if (!res) {
          throw new Error('Không tìm thấy khách hàng');
        }
        return res;
      })
    );
  }

   GenerateCustomerCode(): Observable<any> {
  const url = `${environment.apiUrl}/api/v1/Customers/GenerateCustomerCode`;
  return this.http.get<DataResponse<string>>(url).pipe(
    map(response => {
      if (!response || !response.data) {
        console.error('Invalid API response:', response);
        return null;
      }
      return response.data;
    })
  );
}



   addCustomer(formData : any): Observable<any> {
    let url = `/api/v1/Customers`;
    return super.postEntity(url, formData).pipe(
      map((res) => {
        if (res === undefined) {
          throw new Error('Invalid response from server');
        }
        return res;
      })
    );
  }


 editCustomer(id: string, formData: any): Observable<any> {
  const url = `/api/v1/Customers`;
  return super.patchEntity(url, id, formData).pipe(
    map(res => {
      if (res === undefined) {
        throw new Error('Invalid response from server');
      }
      return res;
    })
  );
}

uploadExcel(formData: FormData): Observable<any> {
  return this.http.post('https://localhost:7185/api/v1/Customers/import', formData);
}




}
export interface Customer {
  customerType: string;
  customerCode: string;
  fullName: string;
  companyName?: string;
  shippingAddresses?: string;
  phoneNumber: string;
  latestPurchaseDate?: string;
  purchasedProductCodes?: string;
  purchasedProductNames?: string;
  Products?: any[];
}
