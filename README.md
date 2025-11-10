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
- Fase 3 — OO sem interface (`src/fase-03-oo-sem-interface`)
- Fase 4 — Interface plugável e testável (`src/fase-04-com-interfaces`)
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

Para compilar e rodar os projetos:

```powershell
# Compilar os projetos
dotnet build src/PooInterface.Core/PooInterface.Core.csproj
dotnet build src/PooInterface.App/PooInterface.App.csproj

# Rodar a demo
cd src/PooInterface.App
dotnet run

# Executar os testes
cd ../../tests/PooInterface.Tests
dotnet test
```