import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Category } from '../_models/category';
import { AlertifyService } from '../_services/alertify.service';
import { CategoryService } from '../_services/category.service';

@Injectable()
export class CategoriesResolver implements Resolve<Category[]> {
    constructor(private categoryService: CategoryService,
                private router: Router,
                private alertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<Category[]> {
        return this.categoryService.getAll().pipe(
            catchError(error => {
                this.alertify.error(error);
                this.router.navigate(['']);
                return of(null); // []
            })
        );
    }
}
