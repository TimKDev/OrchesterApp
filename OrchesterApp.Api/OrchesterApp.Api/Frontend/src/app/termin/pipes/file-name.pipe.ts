import { Pipe, PipeTransform } from '@angular/core';
import { FileUploadService } from 'src/app/core/services/file-upload.service';

@Pipe({
  name: 'fileName',
  standalone: true
})
export class FileNamePipe implements PipeTransform {

  constructor(
    private fileUploadService: FileUploadService
  ){}

  transform(fileName: string): string {
    return this.fileUploadService.revertGuidTransformation(fileName);
  }

}
