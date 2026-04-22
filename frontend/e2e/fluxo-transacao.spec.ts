import { test, expect } from '@playwright/test'

test('não deve permitir receita para menor (fluxo completo)', async ({ page }) => {
  await page.goto('http://localhost:5173')

  await page.click('text=Nova Pessoa')
  await page.fill('input[name=nome]', 'João Teste')
  await page.fill('input[name=dataNascimento]', '2012-01-01')
  await page.click('text=Salvar')

  await page.click('text=Nova Transação')
  await page.fill('input[name=descricao]', 'Teste inválido')
  await page.fill('input[name=valor]', '100')
  await page.selectOption('select[name=tipo]', 'Receita')

  await page.click('text=Salvar')

  await expect(page.locator('text=erro')).toBeVisible()
})