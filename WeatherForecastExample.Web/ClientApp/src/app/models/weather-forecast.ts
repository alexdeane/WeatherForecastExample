export interface ServiceResult<T> {
  result?: T;
  userSafeError?: string;
}

export interface WeatherForecastResult {
  name: string;
  country: string;
  timeZone: string;
  data?: WeatherForecastData[];
  timeUnit?: string;
  temperatureUnit?: string;
}

export interface WeatherForecastData {
  date?: Date;
  temperatures?: number[];
}
