import { Component, Inject } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { Note } from "../notes.types";
import { NotesService } from "../notes.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Observable, map, take } from "rxjs";

@Component({
    selector: 'notes-detail',
    templateUrl : './notes-detail.component.html'
})
export class NotesDetailComponent
{
    form!: FormGroup;
    dialogTitle: string = "";
    isSaving$: Observable<boolean>;
    
    constructor( @Inject(MAT_DIALOG_DATA) data: Note, 
        private _dialogRef: MatDialogRef<NotesDetailComponent>,
        private _formBuilder: FormBuilder,
        private _noteService: NotesService) {
        this.createForm(data);
        this.dialogTitle = data.id === '' ? 'New Note' : `Editing ${data.id}`;
        this.isSaving$ = this._noteService.state$.pipe(map(state => state.isSaving));
    }

    createForm(note: Note) { this.form = this._formBuilder.group({
            id: note.id,
            title: [note.title, Validators.required],
            text: [note.text],
        });
    }

    saveNote() {
        if (!this.form?.valid) {
            return;
        }

        this._noteService.createOrUpdate(this.form?.value)
            .pipe(take(1))
            .subscribe(() => this.close());
    }

    deleteNote() {
        const id = this.form.get('id')?.value;
        if (id == null || id === '') {
            return;
        }
        this._noteService.deleteNote(id)
            .pipe(take(1))
            .subscribe(() => this.close());
    }

    close() {
        this._dialogRef.close();
    }
}