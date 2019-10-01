import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) { }
  @Output() cancelRegister = new EventEmitter();
  ngOnInit() { }
  register() {
    this.authService.register(this.model)
      .subscribe(() => {
        console.log('kaydolma basarili');
      }, error => {
        console.log('kaydolma hatali');
      });
  }
  cancel() {
    this.cancelRegister.emit(false);
    console.log(this.model);
  }

}
