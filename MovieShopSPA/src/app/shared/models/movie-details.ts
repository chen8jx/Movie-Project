export interface MovieDetails{
    id: number;
    title: string;
    posterUrl: string;
    backdropUrl: string;
    rating: null;
    overview: string;
    tagline: string;
    budget?: number;
    revenue?: number;
    imdbUrl: string;
    tmdbUrl: string;
    releaseDate?: Date;
    runTime?: number;
    price?: number;
    favoritesCount: number;
    casts: any[];
    genres: any[];
}