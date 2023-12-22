import { test as setup, expect } from '@playwright/test';
import { STORAGE_STATE } from '../playwright.config';

setup('do login', async ({ page }) => {
  await page.goto('http://localhost:4200/register');

  await page.fill(
    'input[formControlName="email"]',
    'kostyanovytskyi+MWSAdmin22@gmail.com'
  );
  await page.fill('input[formControlName="username"]', 'MwsAdmin22');
  await page.selectOption('select[formControlName="role"]', 'User');
  await page.fill('input[formControlName="password"]', 'Qwerty1234%');
  await page.click('button[type="submit"]');
  await page.goto('http://localhost:4200/login');

  await page.fill(
    'input[formControlName="email"]',
    'kostyanovytskyi+MWSAdmin22@gmail.com'
  );
  await page.fill('input[formControlName="password"]', 'Qwerty1234%');

  await page.click('button[type="submit"]');

  await expect(page).toHaveURL('http://localhost:4200/tasks');
  await page.context().storageState({ path: STORAGE_STATE });
});
