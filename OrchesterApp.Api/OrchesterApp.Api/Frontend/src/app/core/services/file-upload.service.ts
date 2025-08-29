import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/app/authentication/services/authentication.service';
import { environment } from 'src/environments/environment';
import { AuthHttpClientService } from './auth-http-client.service';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  constructor(
    private http: AuthHttpClientService,
  ) { }

  public uploadFile(fileId: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post(
      `api/files/upload/${fileId}`, formData
    );
  }

  public getOriginalFileName(objectName: string): string {
    const extensionMatch = objectName.match(/\.[^.]+$/);
    if(!extensionMatch) return objectName;
    return extensionMatch[1];
  }
}