import { Routes } from '@angular/router';
import { NotesListComponent } from './list/notes-list.component';

export const notesRoutes: Routes = [
    {
        path: '',
        component: NotesListComponent,
        pathMatch: 'full'
    }
];