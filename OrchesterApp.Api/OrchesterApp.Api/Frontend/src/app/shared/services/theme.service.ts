import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private currentTheme = new BehaviorSubject<string>('light');
  public theme$ = this.currentTheme.asObservable();

  constructor() {
    // Check if there's a saved theme in localStorage, otherwise default to light
    const savedTheme = localStorage.getItem('theme') || 'light';
    this.setTheme(savedTheme);
  }

  setTheme(theme: string) {
    this.currentTheme.next(theme);
    localStorage.setItem('theme', theme);

    // Apply theme to body element
    document.body.classList.remove('light-theme', 'dark-theme');
    document.body.classList.add(`${theme}-theme`);

    // Update the color-scheme meta tag
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