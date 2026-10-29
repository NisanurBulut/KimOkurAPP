import { Injectable } from '@angular/core';
import { User } from 'src/app/_models/User';
import { PaginatedResult } from 'src/app/_models/Pagination';
import { Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver  {
    pageNumber = 1;
    pageSize = 5;
    likesParams = 'Likers';

    constructor(private userService: UserService, private router: Router,
                private alertify: AlertifyService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<PaginatedResult<User[]>> {
        return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParams).pipe(
            catchError(error => {
                this.alertify.error('Veri okunurken hata ile karşılaşıldı.');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}
