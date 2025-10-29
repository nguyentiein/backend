import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableWork } from './table-work';

describe('TableWork', () => {
  let component: TableWork;
  let fixture: ComponentFixture<TableWork>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableWork]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TableWork);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
