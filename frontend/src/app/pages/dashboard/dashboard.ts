import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Toolbar } from '../_component/toolbar/toolbar';
import { Customer } from '../../core/services/customer';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, Toolbar],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'], // ✅ phải là styleUrls (có "s")
})
export class Dashboard implements OnInit {
  public listCustomer: any[] = [];
  public pagedCustomers: any[] = [];
  Math = Math;

  // 🔢 Phân trang
  public currentPage = 1;
  public pageSize = 10;
  public totalRecords = 0;
  public totalPages = 0;
  selectedCustomers: any[] = [];
  constructor(
    private customerService: Customer,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.onGetData();
  }


 onGetData(): void {
  this.customerService.getListCustomer(this.currentPage, this.pageSize).subscribe({
    next: (response) => {
      console.log('API trả về:', response); 
      
      this.listCustomer = response.data ?? [];

      this.totalRecords = response.meta?.total ?? this.listCustomer.length;
      
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      
      this.updatePagedData();
    },
    error: (err) => {
      console.error('Lỗi khi lấy danh sách khách hàng:', err);
    }
  });
}


  updatePagedData(): void {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedCustomers = this.listCustomer.slice(startIndex, endIndex);
  }

  changePageSize(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    this.pageSize = +value;
    this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
    this.currentPage = 1;
    this.updatePagedData();
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagedData();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagedData();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagedData();
    }
  }

  firstPage(): void {
    this.currentPage = 1;
    this.updatePagedData();
  }

  lastPage(): void {
    this.currentPage = this.totalPages;
    this.updatePagedData();
  }

  onEditCustomer(customer: any): void {
    if (!customer?.customerCode) {
      console.warn('Không có mã khách hàng để sửa');
      return;
    }
    console.log('Editing customer:', customer);
    this.router.navigate(['/update-customer', customer.customerCode]);
  }

   updateSelected() {
    this.selectedCustomers = this.pagedCustomers.filter(c => c.selected);
  }

  onDeleteSelected() {
    if (this.selectedCustomers.length === 0) return;

    if (confirm(`Bạn có chắc muốn xóa ${this.selectedCustomers.length} khách hàng đã chọn không?`)) {
      // Thực hiện xóa (gọi API thực tế)
      const idsToDelete = this.selectedCustomers.map(c => c.customerCode);

      // Xóa khỏi danh sách hiển thị
      this.pagedCustomers = this.pagedCustomers.filter(c => !idsToDelete.includes(c.customerCode));

      // Cập nhật lại danh sách chọn
      this.updateSelected();
    }
  }
}
