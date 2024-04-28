import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCandidateFormComponent } from './create-candidate-form.component';

describe('CreateCandidateFormComponent', () => {
  let component: CreateCandidateFormComponent;
  let fixture: ComponentFixture<CreateCandidateFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateCandidateFormComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCandidateFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
