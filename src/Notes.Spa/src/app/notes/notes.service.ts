import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, catchError, map, of, 
        switchMap, take, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Note, NoteState } from './notes.types';

@Injectable({
    providedIn: 'root'
})
export class NotesService
{
    private _initialState: NoteState = {
        isLoading: false,
        isSaving: false,
        notes: [],
        error: ''
    }

    private _state: BehaviorSubject<NoteState> = new BehaviorSubject(this._initialState);

    constructor(private _httpClient: HttpClient) {
        
    }

    get state$(): Observable<NoteState> {
        return this._state.asObservable();
    }

    public getAll() : Observable<Note[]>
    {
        return this._state.pipe(
            take(1),
            tap(state => this._state.next({...state, isLoading: true})),
            switchMap(state => this._httpClient.get<Note[]>(`${environment.apiUrl}/api/notes/list`).pipe(
                map(notes => { 
                    this._state.next({...state, isLoading: false, notes: notes, error: ''});
                    return notes;
                }),
                catchError(error => {
                    this._state.next({...state, isLoading: false, error: error.message});
                    return of(error);
                })
            )),
            
        )
    }

    public createOrUpdate(note: Note) : Observable<Note>
    {
        const isCreating = note.id === '';
        const action = isCreating ? 'create' : 'update';
        return this._state.pipe(
            take(1),
            tap(state => this._state.next({...state, isSaving: true})),
            switchMap(state => this._httpClient.post<Note>(`${environment.apiUrl}/api/notes/${action}`, note).pipe(
                map(note => { 
                    // When creating, push the new note into notes array.
                    // When updating, replace the old note to the new one.
                    const newNotes = isCreating
                        ? [...state.notes, note] 
                        : state.notes.map(n => n.id === note.id ? note : n);
                    this._state.next({...state, isSaving: false, notes: newNotes, error: ''});
                    return note;
                }),
                catchError(error => {
                    this._state.next({...state, isSaving: false, error: error.message});
                    return of(error);
                })
            )),

        );
    }

    public deleteNote(noteId: string) : Observable<any>
    {
        return this._state.pipe(
            take(1),
            tap(state => this._state.next({...state, isSaving: true})),
            switchMap(state => this._httpClient.delete(`${environment.apiUrl}/api/notes/delete?id=${noteId}`).pipe(
                map(() => { 
                    this._state.next({...state, isSaving: false, 
                            notes: state.notes.filter(n => n.id !== noteId), error: ''});
                    return null;
                }),
                catchError(error => {
                    this._state.next({...state, isSaving: false, error: error.message});
                    return of(error);
                })
            )),
        );
    }
}