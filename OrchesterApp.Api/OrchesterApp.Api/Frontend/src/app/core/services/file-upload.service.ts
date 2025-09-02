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

    return this.http.postFile(
      `api/files/upload/${fileId}`, formData
    );
  }

  public transformFileNameWithGuid(fileName: string): string {
    const guid = this.generateGuid();
    return `${guid}_${fileName}`;
  }

  public revertGuidTransformation(transformedFileName: string): string {
    const underscoreIndex = transformedFileName.indexOf('_');
    if (underscoreIndex === -1) {
      return transformedFileName; // No GUID prefix found, return as is
    }
    return transformedFileName.substring(underscoreIndex + 1);
  }

  private generateGuid(): string {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
      const r = Math.random() * 16 | 0;
      const v = c === 'x' ? r : (r & 0x3 | 0x8);
      return v.toString(16);
    });
  }
}