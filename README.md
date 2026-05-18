# ARCANUM STAGE

그래픽 기반 PC Standalone 타로 상담 게임 프로젝트입니다. Sprint 1의 목표는 사용자가 메인 테이블에서 오늘의 카드 상담을 시작하고, 뒷면 카드 한 장을 직접 고른 뒤, 타로 마스터 루나의 짧은 상담 노트를 받는 수직 슬라이스입니다.

## 현재 문서

- `ARCANUM_STAGE_PRD.md`: 제품 방향과 주요 리딩 경험 정의
- `ONTOLOGY_SIMULATION_REPORT.md`: 카드, 리딩, 사용자 상태를 잇는 온톨로지 검토
- `DEVELOPMENT_SIMULATION_REPORT.md`: Unity, 서버, 에셋, QA, PM 개발 시뮬레이션
- `ARCANUM_STAGE_QA_RELEASE_SIMULATION.md`: QA와 릴리즈 리스크 검토
- `ARCANUM_STAGE_LOCALIZATION_COPY_KO.md`: 한국어 게임 상담 톤과 Sprint 1 UI 네이밍 기준

## 확정 사항

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

## 현재 구현

- Unity project root: `Taro/`
- Sprint 1 target: PC Standalone 16:9 오늘의 카드 상담 수직 슬라이스
- Implemented scene chain: `Boot -> HomeTable -> Ritual -> ReadingResult`
- Current scene paths: `Taro/Assets/Arcanum/Scenes/*.unity`
- Current code paths: `Taro/Assets/Arcanum/Scripts/`
- Current smoke tests: `Taro/Assets/Arcanum/Tests/Editor/ArcanumSprint1SmokeTests.cs`
- Sprint 1 content scope: 프로토타입 6장 덱, 로컬 오늘의 카드 결과, placeholder/approved-only visual assets
- Art pipeline docs: `Taro/Assets/Arcanum/Art/README.md`, `VisualStyleGuide.md`, `GenerationBacklog.md`, `AssetReview.md`
- GPT-Image-2 target coverage: 메인화면, 프로필생성, 타로보기 파이프라인 에셋과 green-screen alpha 규칙
- Sprint 1 non-goals: 관계 리딩, 연간 운세, 실제 결제, 실제 광고, Live2D/Spine, 모바일 레이아웃

## Sprint 1 사용자 흐름 네이밍

- 메인화면: `아르카눔 스테이지`
- 프로필 준비: `첫 상담 준비`
- 타로 보기: `오늘의 카드 보기`
- 카드 선택: `눈길이 머무는 카드`
- 결과 화면: `오늘 열린 아르카나`
- 상담자: `타로 마스터 루나`

## Sprint 2 후보

- 오늘의 카드에서 관계 3장 리딩으로 확장
- 관계/불안 유발 문구용 SafetyPolicy fixture 추가
- ShareRenderer 1920x1080 결과 이미지 proof 추가
- GPT-Image-2 후보 관리를 위한 AssetReview registry 시작

## QA 검증 절차

릴리즈 QA는 사용자 노출 경로를 **메인화면 -> 프로필생성 -> 타로 보기**로 본다. 이 파이프라인을 추가하는 동안 기존 Sprint 1 smoke chain(`Boot -> HomeTable -> Ritual -> ReadingResult`)도 계속 통과해야 한다.

1. Unity 프로젝트 `Taro/`를 연다.
2. `Taro/Assets/Arcanum/Tests/Editor/ArcanumSprint1SmokeTests.cs` EditMode smoke tests를 실행한다.
3. QA 문서에 동일한 릴리즈 경로 `메인화면 -> 프로필생성 -> 타로 보기`가 있는지 확인한다.
4. PC Standalone Windowed 모드 `1920x1080`에서 빌드 또는 실행한다.
5. PC 16:9 해상도 `1920x1080`, `1600x900`, `1280x720` 스크린샷을 캡처한다.
6. 메인화면, 프로필생성, 타로 보기의 한국어 UI를 확인한다. 한글 깨짐, 영어 placeholder, debug key text가 없어야 한다.
7. 프로필 정보가 null/empty label 없이 타로 보기 단계에 전달되는지 확인한다.
8. PC 16:9에서 카드 영역, 타로 마스터, 하단 대화창, 프로필 입력 영역, CTA가 겹치지 않는지 확인한다.

릴리즈 차단: 메인화면 -> 프로필생성 -> 타로 보기 경로, 한국어 UI 가독성, 1920x1080 레이아웃 acceptance 중 P0 결함이 있으면 RC sign-off를 보류한다.
