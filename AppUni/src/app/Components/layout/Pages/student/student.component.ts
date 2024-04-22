import { Component, OnInit } from '@angular/core';
import { Student } from '../../../../Interfaces/student';
import { Course } from '../../../../Interfaces/course';
import { StudentService } from '../../../../Services/student.service';

@Component({
  selector: 'app-student',
  templateUrl: './student.component.html',
  styleUrls: ['./student.component.css']
})
export class StudentComponent implements OnInit {

  students: Student[] = [];
  selectedStudent: Student | undefined;
  enrolledCourses: Course[] = [];

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.studentService.getAllStudents().subscribe(
      (students) => {
        this.students = students;
      },
      (error) => {
        console.error('Error loading students:', error);
      }
    );
  }

  onSelectStudent(student: Student): void {
    this.selectedStudent = student;
    this.loadEnrolledCourses(student.StudentId);
  }

  loadEnrolledCourses(studentId: number): void {
    this.studentService.getEnrolledCourses(studentId).subscribe(
      (courses) => {
        this.enrolledCourses = courses;
      },
      (error) => {
        console.error(`Error loading enrolled courses for student ${studentId}:`, error);
      }
    );
  }

  enrollStudent(studentId: number, courseId: number, professorId: number): void {
    const enrollmentData = {
      studentId,
      courseId,
      professorId
    };

    this.studentService.enrollStudent(enrollmentData).subscribe(
      (result) => {
        if (result) {
          console.log('Student enrolled successfully.');
          // Recargar los cursos inscritos después de la inscripción
          this.loadEnrolledCourses(studentId);
        } else {
          console.error('Failed to enroll student.');
        }
      },
      (error) => {
        console.error('Error enrolling student:', error);
      }
    );
  }

  deleteStudent(studentId: number): void {
    this.studentService.deleteStudent(studentId).subscribe(
      (result) => {
        if (result) {
          console.log(`Student with ID ${studentId} deleted successfully.`);
          // Recargar la lista de estudiantes después de eliminar uno
          this.loadStudents();
          this.selectedStudent = undefined; // Limpiar el estudiante seleccionado
        } else {
          console.error(`Failed to delete student with ID ${studentId}.`);
        }
      },
      (error) => {
        console.error(`Error deleting student with ID ${studentId}:`, error);
      }
    );
  }
}
