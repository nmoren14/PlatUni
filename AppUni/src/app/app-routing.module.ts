import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentComponent } from './Components/layout/Pages/student/student.component';
import { AppComponent } from './app.component';

const routes: Routes = [
  {path:'',redirectTo:'pages',pathMatch:'full'},
  { path: 'Index', component: AppComponent },
  { path: 'pages', loadChildren: () => import('./Components/layout/layout.module').then(x => x.LayoutModule) }
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
