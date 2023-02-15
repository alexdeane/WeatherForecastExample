import {Injectable} from "@angular/core";
import {HttpClient, HttpResponse} from "@angular/common/http";
import {ServiceResult, WeatherForecastResult} from "../models/weather-forecast";
import {Observable} from "rxjs";

@Injectable()
export class ApiService {
  constructor(private http: HttpClient) {
  }

  public searchForecasts(search: string) : Observable<HttpResponse<ServiceResult<WeatherForecastResult>>>{
    return this.http.get<ServiceResult<WeatherForecastResult>>(`api/weatherForecast?search=${search}`, {
      observe: "response"
    });
  }
}
