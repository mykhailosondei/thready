import {AfterViewInit, Component, ElementRef, Input, ViewChild} from '@angular/core';

@Component({
  selector: 'app-auto-resize-input',
  templateUrl: './auto-resize-input.component.html',
  styleUrls: ['./auto-resize-input.component.scss']
})
export class AutoResizeInputComponent {
  inputValue: string = ''; // Property to bind to the input
  @ViewChild('textArea') textArea: ElementRef<HTMLTextAreaElement>; // Reference to the textarea element

  @Input("placeholder") placeholder: string = "What's on your mind?"; // Placeholder text for the input
  @Input() public startingText: string = ''; // Starting text for the input

  ngOnInit(): void {
    this.resizeTextArea();
    this.inputValue = this.startingText;
  }

  resizeTextArea() {
    if(this.textArea){
    const element = this.textArea.nativeElement;
    element.style.height = 'auto'; // Reset the height to auto
    element.style.height = element.scrollHeight + 'px'; // Set the height to match the scrollHeight
    }
  }
}
