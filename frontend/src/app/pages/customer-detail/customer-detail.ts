import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Customer } from '../../core/services/customer';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerType } from '../../core/services/customertype';

@Component({
  selector: 'app-customer-detail',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './customer-detail.html',
  styleUrls: ['./customer-detail.css']
})
export class CustomerDetail implements OnInit {
  customerForm!: FormGroup;
  public id: string | null = null;
  customerTypes: any[] = [];
  sources = ['Facebook', 'Zalo', 'Tiktok'];
  businessTypes = ['Cá nhân', 'Doanh nghiệp'];
  contactChannels = ['Điện thoại', 'Email', 'Zalo'];

  constructor(
    private fb: FormBuilder,
    private customerService: Customer,
    private customerTypeService:CustomerType,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');

    this.customerForm = this.fb.group({
      customerCode: [''],
      fullName: ['', Validators.required],
      customerType: [''],
      taxCode: [''],
      companyName: [''],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{10,15}$/)]],
    });
     this.loadCustomerType();
     this.GenerateCustomerCode();

    if (this.id) {
      this.loadCustomer(this.id);
    }
  }
  
 loadCustomerType() {
  this.customerTypeService.getListCustomerType().subscribe({
    next: (res:any) => {
      this.customerTypes = res; // res là mảng loại khách hàng
      console.log('Customer types:', this.customerTypes);
    },
    error: (err) => console.error('Không tải được loại khách hàng', err)
  });
}

GenerateCustomerCode() {
  this.customerService.GenerateCustomerCode().subscribe({
    next: (code: string) => {
      console.log('Mã KH nhận được:', code);
      this.customerForm.patchValue({ customerCode: code });
    },
    error: (err) => console.error('Không tải được mã khách hàng', err)
  });
}


  private loadCustomer(id: string) {
    this.customerService.getCustomerById(id).subscribe({
      next: (res: any) => {
        if (res && res.data && res.data.length > 0) {
          const customer = res.data[0];
          this.customerForm.patchValue({
            customerCode: customer.customerCode,
            fullName: customer.fullName,
            customerType: customer.customerType,
            taxCode: customer.taxCode,
            companyName: customer.companyName,
            phoneNumber: customer.phoneNumber
          });
        }
      },
      error: (err) => console.error('Không tải được dữ liệu khách hàng', err)
    });
  }

  isInvalid(field: string): boolean {
    const control = this.customerForm.get(field);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  onSubmit() {
    if (this.customerForm.invalid) {
      this.customerForm.markAllAsTouched();
      return;
    }

    const formData = this.customerForm.value;

    if (this.id) {
      this.customerService.editCustomer(this.id, formData).subscribe({
        next: (res: any) => {
          alert('Cập nhật khách hàng thành công');
          this.router.navigate(['/customer']);
        },
        error: (err: any) => {
          alert('Có lỗi xảy ra khi cập nhật khách hàng');
          console.error(err);
        }
      });
    } else {
      this.customerService.addCustomer(formData).subscribe({
        next: (res: any) => {
          alert('Thêm khách hàng thành công');
          this.router.navigate(['/customer']);
        },
        error: (err: any) => {
          alert('Có lỗi xảy ra khi thêm khách hàng');
          console.error(err);
        }
      });
    }
  }
}
