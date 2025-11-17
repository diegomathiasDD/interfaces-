# Projeto: Interfaces em C# — LSP RANGERS

> Template de repositório para o trabalho de Fases (procedural → OO → interfaces → repository)

## Composição da equipe (obrigatório)

- Diego Mathias — RA: 2705982
- Nicolas Gabriel Brunismann — RA: 2716011
- Luan Cesar Costa — RA: 2706032

## Sumário de Fases

- Fase 0 — Aquecimento conceitual (`src/fase-00-aquecimento`)
- Fase 1 — Heurística antes do código (`src/fase-01-procedural`)
- Fase 2 — Procedural mínimo (`src/fase-02-procedural-minimo`)
    - Entrega da Fase 2 (artefato): `src/fase-02-procedural-minimo/FASE-02-Procedure-Minimo.md`
- Fase 3 — OO sem interface (`src/fase-03-oo-sem-interface`)
    - Entrega da Fase 3 (artefato): `src/fase-03-oo-sem-interface/FASE-03-OO-Sem-Interface.md`
- Fase 4 — Interface plugável e testável (`src/fase-04-com-interfaces`)
    - Entrega da Fase 4 (artefato): `src/fase-04-com-interfaces/FASE-04-Com-Interfaces.md`
- Fase 5 — Essenciais de interfaces em C# (`src/fase-05-essenciais-interfaces-csharp`)
- Fase 6 — ISP na prática (`src/fase-06-isp`)
- Fase 7 — Repository InMemory (`src/fase-07-repository-inmemory`)
- Fase 8 — Repository CSV (`src/fase-08-repository-csv`)
- Fase 9 — Repository JSON (`src/fase-09-repository-json`)
- Fase 10 — Testabilidade: dublês e costuras (`src/fase-10-testabilidade`)
- Fase 11 — Cheiros e antídotos (`src/fase-11-cheiros-antidotos`)
- Fase 12 — Eixos / composição (opcional) (`src/fase-12-eixos-opcional`)
- Fase 13 — Mini-projeto (`src/fase-13-mini-projeto`)

## Estrutura do Repositório

```
repo-raiz/
├── README.md
├── src/
│   ├── fase-00-aquecimento/
│   ├── fase-01-procedural/
│   ├── fase-02-procedural-minimo/
│   ├── fase-03-oo-sem-interface/
│   ├── fase-04-com-interfaces/
│   ├── fase-05-essenciais-interfaces-csharp/
│   ├── fase-06-isp/
│   ├── fase-07-repository-inmemory/
│   ├── fase-08-repository-csv/
│   ├── fase-09-repository-json/
│   ├── fase-10-testabilidade/
│   ├── fase-11-cheiros-antidotos/
│   ├── fase-12-eixos-opcional/
│   └── fase-13-mini-projeto/
├── tests/
│   └── PooInterface.Tests/
└── docs/
    ├── arquitetura/
    └── decisoes/
```

## Como usar este repositório

Cada Fase tem sua própria pasta em `src/` com um `README.md` descrevendo o enunciado, os entregáveis e orientações. O fluxo de entrega simplificado por Fase é:

1. Criar/atualizar a pasta da Fase em `src/` com código e README.
2. Atualizar o README raiz com a composição da equipe e decisões de design (1–3 bullets).
3. Publicar no ClassHero: link do repositório + notas da Fase (entregáveis e observações).

## Checklist de qualidade (aplicar por Fase)

- Contratos coesos e limites claros (interfaces pequenas, responsabilidade única).
- Cliente depende de abstrações; troca de implementação sem alterar cliente.
- Testes unitários sem I/O; uso de dublês onde necessário.
- Evitar switch/case para seleção de comportamento — preferir composição/polimorfismo.
- Mudanças pequenas e localizadas por Fase; histórico coerente no git.

## Evidências e logs

Colocar prints curtos ou logs mínimos em `docs/` se fizer sentido para a Fase (ex.: saída de execução, resumo de testes).

## Como executar

Para compilar e rodar os projetos de todas as fases:

```powershell
# Compilar toda a solução
dotnet build pOOInterface.sln

# Rodar a aplicação principal (Fases 2-4 integradas)
cd src/PooInterface.App
dotnet run

# Executar todos os testes (Fases 2-4)
cd ../../tests/PooInterface.Tests
dotnet test

# Executar teste verbose (com detalhes)
dotnet test -v detailed
```

## Testes por Fase

- **Fase 2 (Procedural mínimo):** Testes em `FormatterTests.cs` — valida switch/if
- **Fase 3 (OO sem interface):** Testes em `Phase3OOFormatterTests` — valida polimorfismo
- **Fase 4 (Interface plugável):** Testes em `Phase4InterfaceFormatterTests` — valida injeção de dependência e dublês