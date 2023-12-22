import { test, expect } from '@playwright/test';

test('should registrate user', async ({ page }) => {
  await page.goto('http://localhost:4200/register');

  await page.fill('input[formControlName="email"]', 'user+hotmail@hotmail.com');
  await page.fill('input[formControlName="username"]', 'user');
  await page.selectOption('select[formControlName="role"]', 'User');
  await page.fill('input[formControlName="password"]', 'Qwerty1234%');

  await page.click('button[type="submit"]');

  await expect(page).toHaveURL('http://localhost:4200/login');
  await expect(page.locator('button[type="submit"]')).toBeDisabled();
});
