import { Component, OnInit } from '@angular/core';
import { Customer } from '../../core/services/customer';
import { CommonModule } from '@angular/common';
import { Toolbar } from '../_component/toolbar/toolbar';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule,Toolbar],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  public listCustomer: any[] = [];
  public pagedCustomers: any[] = [];
  Math = Math;
  // Phân trang
  public currentPage = 1;
  public pageSize = 10;
  public totalRecords = 0;
  public totalPages = 0;

  constructor(private customerService: Customer,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.onGetData();
  }

  onGetData() {
    this.customerService.getListCustomer().subscribe((data) => {
      this.listCustomer = data;
      this.totalRecords = data.length;
      this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
      this.updatePagedData();
    });
  }

  updatePagedData() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedCustomers = this.listCustomer.slice(startIndex, endIndex);
  }

  changePageSize(event: any) {
    this.pageSize = +event.target.value;
    this.totalPages = Math.ceil(this.totalRecords / this.pageSize);
    this.currentPage = 1;
    this.updatePagedData();
  }

  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagedData();
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagedData();
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagedData();
    }
  }

  firstPage() {
    this.currentPage = 1;
    this.updatePagedData();
  }

  lastPage() {
    this.currentPage = this.totalPages;
    this.updatePagedData();
  }
  onEditCustomer(customer: any) {
    console.log('Editing customer:', customer);
   this.router.navigate(['/update-customer/', customer.customerCode]);  
  }
}
