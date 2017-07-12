import { EagleEyeScriptLogMonitorPage } from './app.po';

describe('EagleEye-ScriptLogMonitor App', function() {
  let page: EagleEyeScriptLogMonitorPage;

  beforeEach(() => {
    page = new EagleEyeScriptLogMonitorPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
