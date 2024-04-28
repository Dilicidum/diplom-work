import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-popup-form',
  templateUrl: './popup-form.component.html',
  styleUrls: ['./popup-form.component.css'],
})
export class PopupFormComponent implements OnInit {
  show = false;
  form: FormGroup;
  constructor(private formBuilder: FormBuilder) {
    this.form = this.formBuilder.group({
      file: [null, Validators.required],
    });
  }

  ngOnInit(): void {
    for (let i = 0; i < 15; i++) {
      this.toppingList.push('Кандидат ' + (i + 1));
    }
  }

  candidates = new FormControl('');
  toppingList: string[] = [];

  onSubmit() {}

  open() {
    this.show = true;
  }

  close() {
    this.show = false;
  }
}
