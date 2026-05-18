# ARCANUM STAGE

그래픽 중심의 게임형 타로 앱 기획/개발 저장소입니다.

## Current Documents

- `ARCANUM_STAGE_PRD.md`: 메인 PRD
- `ONTOLOGY_SIMULATION_REPORT.md`: 온톨로지 기반 페르소나 시뮬레이션
- `DEVELOPMENT_SIMULATION_REPORT.md`: Unity/서버/에셋/QA/PM 통합 개발 시뮬레이션
- `ARCANUM_STAGE_QA_RELEASE_SIMULATION.md`: QA/릴리즈 시뮬레이션

## Locked Decisions

- Engine: Unity
- Editor: Unity 6000.3.15f1 (Unity 6.3 LTS)
- Unity template: Universal 2D
- Rendering: 2D URP for MVP
- Target platform: PC Standalone
- Baseline resolution: 16:9, 1920x1080
- Unity MCP: AnkleBreaker Unity MCP, bridge port `7890`
- First scene: Boot
- UI: uGUI for MVP
- Client language: C#
- Server: TypeScript + NestJS
- Database: PostgreSQL
- Graphics: GPT-Image-2 generated assets, reviewed before production use
- MVP deck: Major Arcana 22 cards
- Prototype deck: 6 cards

## Current Build Reality

- Unity project root: `Taro/`
- Sprint 1 target: PC Standalone 16:9 today-card vertical slice
- Implemented scene chain: `Boot -> HomeTable -> Ritual -> ReadingResult`
- Current scene paths: `Taro/Assets/Arcanum/Scenes/*.unity`
- Current code paths: `Taro/Assets/Arcanum/Scripts/`
- Current smoke tests: `Taro/Assets/Arcanum/Tests/Editor/ArcanumSprint1SmokeTests.cs`
- Sprint 1 content scope: prototype 6-card deck, local today-card result, placeholder/approved-only visual assets
- Sprint 1 non-goals: relationship readings, annual forecast, real payment, real ads, Live2D/Spine, mobile layout

## Sprint 2 Candidates

- Expand from today-card to relationship 3-card reading
- Add first SafetyPolicy fixtures for relationship and anxiety-driving copy
- Add ShareRenderer 1920x1080 result image proof
- Start AssetReview registry for GPT-Image-2 candidates
