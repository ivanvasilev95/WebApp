import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'isApproved'
})
export class IsApprovedPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    return value.isApproved ? value.title : value.title + ' (очаква одобрение)';
  }

}
