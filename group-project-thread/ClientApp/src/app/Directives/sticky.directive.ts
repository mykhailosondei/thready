import {Directive, ElementRef, HostListener, Renderer2} from '@angular/core';

@Directive({
  selector: '[appSticky]'
})
export class StickyDirective {

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  @HostListener('window:scroll', ['$event'])
  onScroll(event: Event) {
    const scrollY = window.scrollY;
    const element = this.el.nativeElement;
{
      this.renderer.setStyle(element, 'margin-top', `${scrollY}px`);
    }
  }

}
