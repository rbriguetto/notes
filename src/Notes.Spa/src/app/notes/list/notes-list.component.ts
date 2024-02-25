import { Component, OnInit } from "@angular/core";
import { Observable, take } from "rxjs";
import { Note, NoteState } from "../notes.types";
import { NotesService } from "../notes.service";
import { MatDialog } from "@angular/material/dialog";
import { NotesDetailComponent } from "../detail/notes-detail.component";

@Component({
    selector: 'notes-list',
    templateUrl: './notes-list.component.html'
})
export class NotesListComponent implements OnInit
{
    state$: Observable<NoteState>;

    /**
     *
     */
    constructor(private _notesService: NotesService, 
            private _matDialog: MatDialog) 
    {
        this.state$ = this._notesService.state$;
    }

    ngOnInit(): void {
        this._notesService.getAll().pipe(take(1))
            .subscribe();
    }

    addNewNote()
    {
        this.showDetail({
                id: '',
                title: '',
                text: ''
            });
    }

    editNote(note: Note)
    {
        this.showDetail(note);
    }

    showDetail(note: Note)
    {
        this._matDialog.open(NotesDetailComponent, {
            width: '800px',
            height: '500px',
            data: note
        });
    }
}