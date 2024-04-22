import { Component, OnInit } from '@angular/core';
import { ProfessorService } from '../../../../Services/professor.service';
import { Professor } from '../../../../Interfaces/professor';

@Component({
  selector: 'app-professor',
  templateUrl: './professor.component.html',
  styleUrls: ['./professor.component.css']
})
export class ProfessorComponent implements OnInit {

  professors: Professor[] = [];

  constructor(private professorService: ProfessorService) { }

  ngOnInit(): void {
    this.loadProfessors();
  }

  loadProfessors(): void {
    this.professorService.getAllProfessors().subscribe(
      (data: Professor[]) => {
        this.professors = data;
      },
      (error) => {
        console.log('Error fetching professors:', error);
      }
    );
  }

  deleteProfessor(professorId: number): void {
    this.professorService.deleteProfessor(professorId).subscribe(
      (result) => {
        if (result) {
          console.log('Professor deleted successfully.');
          // Recargar la lista de profesores después de eliminar
          this.loadProfessors();
        } else {
          console.log('Failed to delete professor.');
        }
      },
      (error) => {
        console.log('Error deleting professor:', error);
      }
    );
  }

}
