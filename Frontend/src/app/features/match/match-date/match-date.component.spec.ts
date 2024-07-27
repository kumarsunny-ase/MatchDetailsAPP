import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MatchDateComponent } from './match-date.component';

describe('MatchDateComponent', () => {
  let component: MatchDateComponent;
  let fixture: ComponentFixture<MatchDateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MatchDateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MatchDateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
