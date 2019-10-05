import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  constructor(
    private authService: AuthService,
    private alertify: AlertifyService
  ) {}
  @Output() cancelRegister = new EventEmitter();
  ngOnInit() {}
  register() {
    this.authService.register(this.model).subscribe(() => {
        this.alertify.success('kaydolma basarili');
      },
      error => {
        this.alertify.error(error);
      }
    );
  }
  cancel() {
    this.cancelRegister.emit(false);
   
  }
}
