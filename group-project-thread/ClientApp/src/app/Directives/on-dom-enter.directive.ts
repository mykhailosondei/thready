import { Directive, ElementRef, EventEmitter, Output, Renderer2, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[appOnDomEnter]'
})
export class OnDomEnterDirective implements AfterViewInit {
  @Output() domEnter: EventEmitter<void> = new EventEmitter<void>();

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    const element = this.el.nativeElement;
    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting) {
        // Element has entered the viewport
        this.domEnter.emit();
      }
    });

    // Start observing the element
    observer.observe(element);
  }
}
