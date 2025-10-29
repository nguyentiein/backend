import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Customer } from '../../../core/services/customer';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './toolbar.html',
  styleUrls: ['./toolbar.css'], // ✅ sửa styleUrl -> styleUrls
})
export class Toolbar implements OnInit {

  constructor(
    private fb: FormBuilder,
    private customerService: Customer,  
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('Toolbar loaded');
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

    input.click(); // ✅ lúc này được phép mở vì do click từ người dùng
  }
}
