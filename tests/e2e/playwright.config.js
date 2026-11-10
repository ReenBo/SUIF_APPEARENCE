const { defineConfig } = require('@playwright/test');

module.exports = defineConfig({
  testDir: '.',
  timeout: 300000,
  use: {
    browserName: 'chromium',
    launchOptions: {
      args: [
        '--enable-webgl',
        '--use-gl=angle',
        '--use-angle=swiftshader',
        '--ignore-gpu-blocklist',
        '--enable-gpu-rasterization',
        '--no-sandbox',
        '--disable-setuid-sandbox'
      ]
    }
  }
});
