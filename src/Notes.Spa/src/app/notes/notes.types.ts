export interface Note
{
    id?: string;
    title?: string;
    text?: string;
}

export interface NoteState
{
    isLoading: boolean;
    isSaving: boolean;
    notes: Note[];
    error: string;
}
