import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LetNavComponent } from './let-nav.component';

describe('LetNavComponent', () => {
  let component: LetNavComponent;
  let fixture: ComponentFixture<LetNavComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LetNavComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LetNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
