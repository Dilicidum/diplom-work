import { HttpClient } from '@angular/common/http';
import { Component, Inject, Input, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-popup-form',
  templateUrl: './popup-form.component.html',
  styleUrls: ['./popup-form.component.css'],
})
export class PopupFormComponent implements OnInit {
  show = false;
  form: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    @Inject(MAT_DIALOG_DATA)
    public data: {
      bestAlternatives: [];
      amountOfCandidates: number;
      vacancyId: number;
    }
  ) {
    this.form = this.formBuilder.group({
      candidates: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    for (let i = 0; i < this.data.amountOfCandidates; i++) {
      this.toppingList.push('Candidate ' + (i + 1));
    }
  }

  toppingList: string[] = [];

  onSubmit() {
    console.log('event', this.form.get('candidates')?.value);
    let arr = this.form.get('candidates')?.value;
    const candidateNumbers = arr.map((candidate) => {
      const match = candidate.match(/\d+/); // \d+ matches one or more digits
      return match ? parseInt(match[0], 10) : null; // convert the match to a number
    });
    console.log('candidateNumbers', candidateNumbers);
    this.http
      .post(
        'http://localhost:5292/api/assesments/analysis/' + this.data.vacancyId,
        candidateNumbers
      )
      .subscribe((res) => {});
  }

  open() {
    this.show = true;
  }

  close() {
    this.show = false;
  }
}
