import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { map, Observable } from 'rxjs';
import { DataResponse } from '../data-response.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerType extends ApiService{
    constructor(protected override http: HttpClient) {
       super(http);
    }
   getListCustomerType(): Observable<any[]> {
    const url = `${environment.apiUrl}/api/v1/CustomerType`;

    return this.http.get<DataResponse<CustomerType>>(url).pipe(
      map(response => {
        if (!response || !Array.isArray(response.data)) {
          console.error('Invalid API response:', response);
          return [];
        }
        
        return response.data;
      })
    );
  }
}



export interface CustomerType {
  CustomerTypeId: string;
  CustomerTypeName: string;
       
}


