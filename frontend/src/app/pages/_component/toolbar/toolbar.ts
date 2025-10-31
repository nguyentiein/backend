import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Customer } from '../../../core/services/customer';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-toolbar',
  standalone: true,
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './toolbar.html',
  styleUrls: ['./toolbar.css'], // ✅ sửa styleUrl -> styleUrls
})
export class Toolbar implements OnInit {
 @Output() searchChanged = new EventEmitter<string>();
  private searchSubject = new Subject<string>();
    searchForm!: FormGroup;

  keyword = '';
  constructor(
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
