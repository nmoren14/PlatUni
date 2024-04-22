import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LayoutComponent } from './layout.component';
import { StudentComponent } from './Pages/student/student.component';
import { CourseComponent } from './Pages/course/course.component';
import { ProfessorComponent } from './Pages/professor/professor.component';

const routes: Routes = [{
  path:'',
  component:LayoutComponent,
  children:[
    {path:'students',component:StudentComponent},
    {path:'courses',component:CourseComponent},
    {path:'professor',component:ProfessorComponent}
  ]
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
