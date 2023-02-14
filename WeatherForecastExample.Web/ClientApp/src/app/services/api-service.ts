import {Injectable} from "@angular/core";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {Observable} from "rxjs";
import {WeatherForecastResult} from "../models/weather-forecast";
import { environment } from '../../environments/environment'

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {
  }

  public searchForecasts(search: string) : Observable<HttpResponse<WeatherForecastResult>>{
    return this.http.get<WeatherForecastResult>(`${environment.apiBaseUrl}api/weatherForecast?search=${search}`, {
      observe: "response"
    });
  }
}
