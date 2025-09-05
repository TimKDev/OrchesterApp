import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private currentTheme = new BehaviorSubject<string>('dark');
  public theme$ = this.currentTheme.asObservable();

  constructor() {
    const savedTheme = localStorage.getItem('theme') || 'dark';
    this.setTheme(savedTheme);
  }

  setTheme(theme: string) {
    this.currentTheme.next(theme);
    localStorage.setItem('theme', theme);

    document.body.classList.remove('light-theme', 'dark-theme');
    document.body.classList.add(`${theme}-theme`);

    const metaThemeColor = document.querySelector('meta[name="color-scheme"]');
    if (metaThemeColor) {
      metaThemeColor.setAttribute('content', theme === 'dark' ? 'dark' : 'light');
    }
  }

  toggleTheme() {
    const currentTheme = this.currentTheme.value;
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    this.setTheme(newTheme);
  }

  getCurrentTheme(): string {
    return this.currentTheme.value;
  }

  isDarkMode(): boolean {
    return this.currentTheme.value === 'dark';
  }
}