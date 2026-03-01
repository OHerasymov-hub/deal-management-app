import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DealDetails } from './deal-details';

describe('DealDetails', () => {
  let component: DealDetails;
  let fixture: ComponentFixture<DealDetails>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DealDetails]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DealDetails);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
