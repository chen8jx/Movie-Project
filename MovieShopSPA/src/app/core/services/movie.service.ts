import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Movie } from 'src/app/shared/models/movie';
import { MovieDetails } from 'src/app/shared/models/movie-details';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class MovieService {
  constructor(private apiService:ApiService) { }
  getTopRevenueMovies():Observable<Movie[]>{
    return this.apiService.getAll('movies/toprevenue')
  }

  getMovieDetails(id:number):Observable<MovieDetails>{
    return this.apiService.getOne('movies',id)
  }

  getMoviesByGenre(id:number):Observable<Movie[]> {
    return this.apiService.getAll('movies/genre',id)
  }
}