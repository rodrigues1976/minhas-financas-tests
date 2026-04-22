import { describe, it, expect } from 'vitest'

function podeCriarTransacao(idade: number, tipo: string) {
  if (idade < 18 && tipo === 'receita') return false
  return true
}

describe('Regras de Transação', () => {
  it('não deve permitir receita para menor de idade', () => {
    const permitido = podeCriarTransacao(15, 'receita')
    expect(permitido).toBe(false)
  })

  it('deve permitir despesa para menor', () => {
    const permitido = podeCriarTransacao(15, 'despesa')
    expect(permitido).toBe(true)
  })
})