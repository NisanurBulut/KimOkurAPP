import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  model: any = {};
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }
  register() {
    this.authService.register(this.model)
      .subscribe(() => {
        console.log('kaydolma basarili');
      }, error => {
        console.log('kaydolma hatali');
      });
  }
  cancel() {
    console.log(this.model);
  }

}
