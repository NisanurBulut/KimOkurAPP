import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { UntypedFormGroup, FormControl, Validators, UntypedFormBuilder } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { User } from '../_models/User';
import { Router } from '@angular/router';
import { toLocalDate } from '../_helpers/local-date';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User; // Kaydolacak kullanıcının bilgilerini tutar
  registerForm: UntypedFormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(private authService: AuthService, private alertify: AlertifyService, private fb: UntypedFormBuilder, private router: Router) { }


  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };
    this.createRegisterForm();
  }
  passwordMatchValidator(g: UntypedFormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : { nomatch: true };
  }
  register() {
    if (this.registerForm.valid) {
      const formValue = this.registerForm.value;
      this.user = Object.assign({}, formValue, { dateOfBirth: toLocalDate(formValue.dateOfBirth) });
      this.authService.register(this.user).subscribe(() => {
        this.alertify.success('Kullanıcı kayıt işlemi başarılıdır.');
      },
        error => {
          this.alertify.error(error);
        },
        () => {
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          }, error => {
            this.alertify.error(error);
          });
        });
    }
  }
  createRegisterForm() {
    this.registerForm = this.fb.group({
      gender: ['Kadın'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required]
    }, { validator: this.passwordMatchValidator });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
