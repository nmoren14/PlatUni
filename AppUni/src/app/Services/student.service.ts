import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment'; 
import { Student } from '../Interfaces/student';
import { Course } from '../Interfaces/course';

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  private apiUrl: string = environment.endpoint + 'Students/';

  constructor(private http: HttpClient) { }

  getAllStudents(): Observable<Student[]> {
    return this.http.get<Student[]>(this.apiUrl);
  }

  getStudentById(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}${id}`);
  }

  createStudent(student: Student): Observable<Student> {
    return this.http.post<Student>(this.apiUrl, student);
  }

  updateStudent(studentId: number, student: Student): Observable<Student> {
    return this.http.put<Student>(`${this.apiUrl}${studentId}`, student);
  }

  deleteStudent(studentId: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}${studentId}`);
  }

  enrollStudent(enrollmentData: any): Observable<boolean> {
    return this.http.post<boolean>(`${this.apiUrl}EnrollStudentAsync`, enrollmentData);
  }

  getEnrolledCourses(studentId: number): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.apiUrl}${studentId}/EnrolledCourses`);
  }
}