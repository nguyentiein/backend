import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Customer } from '../../core/services/customer';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerType } from '../../core/services/customertype';
import { ToastrService } from 'ngx-toastr';

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
    private toast: ToastrService,
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
            taxCode: customer.tax,
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
          this.toast.success("Sửa khách hàng thành công")
          this.router.navigate(['/customer']);
        },
        error: (err: any) => {
          this.toast.error("Sửa khách hàng không thành công thành công")
          console.error(err);
        }
      });
    } else {
      this.customerService.addCustomer(formData).subscribe({
        next: (res: any) => {
         this.toast.success('Thêm thành công');
          this.router.navigate(['/customer']);
        },
        error: (err: any) => {
           this.toast.error("Có lỗi khi thêm khách hàng")
          console.error(err);
        }
      });
    }
  }

 onSubmitSave() {
  if (this.customerForm.invalid) {
    this.customerForm.markAllAsTouched();
    return;
  }

  const data = this.customerForm.value;

  // ✅ Gọi API thêm khách hàng
  this.customerService.addCustomer(data).subscribe({
    next: () => {
      alert('Thêm khách hàng thành công');

      // ✅ Gọi lại API để sinh mã mới
      this.customerService.GenerateCustomerCode().subscribe({
        next: (code: string) => {
          // ✅ Reset form, gán mã mới
          this.customerForm.reset();
          this.customerForm.patchValue({ customerCode: code });

          // ✅ Focus lại vào ô tên khách hàng
          const nameInput = document.querySelector(
            'input[formControlName="fullName"]'
          ) as HTMLInputElement;
          if (nameInput) nameInput.focus();
        },
        error: (err) => console.error('Không tải được mã khách hàng mới', err)
      });
    },
    error: (err) => {
      alert('Có lỗi khi thêm khách hàng');
      console.error(err);
    }
  });
}

}
