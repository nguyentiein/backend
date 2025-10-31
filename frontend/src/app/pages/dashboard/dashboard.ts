import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Toolbar } from '../_component/toolbar/toolbar';
import { Customer } from '../../core/services/customer';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, Toolbar],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'],
})
export class Dashboard implements OnInit {
    Math = Math; 
  public listCustomer: any[] = [];
  public pagedCustomers: any[] = [];
  public currentPage = 1;
  public pageSize = 10;
  public totalRecords = 0;
  public totalPages = 0;
  public selectedIds: Set<string> = new Set();

  constructor(
    private customerService: Customer,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.onGetData();
  }

  /** 🔹 Lấy danh sách khách hàng */
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
        this.toastr.error('Không thể tải danh sách khách hàng', 'Lỗi');
      },
    });
  }

onRowClick(customerCode: string): void {
  if (this.selectedIds.has(customerCode)) {
    this.selectedIds.delete(customerCode);
  } else {
    this.selectedIds.add(customerCode);
  }
  this.selectedIds = new Set(this.selectedIds);
}

toggleSelect(customerCode: string, checked: boolean): void {
  if (checked) this.selectedIds.add(customerCode);
  else this.selectedIds.delete(customerCode);
  this.selectedIds = new Set(this.selectedIds);
}

toggleSelectAll(event: Event): void {
  const checked = (event.target as HTMLInputElement).checked;
  if (checked) {
    this.pagedCustomers.forEach(c => this.selectedIds.add(c.customerCode));
  } else {
    this.pagedCustomers.forEach(c => this.selectedIds.delete(c.customerCode));
  }
  this.selectedIds = new Set(this.selectedIds);
}

onDeleteSelected(): void {
  if (this.selectedIds.size === 0) {
    this.toastr.warning('Chưa chọn khách hàng nào để xóa!');
    return;
  }

  if (confirm(`Bạn có chắc muốn xóa ${this.selectedIds.size} khách hàng đã chọn?`)) {
    const deleteRequests = Array.from(this.selectedIds).map(code =>
      this.customerService.deleteCustomer(code)
    );

    Promise.all(deleteRequests.map(obs => obs.toPromise()))
      .then(() => {
        this.toastr.success('Đã xóa thành công!');
        this.selectedIds.clear();
        this.onGetData();
      })
      .catch(err => {
        console.error(err);
        this.toastr.error('Có lỗi khi xóa khách hàng!');
      });
  }
}

  /** 🔹 Tìm kiếm khách hàng */
  onSearch(keyword: string): void {
    this.customerService.onFilterCustomer(keyword, this.currentPage, this.pageSize).subscribe({
      next: (res: any) => {
        this.listCustomer = res.data ?? [];
        this.pagedCustomers = this.listCustomer;
        this.totalRecords = res.pagination?.total ?? 0;
        this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      
      },
      error: (err: any) => {
        console.error('Lỗi khi tìm kiếm:', err);
        this.toastr.error('Không thể tìm kiếm khách hàng', 'Lỗi');
      },
    });
  }

  /** 🔹 Sửa khách hàng */
  onEditCustomer(customer: any): void {
    if (!customer?.customerCode) {
      this.toastr.warning('Không có mã khách hàng để sửa', 'Cảnh báo');
      return;
    }
    this.router.navigate(['/update-customer', customer.customerCode]);
  }

  
  /** 🔹 Đổi kích thước trang */
  changePageSize(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.pageSize = +value;
    this.currentPage = 1;
    this.onGetData();
  }

  /** 🔹 Chuyển đến trang cụ thể */
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.onGetData();
    }
  }

  /** 🔹 Trang trước */
  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.onGetData();
    }
  }

  /** 🔹 Trang sau */
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.onGetData();
    }
  }

  /** 🔹 Trang đầu */
  firstPage(): void {
    this.currentPage = 1;
    this.onGetData();
  }

  /** 🔹 Trang cuối */
  lastPage(): void {
    this.currentPage = this.totalPages;
    this.onGetData();
  }

  /** 🔹 Chỉnh sửa khách hàng */


  /** 🔹 Cập nhật danh sách chọn */

}
