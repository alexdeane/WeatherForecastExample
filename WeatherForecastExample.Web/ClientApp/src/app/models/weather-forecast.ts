export interface WeatherForecastResult {
  name: string;
  data?: WeatherForecastData[];
  timeUnit?: string;
  temperatureUnit?: string;
}

export interface WeatherForecastData {
  date?: Date;
  temperatures?: number[];
}
