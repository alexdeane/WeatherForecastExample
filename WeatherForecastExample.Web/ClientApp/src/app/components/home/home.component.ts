import {AfterViewInit, Component} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {ApiService} from "../../services/api-service";
import {HttpResponse} from "@angular/common/http";
import {WeatherForecastResult} from "../../models/weather-forecast";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements AfterViewInit {
  public isSearching = false;
  public dataSourceWrappers: DataSourceWrapper[] = [];
  public locationName: string | undefined = undefined;
  public temperatureUnit: string | undefined = undefined;

  public error: string | null = null;

  public readonly displayedColumns = [
    'hour',
    'temperature'
  ];

  constructor(private apiService: ApiService) {
  }

  ngAfterViewInit(): void {
    this.apiService.searchForecasts('bal')
      .subscribe({
        next: (res: HttpResponse<WeatherForecastResult>) => {
          this.locationName = res.body?.name;
          this.temperatureUnit = res.body?.temperatureUnit;
          this.dataSourceWrappers = res.body?.data?.map(x => <DataSourceWrapper>{
            dataSource: new MatTableDataSource<WeatherForecastTableEntity>(x.temperatures?.map((y: number, i: number) => <WeatherForecastTableEntity>{
              temperature: y,
              hour: i
            })) ?? [],
            Date: x.date
          }) ?? [];
        },
        error: (e) => {
          console.log(e);
          this.error = "Something broke";
        },
        complete: () => {
          this.isSearching = false;
        }
      })
  }
}

interface DataSourceWrapper {
  dataSource: MatTableDataSource<WeatherForecastTableEntity>;
  Date: Date;
}

export interface WeatherForecastTableEntity {
  hour: number;
  temperature: number;
}
