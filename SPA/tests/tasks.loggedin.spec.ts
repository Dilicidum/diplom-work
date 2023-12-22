import { test, expect } from '@playwright/test';

let name = 'Test Task';
test.beforeEach(async ({ page }) => {
  await page.goto('http://localhost:4200/create-task');

  await page.fill('input[formControlName="description"]', 'Task description');

  await page.fill('input[formControlName="name"]', name);
  await page.selectOption('select[formControlName="category"]', 'University');
  await page.selectOption('select[formControlName="status"]', 'None');
  await page.fill('input[formControlName="dueDate"]', '2024-12-31');

  await page.click('button[type="submit"]');
  await expect(page).toHaveURL('http://localhost:4200/tasks');
});

test('Should edit task and delete', async ({ page }) => {
  await page.goto('http://localhost:4200/tasks');
  await expect(page).toHaveURL('http://localhost:4200/tasks');

  await page.click('button:text("Edit")');

  await page.fill('#description', 'Edited Task Description');
  await page.fill('#date', '2024-01-01');

  await page.click('button:text("Submit")');

  await expect(page.locator('#description')).toHaveValue(
    'Edited Task Description'
  );

  await page.click('button:text("Delete")');

  await expect(page.locator('#description')).toHaveCount(0);
});
