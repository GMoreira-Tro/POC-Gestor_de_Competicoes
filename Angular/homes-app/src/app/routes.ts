import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DetailsComponent } from './details/details.component';
import { CRUDComponent } from './crud/crud.component';

const routeConfig: Routes = [
    {
        path: '',
        component: HomeComponent,
        title: 'Home Page'
    },
    {
        path: 'Details/:id',
        component: DetailsComponent,
        title: 'Details Page'
    },
    {
        path: 'CRUD',
        component: CRUDComponent,
        title: 'CRUD Page'
    }
];

export default routeConfig;
