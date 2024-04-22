import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { StudentComponent } from './Pages/student/student.component';
import { CourseComponent } from './Pages/course/course.component';
import { ProfessorComponent } from './Pages/professor/professor.component';
import { SharedModule } from 'src/app/Reutilizable/shared/shared.module';


@NgModule({
  declarations: [
    StudentComponent,
    CourseComponent,
    ProfessorComponent
  ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    SharedModule
  ]
})
export class LayoutModule { }
