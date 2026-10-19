import { ChangeDetectorRef, NgZone, OnDestroy, Pipe, PipeTransform } from '@angular/core';

// Ivy uyumlu yerel timeAgo pipe'i (eski 'time-ago-pipe' paketinin yerine).
@Pipe({
  name: 'timeAgo',
  pure: false
})
export class TimeAgoPipe implements PipeTransform, OnDestroy {
  private timer: number | null;

  constructor(private changeDetectorRef: ChangeDetectorRef, private ngZone: NgZone) {}

  transform(value: any): string {
    this.removeTimer();
    const d = new Date(value);
    const now = new Date();
    const seconds = Math.round(Math.abs((now.getTime() - d.getTime()) / 1000));
    const timeToUpdate = Number.isNaN(seconds) ? 1000 : this.getSecondsUntilUpdate(seconds) * 1000;
    this.timer = this.ngZone.runOutsideAngular(() => {
      if (typeof window !== 'undefined') {
        return window.setTimeout(() => {
          this.ngZone.run(() => this.changeDetectorRef.markForCheck());
        }, timeToUpdate);
      }
      return null;
    });
    const minutes = Math.round(Math.abs(seconds / 60));
    const hours = Math.round(Math.abs(minutes / 60));
    const days = Math.round(Math.abs(hours / 24));
    const months = Math.round(Math.abs(days / 30.416));
    const years = Math.round(Math.abs(days / 365));
    if (Number.isNaN(seconds)) {
      return '';
    } else if (seconds <= 45) {
      return 'a few seconds ago';
    } else if (seconds <= 90) {
      return 'a minute ago';
    } else if (minutes <= 45) {
      return minutes + ' minutes ago';
    } else if (minutes <= 90) {
      return 'an hour ago';
    } else if (hours <= 22) {
      return hours + ' hours ago';
    } else if (hours <= 36) {
      return 'a day ago';
    } else if (days <= 25) {
      return days + ' days ago';
    } else if (days <= 45) {
      return 'a month ago';
    } else if (days <= 345) {
      return months + ' months ago';
    } else if (days <= 545) {
      return 'a year ago';
    }
    return years + ' years ago';
  }

  ngOnDestroy(): void {
    this.removeTimer();
  }

  private removeTimer() {
    if (this.timer) {
      window.clearTimeout(this.timer);
      this.timer = null;
    }
  }

  private getSecondsUntilUpdate(seconds: number): number {
    const min = 60;
    const hr = min * 60;
    const day = hr * 24;
    if (seconds < min) {
      return 2; // 1 dakikadan kısa: 2 sn'de bir
    } else if (seconds < hr) {
      return 30; // 1 saatten kısa: 30 sn'de bir
    } else if (seconds < day) {
      return 300; // 1 günden kısa: 5 dk'da bir
    }
    return 3600; // diğer: saatte bir
  }
}
