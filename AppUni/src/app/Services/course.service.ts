import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Course } from '../Interfaces/course';
import { environment } from '../../environments/environment'; 

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  private apiUrl: string = environment.endpoint + 'Courses/';

  constructor(private http: HttpClient) { }

  getAllCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(`${this.apiUrl}/GetAllCourses`);
  }

  getCourseById(id: number): Observable<Course> {
    return this.http.get<Course>(`${this.apiUrl}/${id}`);
  }

  createCourse(Course: Course): Observable<Course> {
    return this.http.post<Course>(`${this.apiUrl}/CreateCourse`, Course);
  }

  updateCourse(id: number, Course: Course): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, Course);
  }

  deleteCourse(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}
