import { Component, OnInit } from '@angular/core';
import { ComparisonService } from 'src/app/Services/Comparison/comparison.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-plagiarism-test',
  templateUrl: './plagiarism-test.component.html',
  styleUrls: ['./plagiarism-test.component.scss']
})
export class PlagiarismTestComponent implements OnInit {

  fileType: string;                   //type of file being sent to engine
  fileSelector: HTMLElement;          //file selector
  fileTypes = [ "cpp", "java", "c" ]; //types of files that can be submitted

  /**
   * Constructs the PlagiarismTest component used for testing the plagiarism detection engine
   * 
   * @param comparisonService service for sending files to the comparison engine
   * @param aRoute activated route
   * @param router router for navigation
   */
  constructor(private comparisonService: ComparisonService, private aRoute: ActivatedRoute, private router: Router) { }

  //Adds event listener to the file selector on initialization
  ngOnInit() {
    this.fileSelector = document.getElementById('fileSelector');
    this.fileSelector.addEventListener('change', (event) => {
      this.invalid("files");
    });
  }

  /**
   * Checks if form input is valid
   * 
   * @param files file name 
   */
  invalid(files): boolean {
    if(files.length == 0 || this.fileType == null || this.fileType == undefined)
      return true;
    return false;
  }

  /**
   * Sends the files to the comparison engine
   * 
   * @param files file name
   */
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
 
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.comparisonService.test(this.fileType, formData).subscribe(x => {
      this.router.navigate(['/compare/', x.guid]);
    });
  }

}
