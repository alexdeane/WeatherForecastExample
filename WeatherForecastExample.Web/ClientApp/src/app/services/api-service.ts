import {Injectable} from "@angular/core";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {WeatherForecastResponse} from "../models/weather-forecast";
import {Observable} from "rxjs";

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {
  }

  public searchForecasts(search: string) : Observable<HttpResponse<WeatherForecastResponse>>{
    return this.http.get<WeatherForecastResponse>(`api/weatherForecast?search=${search}`, {
      observe: "response"
    });
  }
}
