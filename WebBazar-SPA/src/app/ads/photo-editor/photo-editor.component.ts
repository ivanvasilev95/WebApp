import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { environment } from '../../../environments/environment';
import { FileUploader } from 'ng2-file-upload';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { PhotoService } from 'src/app/_services/photo.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() photos: Photo[];
  @Input() adId: number;
  @Output() getAdPhotoChange = new EventEmitter<string>();

  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  response: string;
  baseUrl = environment.apiUrl;

  constructor(private photoService: PhotoService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'photos?adId=' + this.adId,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 // ==  10mb
    });

    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onSuccessItem = (item, response) => {
      if (response) {
        const res: Photo = JSON.parse(response);
        const photo: Photo = {
          id: res.id,
          url: res.url,
          isMain: res.isMain
        };

        if (this.photos.length === 0) {
          this.getAdPhotoChange.emit(photo.url);
        }

        this.photos.push(photo);
      }
    };
  }

  setMainPhoto(photo: Photo) {
    this.photoService.setMainPhoto(photo.id).subscribe(() => {
      const currentMainPhoto = this.photos.filter(p => p.isMain === true)[0];
      currentMainPhoto.isMain = false;
      photo.isMain = true;

      // set the new main photo at first place
      // this.photos = this.photos.sort((a, b) => a.isMain < b.isMain ? 1 : -1);
      this.photos.splice(this.photos.findIndex(p => p.id === photo.id), 1);
      this.photos.unshift(photo);

      this.getAdPhotoChange.emit(photo.url);
    }, error => {
      this.alertify.error(error);
    });
  }

  deletePhoto(id: number) {
    this.alertify.confirm('Сигурни ли сте, че искате да изтриете тази снимка?', () => {
      this.photoService.deletePhoto(id).subscribe(() => {
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1);
        this.alertify.success('Снимката беше изтрита успешно');
      }, error => {
        this.alertify.error(error);
      });
    });
  }
}
