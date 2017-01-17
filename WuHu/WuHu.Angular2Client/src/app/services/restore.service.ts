import {Injectable} from '@angular/core';

@Injectable()
export class RestoreService<T> {
  originalItem: T;
  currentItem: T;

  setItem(item: T) {
    this.originalItem = item;
    this.currentItem = this.clone(item);
  }

  getItem(): T {
    return this.currentItem;
  }

  getItemFinal(): T {
    this.setItem(this.currentItem);
    return this.getItem();
  }

  restoreItem(): T {
    this.currentItem = this.clone(this.originalItem);
    return this.getItem();
  }

  clone(item: T): T {
    // super poor clone implementation
    return JSON.parse(JSON.stringify(item));
  }

  reset(): void {
      this.originalItem = null;
      this.currentItem = null;
  }
}
