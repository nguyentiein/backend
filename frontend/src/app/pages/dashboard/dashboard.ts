import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { Toolbar } from '../_component/toolbar/toolbar';
import { Customer } from '../../core/services/customer';
import { FormsModule, NgModel } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule,RouterLink,FormsModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'],
})
export class Dashboard implements OnInit {
  Math = Math; 
  public keyWord = '';
  public listCustomer: any[] = [];
  public pagedCustomers: any[] = [];

  public currentPage = 1;
  public pageSize = 10;
  public totalRecords = 0;
  public totalPages = 0;
 private searchSubject = new Subject<string>();
  constructor(
    private customerService: Customer,
    private router: Router
  ) {}

  ngOnInit(): void {
    
    this.onGetData();
     this.searchSubject
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe((keyword) => {
        this.searchCustomers(keyword);
      });

    // Load mặc định
    this.searchCustomers('');
  
  }
handleSearchChange(value: string) {
    this.searchSubject.next(value);
  }

  searchCustomers(keyword: string) {
    this.customerService.onFilterCustomer(keyword).subscribe({
      next: (res) => {
        this.listCustomer = res.data;
        this.pagedCustomers = this.listCustomer;

      },
      error: (err) => {
        console.error('Error loading customers:', err);
      },
    });
  }
  onGetData(): void {
    this.customerService.getListCustomer(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.listCustomer = response.data ?? [];
        this.pagedCustomers = this.listCustomer;
        this.totalRecords = response.meta?.total ?? 0;
        this.totalPages = response.meta?.totalPages ?? Math.ceil(this.totalRecords / this.pageSize);
      },
      error: (err) => {
        console.error('Lỗi khi lấy danh sách khách hàng:', err);
      }
    });
  }

  changePageSize(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.pageSize = +value;
    this.currentPage = 1;
    this.onGetData();
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.onGetData(); 
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.onGetData();
    }
  }

  firstPage(): void {
    if (this.currentPage !== 1) {
      this.currentPage = 1;
      this.onGetData();
    }
  }

  lastPage(): void {
    if (this.currentPage !== this.totalPages) {
      this.currentPage = this.totalPages;
      this.onGetData();
    }
  }

  onEditCustomer(customer: any): void {
    if (!customer?.customerCode) {
      console.warn('Không có mã khách hàng để sửa');
      return;
    }
    this.router.navigate(['/update-customer', customer.customerCode]);
  }
onImportExcel() {
    alert('onImportExcel hoạt động!');
    console.log("ok");
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.xlsx, .xls';

    input.onchange = (event: any) => {
      const file = event.target.files[0];
      if (file) {
        console.log('Đã chọn file:', file.name);

        const formData = new FormData();
        formData.append('file', file);

        this.customerService.uploadExcel(formData).subscribe({
          next: (res:any) => {
            console.log('Import thành công:', res);
            alert('Import Excel thành công!');
          },
          error: (err:any) => {
            console.error('Import thất bại:', err);
            alert('Import Excel thất bại!');
          }
        });
      }
    };

    input.click(); 
  }
}

