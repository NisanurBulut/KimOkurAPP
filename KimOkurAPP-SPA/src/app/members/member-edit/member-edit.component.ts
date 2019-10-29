import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/_models/User';
import { UserIdentity } from 'src/app/_models/UserIdentity';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { BsDatepickerConfig } from 'ngx-bootstrap';

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
  photoUrl: string;

  bsConfig: Partial<BsDatepickerConfig>;
  @HostListener('window:beforeunload', ['$event'])

  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, 
              private alertify: AlertifyService,
              private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red'
    };

    this.route.data.subscribe(data => {
      this.user = data['user'];  
    });

    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl);

    this.route.data.subscribe(data => {
      this.userIdentity = data['userIdentity'];
      console.log(this.userIdentity);
    });

  }
  updateIdentifyUser() {
    console.log(this.userIdentity);
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
  updateProfilePhoto(photoUrl) {
    this.user.photoUrl = photoUrl;
  }
}
