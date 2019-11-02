import { Injectable } from '@angular/core';
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
import { Message } from '../_models/Message';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
  pageNumber = 1;
  pageSize = 5;
  messageContainer = 'Unread';
  constructor(private userService: UserService, private authService: AuthService,
              private router: Router,
              private alertify: AlertifyService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.userService.getMessages(this.authService.decodedToken.nameid,
      this.pageNumber, this.pageSize, this.messageContainer).pipe(
      catchError(error => {
        this.alertify.error('Mesajlar okunurken hata ile karşılaşıldı.');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
