import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'timeSpan'
})
export class TimeSpanPipe implements PipeTransform {

  /**
   * Converts int to timestamp
   * @param value An integer representing the hour in a day
   * @param args
   */
  transform(value: number, ...args: unknown[]): string {
    const date = new Date();

    date.setTime(0);
    date.setHours(value);

    return date.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit'
    });
  }
}
