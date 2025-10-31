import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Customer } from '../../../core/services/customer';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule,CommonModule],
  templateUrl: './toolbar.html',
  styleUrls: ['./toolbar.css'], // ✅ sửa styleUrl -> styleUrls
})
export class Toolbar implements OnInit {
 @Output() searchChanged = new EventEmitter<string>();
   @Output() deleteClicked = new EventEmitter<void>();
   @Output() importSuccess = new EventEmitter<void>();

  private searchSubject = new Subject<string>();
    searchForm!: FormGroup;
 @Input() showDelete = false; 
  keyword = '';
  constructor(
      private toastr: ToastrService,
    private fb: FormBuilder,
    private customerService: Customer,  
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.searchForm = this.fb.group({
      keyword: ['']
    });

    // ⏳ Khi người dùng dừng gõ 500ms thì phát sự kiện
    this.searchForm.get('keyword')?.valueChanges
      .pipe(debounceTime(500), distinctUntilChanged())
      .subscribe(value => this.searchChanged.emit(value));
  }

  ngOnInit(): void {
    console.log('Toolbar loaded');
  }
    onSearchChange(value: string) {
      console.log("haha");
    this.searchSubject.next(value);
  }

  onImportExcel() {
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
           this.toastr.success("Thêm khách hàng thêm công");
                 this.importSuccess.emit();
          },
          error: (err:any) => {
            console.error('Import thất bại:', err);
             this.toastr.error("Có lỗi khi thêm khách hàng");
          }
        });
      }
    };

    

    input.click(); // ✅ lúc này được phép mở vì do click từ người dùng
  }

  onDeleteCustomers() {
    this.deleteClicked.emit();
  }
}
