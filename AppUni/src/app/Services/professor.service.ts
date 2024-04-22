import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; 
import { Professor } from '../Interfaces/professor';

@Injectable({
  providedIn: 'root'
})
export class ProfessorService {

  private apiUrl: string = environment.endpoint + 'Professors/';

  constructor(private http: HttpClient) { }

  getAllProfessors(): Observable<Professor[]> {
    return this.http.get<Professor[]>(this.apiUrl);
  }

  getProfessorById(professorId: number): Observable<Professor> {
    return this.http.get<Professor>(`${this.apiUrl}${professorId}`);
  }

  createProfessor(professor: Professor): Observable<Professor> {
    return this.http.post<Professor>(this.apiUrl, professor);
  }

  updateProfessor(professorId: number, professor: Professor): Observable<Professor> {
    return this.http.put<Professor>(`${this.apiUrl}${professorId}`, professor);
  }

  deleteProfessor(professorId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}${professorId}`);
  }
}
