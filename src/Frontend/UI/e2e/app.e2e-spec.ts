import { PoshlogmonitorPage } from './app.po';

describe('poshlogmonitor App', function() {
  let page: PoshlogmonitorPage;

  beforeEach(() => {
    page = new PoshlogmonitorPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
