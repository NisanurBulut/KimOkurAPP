import { Component, OnInit, Input } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/Photo';
import { environment } from 'src/environments/environment';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentProfilePhoto: Photo;
  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initilizeUploader();
  }

  setProfilePhoto(photo: Photo) {
    this.userService.setProfilePhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
      this.currentProfilePhoto = this.photos.filter(p => p.isMain == true)[0];
      this.currentProfilePhoto.isMain = false;
      photo.isMain = true;
      this.alertify.success("Profil fotoğrafı başarıyla güncellendi.");
    }, error => {
      this.alertify.error(error);
    });
  }
  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
  initilizeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 //10MB
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response, headers) => {
      if (response) {
        const res: Photo = JSON.parse(response);

        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        //push sayesinde mini galeride goruntulenır
        this.photos.push(photo);
      }
    };
  }
}
