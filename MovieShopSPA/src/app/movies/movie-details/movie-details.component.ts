import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from 'src/app/core/services/movie.service';
import { Genre } from 'src/app/shared/models/genre';
import { MovieDetails } from 'src/app/shared/models/movie-details';
import { Casts } from 'src/app/shared/models/cast';

@Component({
  selector: 'app-movie-details',
  templateUrl: './movie-details.component.html',
  styleUrls: ['./movie-details.component.css'],
})
export class MovieDetailsComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private movieService: MovieService
  ) {}
  
  movieId: number;
  movieDetails: MovieDetails;
  genres: Genre[];
  casts: Casts[];
  ngOnInit(): void {
    this.route.paramMap.subscribe((p) => {
      this.movieId = +p.get('id');
      // make a call to movie service to get movie details
      this.movieService.getMovieDetails(this.movieId).subscribe((m) => {
        this.movieDetails = m;
        this.genres=this.movieDetails.genres;
        this.casts=this.movieDetails.casts;
      });
    });
  }
}