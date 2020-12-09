import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  constructor(private apiService: ApiService) { }

  getAllGenres(){
    this.apiService.getAll('genres');
  }
}
