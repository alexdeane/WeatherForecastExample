export interface WeatherForecastResult {
  name: string;
  country: string;
  timeZone: string;
  data?: WeatherForecastData[];
  timeUnit?: string;
  temperatureUnit?: string;
}

export interface Error {
  userSafeErrorMessage: string;
}

export type WeatherForecastResponse = WeatherForecastResult & Error;

export interface WeatherForecastData {
  date?: Date;
  temperatures?: number[];
}
