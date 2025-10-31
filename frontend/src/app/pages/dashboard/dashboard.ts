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
  public listCustomer: any[] = [];
  public pagedCustomers: any[] = [];
  public currentPage = 1;
  public pageSize = 10;
  public totalRecords = 0;
  public totalPages = 0;
  public selectedCustomers: any[] = [];
  Math = Math;

  constructor(
    private customerService: Customer,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.onGetData();
  }

  /** 🔹 Lấy danh sách khách hàng (phân trang server) */
  onGetData(): void {
    this.customerService.getListCustomer(this.currentPage, this.pageSize).subscribe({
      next: (response) => {
        this.listCustomer = response.data ?? [];
        this.pagedCustomers = this.listCustomer;
        this.totalRecords = response.meta?.total ?? 0;
        this.totalPages = response.meta?.totalPages ?? Math.ceil(this.totalRecords / this.pageSize);
        this.toastr.success('Lấy dữ liệu thành công', 'Thông báo');
      },
      error: (err) => {
        console.error('Lỗi khi lấy danh sách khách hàng:', err);
        this.toastr.error('Không thể tải danh sách khách hàng', 'Lỗi');
      },
    });
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
  onEditCustomer(customer: any): void {
    if (!customer?.customerCode) {
      this.toastr.warning('Không có mã khách hàng để sửa', 'Cảnh báo');
      return;
    }
    this.router.navigate(['/update-customer', customer.customerCode]);
  }

  /** 🔹 Cập nhật danh sách chọn */
  updateSelected(): void {
    this.selectedCustomers = this.pagedCustomers.filter(c => c.selected);
  }

  /** 🔹 Xóa khách hàng đã chọn */
  onDeleteSelected(): void {
    if (this.selectedCustomers.length === 0) {
      this.toastr.warning('Chưa chọn khách hàng nào để xóa');
      return;
    }

    if (confirm(`Bạn có chắc muốn xóa ${this.selectedCustomers.length} khách hàng đã chọn không?`)) {
      const idsToDelete = this.selectedCustomers.map(c => c.customerCode);
      this.pagedCustomers = this.pagedCustomers.filter(c => !idsToDelete.includes(c.customerCode));
      this.updateSelected();
      this.toastr.success('Xóa khách hàng thành công');
    }
  }
 
  onSearch(keyword: string) {
  this.customerService.onFilterCustomer(keyword, this.currentPage, this.pageSize).subscribe({
    next: (res: any) => {
      // Gán lại danh sách hiển thị
      this.listCustomer = res.data ?? [];
      this.pagedCustomers = this.listCustomer;

      // Gán lại thông tin phân trang (đúng key là pagination)
      this.totalRecords = res.pagination?.total ?? 0;
      this.totalPages =
        Math.ceil(this.totalRecords / this.pageSize);

      // Nếu muốn hiển thị toast
      this.toastr.success('Tìm kiếm thành công');
    },
    error: (err: any) => {
      console.error('Lỗi khi tìm kiếm:', err);
      this.toastr.error('Không thể tìm kiếm khách hàng', 'Lỗi');
    },
  });
}



  /** 🔹 Test Toast */
  toast(): void {
    this.toastr.success('Lấy dữ liệu thành công');
  }
}
