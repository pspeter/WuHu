import { Angular2ClientPage } from './app.po';

describe('angular2-client App', function() {
  let page: Angular2ClientPage;

  beforeEach(() => {
    page = new Angular2ClientPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
