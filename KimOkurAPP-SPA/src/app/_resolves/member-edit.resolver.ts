import { Injectable } from '@angular/core';
import { User } from 'src/app/_models/User';
import {
  Resolve,
  Router,
  ActivatedRouteSnapshot
} from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private authservice: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) { }

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(this.authservice.decodedToken.nameid).pipe(
      catchError(error => {
        this.alertify.error('Kişi verisi okunurken hata ile karsilasildi');
        this.router.navigate(['/member/edit']);
        return of(null);
      })
    );
  }
}
