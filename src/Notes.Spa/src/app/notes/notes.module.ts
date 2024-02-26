import { NgModule } from "@angular/core";
import { NotesListComponent } from "./list/notes-list.component";
import { RouterModule } from "@angular/router";
import { notesRoutes } from "./notes.routes";
import { MatDialogModule } from '@angular/material/dialog'
import { CommonModule } from "@angular/common";
import { NotesDetailComponent } from "./detail/notes-detail.component";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { LoadingModule } from "../ui/loading/loading.module";

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
        MatDialogModule,
        LoadingModule
    ],
    exports: [
        RouterModule
    ],
})
export class NotesModule
{

}