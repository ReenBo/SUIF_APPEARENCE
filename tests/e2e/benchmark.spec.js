const { test, expect } = require('@playwright/test');

test('SUIF WebGL Automated Performance Benchmark', async ({ page }) => {
  const targetUrl = process.env.WEBGL_URL || 'http://localhost:8080/?autotest=true';
  console.log(`Navigating to WebGL instance: ${targetUrl}`);

  await page.goto(targetUrl);

  await page.waitForSelector('#unity-canvas', { timeout: 60000 });
  console.log('Unity Canvas loaded. Waiting for SUIF benchmark completion...');

  const benchmarkHandle = await page.waitForFunction(
    () => window.suifBenchmarkResult,
    { timeout: 120000, polling: 500 }
  );

  const metrics = await benchmarkHandle.jsonValue();
  console.log('\n=============================================');
  console.log('   SUIF WebGL Telemetry Benchmark Results    ');
  console.log('=============================================');
  console.log(`Average FPS:         ${metrics.averageFps.toFixed(1)}`);
  console.log(`1% Low FPS:          ${metrics.onePercentLowFps.toFixed(1)}`);
  console.log(`Average Frame Time:  ${metrics.averageFrameTimeMs.toFixed(2)} ms`);
  console.log(`Max Frame Time:      ${metrics.maxFrameTimeMs.toFixed(2)} ms`);
  console.log(`Peak Draw Calls:     ${metrics.currentDrawCalls}`);
  console.log(`Peak Batches:        ${metrics.currentBatches}`);
  console.log(`Memory Usage:        ${metrics.totalUsedMemoryMb.toFixed(1)} MB`);
  console.log(`Total Frames:        ${metrics.totalFrames}`);
  console.log(`Status:              ${metrics.status}`);
  console.log('=============================================\n');

  expect(metrics.status).toBe('COMPLETED');
  expect(metrics.averageFps).toBeGreaterThanOrEqual(50.0);
  expect(metrics.onePercentLowFps).toBeGreaterThanOrEqual(30.0);
});

