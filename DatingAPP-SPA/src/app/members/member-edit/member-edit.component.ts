import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/User';
import { UserIdentity } from 'src/app/_models/UserIdentity';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm', { static: true }) editForm: NgForm;
  @ViewChild('identityForm', { static: true }) identityForm: NgForm;
  user: User;
  userIdentity: UserIdentity;
  //tarayıcı penceresi kapanırken düzenleme işlemi yapılıyyorsa uyarı verir.
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService, private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.route.data.subscribe(data => {
      this.userIdentity = data['userIdentity'];
    });

  }
  updateIdentifyUser() {
    this.userService.updateIdentityUser(this.authService.decodedToken.nameid, this.userIdentity)
      .subscribe(next => {
        this.alertify.success('Kullanıcı bilgisi başarılı şekilde güncellendi.');
        this.identityForm.reset(this.userIdentity);
      },
        error => {
          this.alertify.error(error);
        });

  }
  updateUser() {
    console.log(this.user);

    this.userService.updateUser(this.authService.decodedToken.nameid, this.user)
      .subscribe(next => {
        this.alertify.success('Kullanıcı bilgisi başarılı şekilde güncellendi.');
        this.editForm.reset(this.user);
      },
        error => {
          this.alertify.error(error);
        });
  }

}
