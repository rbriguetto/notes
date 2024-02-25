import { NgModule } from "@angular/core";
import { NotesListComponent } from "./list/notes-list.component";
import { RouterModule } from "@angular/router";
import { notesRoutes } from "./notes.routes";
import { MatIconModule } from '@angular/material/icon'
import { MatDialogModule } from '@angular/material/dialog'
import { CommonModule } from "@angular/common";
import { NotesDetailComponent } from "./detail/notes-detail.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";

@NgModule({
    declarations: [
        NotesListComponent,
        NotesDetailComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(notesRoutes),
        ReactiveFormsModule,
        FormsModule,
        MatIconModule,
        MatDialogModule
    ],
    exports: [
        RouterModule
    ],
})
export class NotesModule
{

}