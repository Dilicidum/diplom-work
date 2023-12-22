import { test, expect } from '@playwright/test';

test('should login user', async ({ page }) => {
  await page.goto('http://localhost:4200/login');

  await page.fill(
    'input[formControlName="email"]',
    'kostyanovytskyi+MWSAdmin22@gmail.com'
  );
  await page.fill('input[formControlName="password"]', 'Qwerty1234%');

  await page.click('button[type="submit"]');

  await expect(page).toHaveURL('http://localhost:4200/tasks');
});
