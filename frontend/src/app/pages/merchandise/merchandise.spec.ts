import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Merchandise } from './merchandise';

describe('Merchandise', () => {
  let component: Merchandise;
  let fixture: ComponentFixture<Merchandise>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Merchandise]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Merchandise);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
