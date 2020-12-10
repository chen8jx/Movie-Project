import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MovieService } from 'src/app/core/services/movie.service';
import { Movie } from '../shared/models/movie';

@Component({
  selector: 'app-movies',
  templateUrl: './movies.component.html',
  styleUrls: ['./movies.component.css']
})
export class MoviesComponent implements OnInit {

  movies: Movie[];
  genreId: number;
  constructor(private route: ActivatedRoute, private movieService: MovieService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe((g) => {
      this.genreId = +g.get('id');
      //console.log(this.genreId);
      // make a call to movie service to get movie details
      this.movieService.getMoviesByGenre(this.genreId).subscribe((m) => {
        this.movies = m;
        //console.log(this.movies);
      });
    });
  }

}
