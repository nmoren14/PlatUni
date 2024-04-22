import { Component, OnInit } from '@angular/core';
import { CourseService } from '../../../../Services/course.service';
import { Course } from '../../../../Interfaces/course';

@Component({
  selector: 'app-courses',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit {

  courses: Course[] = [];

  constructor(private courseService: CourseService) { }

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.courseService.getAllCourses().subscribe(
      (data) => {
        this.courses = data;
      },
      (error) => {
        console.log('Error fetching courses:', error);
      }
    );
  }

  createCourse(Course: Course): void {
    this.courseService.createCourse(Course).subscribe(
      (createdCourse) => {
        console.log('Course created successfully:', createdCourse);
        // Actualizar la lista de cursos después de crear uno nuevo
        this.loadCourses();
      },
      (error) => {
        console.log('Error creating course:', error);
      }
    );
  }

  deleteCourse(id: number): void {
    this.courseService.deleteCourse(id).subscribe(
      () => {
        console.log(`Course with ID ${id} deleted successfully.`);
        // Actualizar la lista de cursos después de eliminar uno
        this.loadCourses();
      },
      (error) => {
        console.log(`Error deleting course with ID ${id}:`, error);
      }
    );
  }
}
