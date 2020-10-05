import {Injectable} from '@angular/core';
import { UserEditComponent } from '../users/user-edit/user-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<UserEditComponent> {
    canDeactivate(component: UserEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Сигурни ли сте, че искате да продължите? Всички незапазени промени ще бъдат изгубени.');
        }
        return true;
    }
}
