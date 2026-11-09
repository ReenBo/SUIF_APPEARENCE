const { defineConfig, devices } = require('@playwright/test');

module.exports = defineConfig({
  testDir: '.',
  timeout: 180000,
  use: {
    browserName: 'chromium',
    launchOptions: {
      args: [
        '--enable-webgl',
        '--use-gl=angle',
        '--ignore-gpu-blocklist',
        '--enable-gpu-rasterization',
        '--no-sandbox'
      ]
    }
  }
});

