import { Injectable } from "@angular/core";
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable()
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent>
{
    //Form üzerinde çalışma devam ederken sayfadan aryılmak istenirse değişimleri korumak için bu kısım çalışır.
    canDeactivate(component: MemberEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Devam edilen işlem saptanmıştır. Bu sayfadan ayrılmak istediğinize emin misiniz ? ');
        }
        return true;
    }

}