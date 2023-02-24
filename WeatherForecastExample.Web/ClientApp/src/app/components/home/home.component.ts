import {Component} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {ApiService} from "../../services/api-service";
import {HttpResponse} from "@angular/common/http";
import {Error, WeatherForecastResponse, WeatherForecastResult} from "../../models/weather-forecast";
import {FormControl, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  public isSearching = false;
  public dataSourceWrappers: DataSourceWrapper[] = [];
  public locationData: LocationData | null = null;
  public error: string | null = null;

  public searchForm = new FormGroup({
    search: new FormControl('', [
      Validators.required,
      Validators.minLength(3)
    ])
  })

  public readonly displayedColumns = [
    'hour',
    'temperature'
  ];

  constructor(private apiService: ApiService) {
  }


  get search() {
    return this.searchForm.get('search');
  }

  public onSubmit(event: any): void {
    event.preventDefault();
    this.isSearching = true;
    this.error = null;

    const search = this.searchForm.controls.search.value ?? '';

    this.apiService.searchForecasts(search)
      .subscribe({
        next: ({body}: HttpResponse<WeatherForecastResponse>) => {
          if (body?.userSafeErrorMessage) {
            this.onFailure(body)
          } else {
            this.onSuccess(body);
          }
        },
        error: (e) => {
          console.log(e);
          this.error = "Something broke";
          this.isSearching = false;
        },
        complete: () => {
          this.isSearching = false;
        }
      })
  }
  private onFailure(error: Error) {
    console.log(error)
    this.error = error?.userSafeErrorMessage;
    this.isSearching = false;
    return;
  }

  private onSuccess(result: WeatherForecastResult | null) {
    this.locationData = <LocationData> {
      locationName: result?.name,
      countryName: result?.country,
      timeZoneName: result?.timeZone,
      temperatureUnit: result?.temperatureUnit
    }

    this.dataSourceWrappers = result?.data?.map(x => <DataSourceWrapper> {
      dataSource: new MatTableDataSource<WeatherForecastTableEntity>(x.temperatures?.map((y: number, i: number) => <WeatherForecastTableEntity>{
        temperature: y,
        hour: i
      })) ?? [],
      Date: x.date
    }) ?? [];
  }
}

interface LocationData {
  locationName: string;
  countryName: string;
  timeZoneName: string;
  temperatureUnit: string;
}

interface DataSourceWrapper {
  dataSource: MatTableDataSource<WeatherForecastTableEntity>;
  Date: Date;
}

interface WeatherForecastTableEntity {
  hour: number;
  temperature: number;
}
