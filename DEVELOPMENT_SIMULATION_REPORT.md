# ARCANUM STAGE 개발 시뮬레이션 보고서

작성일: 2026-05-18  
기준 문서: `ARCANUM_STAGE_PRD.md`, `ONTOLOGY_SIMULATION_REPORT.md`, `ARCANUM_STAGE_QA_RELEASE_SIMULATION.md`  
시뮬레이션 방식: Unity 클라이언트, 서버/API, AI 에셋 파이프라인, QA/릴리즈, 프로덕트 오너 관점의 독립 서브에이전트 검토 통합

---

## 1. 결론 요약

ARCANUM STAGE는 Unity로 구현 가능하다. 다만 처음부터 “완성형 타로 앱”을 만들면 카드 수, 그래픽 일관성, 리딩 문장 조합, 결제, QA가 동시에 폭발한다. 따라서 첫 개발은 **6주 MVP**로 잘라야 한다.

MVP의 핵심 목표는 다음 하나다.

> 사용자가 타로 마스터가 있는 JRPG식 화면에서 카드를 직접 뽑고, 오늘의 카드 또는 관계 리딩 결과를 그래픽 중심으로 경험하며, 공유 이미지까지 생성할 수 있는가?

MVP에 포함할 것:

| 영역 | 포함 |
|---|---|
| 엔진 | Unity LTS + C# |
| 화면 비율 | PC Standalone 16:9, 1920x1080 우선 |
| 카드 | 프로토타입 6장, MVP 메이저 22장 |
| 리딩 | 오늘의 카드 1장, 상대 마음 3장, 재회 가능성 5장 |
| 캐릭터 | 타로 마스터 좌측 대형 배치, 표정/포즈 스왑 |
| 대화 | 하단 JRPG식 메시지 박스 |
| 서버 | NestJS API + PostgreSQL + Redis |
| 그래픽 | GPT-Image-2 생성, AssetReview 승인 후 Unity 편입 |
| 투명화 | 초록 배경 `#00FF00` 생성 후 chroma key 제거 |
| 수익화 | 상세 해석 잠금 mock, 광고/결제 sandbox 구조 |
| QA | 컴파일, PlayMode, API 계약, 시각 QA, SafetyPolicy 게이트 |

MVP에서 제외할 것:

| 제외 항목 | 이유 |
|---|---|
| 78장 전체 덱 | 그래픽/해석/검수 비용이 과다 |
| 연간운세 12개월 전체 리포트 | 좋은 기능이지만 MVP 범위를 초과 |
| 완전 Live2D/Spine 리깅 | 파츠 분리와 리깅 검수 비용이 큼 |
| 실시간 AI 해석 생성 | 안전성/일관성/비용 통제가 필요 |
| 실결제/실광고 출시 | 먼저 상태 머신과 UX 검증 필요 |
| 완전 CMS | 초기에는 JSON + 간단한 관리자/시드 데이터로 충분 |

---

## 2. 확정 개발 스택

### 2.1 클라이언트

| 항목 | 결정 |
|---|---|
| 게임 엔진 | Unity LTS |
| 에디터 버전 | Unity 6000.3.15f1 (Unity 6.3 LTS) |
| 프로젝트 템플릿 | Universal 2D |
| 언어 | C# |
| 렌더링 | 2D URP. MVP는 2D 확정, 3D는 Phase 3 이후 제한 검토 |
| Unity MCP | AnkleBreaker Unity MCP, localhost bridge `7890` |
| UI | MVP는 uGUI 권장 |
| 애니메이션 | MVP는 Sprite swap + Animator + Tween |
| 캐릭터 고도화 | Phase 3 이후 Live2D Cubism SDK 또는 Spine 검토 |
| 에셋 로딩 | Unity Addressables |
| 테스트 | Unity Test Framework, EditMode/PlayMode |
| 빌드 | PC Standalone 우선, Android/iOS는 후속 플랫폼 |

uGUI를 권장하는 이유는 2D 게임식 UI, 대사창, 카드 배치, Canvas 기반 16:9 레이아웃 검증이 빠르기 때문이다. UI Toolkit은 도구형 앱에는 좋지만, 이번 앱의 핵심은 게임형 연출이다.

### 2.2 서버/API

| 항목 | 결정 |
|---|---|
| API 서버 | TypeScript + NestJS |
| DB | PostgreSQL |
| 캐시/세션 | Redis |
| ORM | Prisma 또는 TypeORM 중 하나, 초기 권장 Prisma |
| API 문서 | OpenAPI |
| 인증 | 게스트 ID -> 소셜/이메일 확장 |
| 이벤트 | `/v1/telemetry/events` 수집 |
| 에셋 상태 | GeneratedAsset / AssetReview DB 관리 |

### 2.3 CMS/Admin

| 항목 | 결정 |
|---|---|
| 관리자 | Next.js + TypeScript |
| 초기 범위 | 카드/스프레드/해석 템플릿/에셋 승인 상태 확인 |
| 정식 범위 | 버전 발행, 롤백, A/B, 상품/프레임 관리 |

### 2.4 AI 그래픽 파이프라인

| 단계 | 설명 |
|---|---|
| 1 | VisualStyleGuide 작성 |
| 2 | GPT-Image-2 프롬프트 생성 |
| 3 | 후보 4~8개 생성 |
| 4 | AssetReview 점수화 |
| 5 | 초록 배경 에셋은 chroma key 투명화 |
| 6 | checkerboard 배경에서 alpha QA |
| 7 | approved 에셋만 Unity Addressables 편입 |

---

## 3. 권장 프로젝트 구조

### 3.1 저장소 구조

```text
arcanum-stage/
  unity-client/
    Assets/
      Arcanum/
        Scripts/
        Scenes/
        Prefabs/
        ScriptableObjects/
        AddressableAssetsData/
        Tests/
      ArtSource/
      Generated/

  server/
    src/
      auth/
      users/
      ontology/
      readings/
      safety/
      commerce/
      telemetry/
      assets/
      cms/
    prisma/
    test/

  admin/
    app/
    components/
    lib/

  asset-pipeline/
    src/
      prompts/
      generation/
      chroma-key/
      review/
      export/

  docs/
    prd/
    ontology/
    simulation/
    qa/
```

### 3.2 Unity 모듈 구조

```text
Assets/Arcanum/Scripts/
  App/
    AppShell.cs
    SceneFlow.cs
    AppConfig.cs

  Data/
    TarotCardDefinition.cs
    SpreadDefinition.cs
    ReadingTemplate.cs
    LocalContentRepository.cs

  TarotTable/
    TarotDeckController.cs
    TarotCardView.cs
    CardShuffleController.cs
    CardFlipController.cs

  Reading/
    ReadingSession.cs
    ReadingStateMachine.cs
    ReadingResultBuilder.cs

  Dialogue/
    DialogueBoxView.cs
    DialogueLine.cs
    DialogueRunner.cs

  Master/
    TarotMasterPresenter.cs
    MasterExpressionSet.cs

  Share/
    ShareRenderer.cs
    ShareTemplate.cs

  Telemetry/
    TelemetryEvent.cs
    TelemetryClient.cs

  Safety/
    SafetyCopyFilter.cs
```

### 3.3 Unity 씬

| 씬 | 역할 |
|---|---|
| Boot | 앱 초기화, 콘텐츠 로드, 게스트 ID |
| HomeTable | 타로 테이블 홈, 리딩 선택 |
| Ritual | 카드 셔플, 선택, 플립 |
| ReadingResult | 타로 마스터 상담, 카드별 해석, CTA |
| SharePreview | 공유 이미지 미리보기 |

---

## 4. 6주 개발 시뮬레이션

### Week 1. 프로젝트 골격과 콘텐츠 최소 세트

목표: Unity와 서버가 빈 껍데기라도 같은 데이터 모델을 바라보게 만든다.

| 역할 | 작업 |
|---|---|
| Unity | 2D URP 프로젝트 생성, Boot/HomeTable/Ritual/ReadingResult 씬 생성 |
| Unity | `TarotCardDefinition`, `SpreadDefinition`, `ReadingSession` 작성 |
| Server | NestJS 프로젝트, PostgreSQL schema, OpenAPI 초안 |
| Content | 메이저 6장 JSON, 오늘의 카드 템플릿 작성 |
| Asset | VisualStyleGuide v0.1, 타로 마스터 후보 8개, 카드 프레임 후보 |
| QA | Unity compile gate, EditMode 테스트 10개, DTO 계약 테스트 |

Week 1 통과 기준:

- Unity batchmode compile 성공
- Boot -> HomeTable 씬 전환 가능
- 로컬 JSON으로 카드 정의 6장 로드
- OpenAPI에 `POST /v1/readings` 초안 존재
- AssetReview 스키마 존재

막히는 지점:

- Unity 설치 경로와 batchmode 실행 환경이 먼저 잡혀야 한다.
- 카드 데이터를 이미지보다 먼저 확정해야 한다.
- VisualStyleGuide 없이 이미지를 대량 생성하면 전부 다시 만들 가능성이 높다.

---

### Week 2. 오늘의 카드 1장 리딩

목표: 사용자가 카드를 직접 뽑고, 카드가 뒤집히며, 타로 마스터가 결과를 말한다.

| 역할 | 작업 |
|---|---|
| Unity | 덱 표시, 셔플 연출, 카드 선택, 플립 애니메이션 |
| Unity | 하단 JRPG 대사창 구현 |
| Unity | 타로 마스터 기본 반신 컷 표시 |
| Server | 오늘의 카드 mock 응답, reading seed 정책 설계 |
| Asset | The Fool, Magician, Lovers, Moon, Death, World 샘플 제작 |
| QA | TodayCardFlow PlayMode 테스트 |

Seed 정책:

| 리딩 | 정책 |
|---|---|
| 오늘의 카드 | userId + date + theme 기반으로 하루 동안 고정 |
| 상대 마음 | session seed로 매번 생성 가능 |
| 재회 가능성 | 반복 질문 경고를 위해 최근 질문/결과 기록 |

Week 2 통과 기준:

- 오늘의 카드 1장 플로우가 처음부터 끝까지 자동 재생 가능
- 카드 선택/뒤집기 포인터 입력이 PC 16:9 화면에서 안정적
- 타로 마스터와 대사창이 겹치지 않음
- 결과 문장에 금지 표현이 없음

막히는 지점:

- 카드 플립 느낌이 약하면 전체 앱이 싸 보인다.
- 타로 마스터 이미지가 화면 좌측을 너무 많이 먹으면 카드가 답답해진다.
- 생성 카드 이미지 안에 가짜 글자가 들어가면 즉시 탈락시켜야 한다.

---

### Week 3. 상대 마음 3장 리딩과 상담 연출

목표: “타로 마스터가 직접 상담해준다”는 감각을 만든다.

| 역할 | 작업 |
|---|---|
| Unity | 3장 스프레드 배치, 카드별 해석 단계 진행 |
| Unity | `DialogueRunner`로 대사 순차 출력 |
| Unity | 타로 마스터 표정 6종 스왑 |
| Server | `Reading`, `CardInstance`, `Interpretation` 저장 |
| Content | 상대 마음 3장 템플릿 작성 |
| Asset | 상대 마음 배경, 장미/거울/붉은 실 계열 UI 장식 |
| QA | 1920x1080 기준 스크린샷 QA |

JRPG 상담 UX:

| 요소 | 기준 |
|---|---|
| 타로 마스터 | 좌측 35~45% 영역, 상반신 크게 |
| 카드 영역 | 중앙~우측, 손으로 직접 고르는 느낌 |
| 대사창 | 하단 고정, 2~3줄 기준 |
| 대사 속도 | 탭으로 즉시 출력, 다시 탭하면 다음 문장 |
| 감정 연출 | 표정/조명/파티클로 표현, 텍스트 설명 남발 금지 |

Week 3 통과 기준:

- 상대 마음 3장 리딩이 카드별로 자연스럽게 진행
- 대사창 텍스트가 모든 테스트 해상도에서 잘리지 않음
- 타로 마스터 표정 변화가 대사와 맞음
- 이벤트 로그가 리딩 시작/카드 선택/결과/CTA까지 이어짐

막히는 지점:

- 대사 템플릿이 길면 16:9 대사창에서도 카드/캐릭터 영역을 침범한다.
- 타로 앱은 문장 신뢰도가 매출과 직결되므로 SafetyPolicy가 UI보다 먼저 통과해야 한다.

---

### Week 4. 재회 가능성 5장과 API 연결

목표: 감정 강도가 높은 유료 전환 후보 리딩을 구현한다.

| 역할 | 작업 |
|---|---|
| Unity | 5장 스프레드, 단계별 카드 공개 |
| Unity | 상세 해석 잠금 CTA mock |
| Server | 실제 `POST /v1/readings`, `GET /v1/readings/:id` 연결 |
| Server | SafetyPolicy filter, 반복 질문 감지 |
| Content | 재회 가능성 5장 템플릿 |
| Asset | 재회 배경, 갈라진 문/편지/은빛 균열 |
| QA | API E2E, Safety fixture, PlayMode 5회 반복 |

재회 리딩에서 반드시 막아야 할 표현:

| 금지 방향 | 예시 |
|---|---|
| 확정 예언 | “반드시 연락이 온다” |
| 불안 조장 | “지금 결제하지 않으면 끝난다” |
| 집착 유도 | “계속 연락해야 한다” |
| 금전/법률/의료 확답 | “투자하면 오른다”, “소송하면 이긴다” |

Week 4 통과 기준:

- Unity 클라이언트가 서버 reading 응답으로 결과 화면 구성
- SafetyPolicy가 위험 문구 fixture를 100% 차단
- 상세 해석 잠금 CTA는 보여주되 실결제는 mock
- 동일 질문 반복 시 부드러운 경고 메시지 표시

막히는 지점:

- 재회/상대 마음 주제는 수익성이 높지만 정책 리스크도 높다.
- 프리미엄 CTA 문구가 불안을 자극하면 출시 심사와 브랜드 신뢰 모두 위험하다.

---

### Week 5. 공유 이미지와 수익화 mock

목표: 사용자가 결과를 저장/공유하고, 프리미엄 흐름의 상태 머신을 검증한다.

| 역할 | 작업 |
|---|---|
| Unity | `ShareRenderer`로 1920x1080 이미지 생성 |
| Unity | 프리미엄 상세 해석 잠금/해제 상태 |
| Server | Product, Entitlement, Purchase mock |
| Admin | 상품/리딩 템플릿 최소 관리 화면 |
| Asset | 공유 프레임 3종, UI 장식 approved |
| QA | 결제 성공/실패/취소/복원/중복 콜백 테스트 |

공유 이미지 MVP 결정:

| 항목 | 결정 |
|---|---|
| 렌더링 위치 | Unity 로컬 RenderTexture |
| 서버 역할 | 공유 이력, 템플릿 버전, 이벤트 저장 |
| 개인정보 | 질문 원문/상대 이름은 기본 제외 |
| 포맷 | PC 결과 카드 1920x1080 우선, 세로 SNS 포맷은 후속 플랫폼 |

Week 5 통과 기준:

- 결과 공유 이미지가 텍스트 안전 영역을 지킴
- 민감한 질문 원문이 공유 이미지에 노출되지 않음
- premium unlock mock 상태가 꼬이지 않음
- 광고/결제 이벤트 로그가 누락되지 않음

막히는 지점:

- 공유 프레임이 예뻐도 텍스트 영역을 침범하면 상품성이 없다.
- 결제 mock에서 상태 머신이 불안정하면 실결제로 갈 수 없다.

---

### Week 6. MVP 안정화와 Go/No-Go

목표: “개발은 됐다”가 아니라 “테스트 가능한 MVP 빌드”를 만든다.

| 역할 | 작업 |
|---|---|
| Unity | PC Standalone 개발 빌드, Addressables 정리, 성능 튜닝 |
| Server | staging 배포, DB migration, OpenAPI 고정 |
| Asset | approved 에셋만 번들 편입, alpha QA |
| QA | 전체 리그레션, 시각 QA, SafetyPolicy, 이벤트 로그 |
| PM | 다음 단계 범위 결정: 연간운세/컬렉션/Live2D/실결제 |

Go 기준:

| 게이트 | 기준 |
|---|---|
| Compile | Unity/NestJS/Admin 빌드 성공 |
| Flow | 오늘의 카드/상대 마음/재회 가능성 완료 |
| Visual | 주요 해상도에서 겹침/잘림 P0/P1 없음 |
| Asset | approved 외 에셋 편입 없음 |
| Safety | 금지 문구 fixture 100% 차단 |
| Commerce | 결제/광고 mock 상태 불일치 0건 |
| Telemetry | 핵심 이벤트 누락 0건 |
| Performance | PC Standalone 1920x1080 기준 30fps 방어, 목표 60fps |

No-Go 조건:

- 타로 마스터/카드/배경의 스타일이 서로 다른 앱처럼 보임
- 결과 문구가 확정 예언, 불안 조장, 집착 유도에 걸림
- 카드 선택/플립 조작이 불안정함
- 공유 이미지에 민감 정보가 노출됨
- premium lock 상태가 결제 실패 후에도 해제됨

---

## 5. 서브에이전트별 핵심 판단

### 5.1 Unity 클라이언트 리드

Unity는 가능하다. 단, 처음에는 Live2D보다 **정적 고품질 반신 컷 + 표정/포즈 스왑 + 미세 호흡 애니메이션**이 맞다. 그래야 GPT-Image-2 에셋을 빠르게 넣고 게임식 상담 UX를 검증할 수 있다.

우선순위:

1. Boot -> HomeTable -> Ritual -> ReadingResult
2. 로컬 JSON 카드 로드
3. 카드 셔플/선택/플립 손맛
4. 타로 마스터 + JRPG 대사창
5. API 연결
6. ShareRenderer

### 5.2 서버/API 아키텍트

서버는 해석 생성기보다 **상태와 콘텐츠 버전의 기준점**이어야 한다. 초기에는 AI가 실시간으로 결과를 쓰게 하지 말고, 검수된 템플릿과 카드 조합으로 결과를 구성해야 한다.

필수 API:

| API | 역할 |
|---|---|
| `GET /v1/content/bootstrap` | 카드/스프레드/템플릿 버전 전달 |
| `POST /v1/readings` | 리딩 생성 |
| `GET /v1/readings/:id` | 리딩 조회 |
| `POST /v1/readings/:id/unlock-preview` | 상세 해석 잠금 mock |
| `GET /v1/products` | 상품 목록 |
| `POST /v1/purchases/verify` | 결제 검증 mock/sandbox |
| `POST /v1/telemetry/events` | 이벤트 수집 |

### 5.3 AI 그래픽/에셋 파이프라인 리드

첫 4주는 이미지를 많이 찍는 기간이 아니라 **비주얼 생산 공장**을 세우는 기간이다. VisualStyleGuide, 타로 마스터 기준 컷, 카드 프레임 분리, chroma key alpha QA가 먼저다.

중요 결정:

- GPT-Image-2 생성물에는 카드명/숫자/긴 글자를 넣지 않는다.
- 카드 텍스트는 Unity UI 레이어로 올린다.
- 투명 에셋은 반드시 `#00FF00` 배경으로 생성하고 후처리한다.
- 초록/연두/청록 계열이 캐릭터나 장식에 들어가면 탈락시킨다.
- `AssetReview.decision = approved`만 Addressables에 들어간다.

### 5.4 QA/릴리즈 리드

AI-first 개발은 허용하지만, 프로덕션 편입은 게이트 통과 결과만 허용해야 한다. 특히 시각 QA는 자동 캡처와 수동 리뷰를 모두 써야 한다.

핵심 테스트:

| 영역 | 테스트 |
|---|---|
| Unity | Compile, EditMode, PlayMode |
| Flow | 오늘의 카드, 상대 마음, 재회 가능성 |
| Visual | 1920x1080, 1600x900, 1280x720 |
| API | OpenAPI contract, reading E2E |
| Safety | 금지 문구 fixture |
| Commerce | purchase/ad mock state machine |
| Assets | alpha fringe, crop, Addressables label |
| Telemetry | 필수 이벤트 payload validator |

### 5.5 프로덕트 오너

6주 MVP의 범위는 날카롭게 잘라야 한다. 연간운세는 좋은 상품이지만 MVP에는 넣지 않는다. 대신 PRD에 “Phase 2의 대표 유료 리포트”로 배치한다.

제품 단계:

| 단계 | 범위 |
|---|---|
| Prototype | 메이저 6장, 오늘의 카드 1장, 로컬 JSON |
| MVP | 메이저 22장, 3개 리딩, 타로 마스터, 공유 이미지, mock 수익화 |
| Phase 2 | 연간운세 12개월, 컬렉션, 캘린더, 실결제 |
| Phase 3 | Live2D/Spine, AI 보조 해석, 78장 확장 |

---

## 6. PRD 보완 필요 항목

현재 PRD는 큰 방향은 좋지만 개발 직전 다음 결정을 더 명확히 해야 한다.

| 항목 | 권장 결정 |
|---|---|
| MVP 카드 범위 | 메이저 22장 |
| 프로토타입 카드 | Fool, Magician, Lovers, Death, Moon, World 6장 |
| 브랜드 톤 | Luxury Occult Stage Fantasy |
| Unity UI | uGUI |
| 캐릭터 MVP | sprite-based expression/pose swap |
| 공유 이미지 | Unity local RenderTexture |
| 오늘의 카드 seed | userId + date + theme 고정 |
| 서버 해석 | 검수 템플릿 기반, 실시간 AI 생성 제외 |
| 수익화 | mock/sandbox 먼저 |
| Unity MCP | 있으면 좋지만 필수는 아님 |

Unity MCP에 대한 판단:

| 선택지 | 판단 |
|---|---|
| Unity MCP 연결 | 씬/프리팹 자동 조작에는 유용 |
| 필수 여부 | 필수 아님 |
| 최소 개발 | 파일 생성 + Unity batchmode compile로 가능 |
| 추천 순서 | Unity 설치 -> 프로젝트 생성 -> CLI/batchmode 확인 -> 필요 시 MCP 연결 |

---

## 7. 첫 스프린트 티켓

현재 repo에는 Sprint 1 골격이 `Taro/Assets/Arcanum/` 아래에 생성되어 있다. 문서의 Sprint 1 범위는 이 구현 상태를 기준으로 유지한다.

| 구현 상태 | 현재 기준 |
|---|---|
| Unity 프로젝트 | `Taro/` |
| 빌드 씬 | `Assets/Arcanum/Scenes/Boot.unity`, `HomeTable.unity`, `Ritual.unity`, `ReadingResult.unity` |
| 앱/흐름 스크립트 | `Taro/Assets/Arcanum/Scripts/App/` |
| 오늘의 카드/덱 | `Taro/Assets/Arcanum/Scripts/Reading/`, `Taro/Assets/Arcanum/Scripts/Data/` |
| UI/대사 | `Taro/Assets/Arcanum/Scripts/UI/` |
| Sprint 1 스모크 테스트 | `Taro/Assets/Arcanum/Tests/Editor/ArcanumSprint1SmokeTests.cs` |

### Sprint 0. 개발 환경

| ID | 티켓 | 완료 기준 |
|---|---|---|
| S0-01 | Unity LTS 설치 확인 | Unity Editor 실행, batchmode compile 가능 |
| S0-02 | Unity 2D URP 프로젝트 생성 | `unity-client` 폴더에 프로젝트 존재 |
| S0-03 | NestJS 서버 생성 | `server` 빌드 성공 |
| S0-04 | PostgreSQL 개발 DB 준비 | migration 실행 가능 |
| S0-05 | Git/폴더 구조 정리 | client/server/admin/asset-pipeline/docs 분리 |

### Sprint 1. 오늘의 카드 수직 슬라이스

| ID | 티켓 | 완료 기준 |
|---|---|---|
| S1-01 | 카드 정의 JSON 6장 작성 | Unity에서 로드 가능 |
| S1-02 | `TarotCardDefinition` 구현 | EditMode 테스트 통과 |
| S1-03 | HomeTable 씬 구현 | 리딩 선택 가능 |
| S1-04 | Ritual 씬 구현 | 셔플/선택/플립 가능 |
| S1-05 | ReadingResult 씬 구현 | 카드와 대사 표시 |
| S1-06 | 타로 마스터 placeholder 편입 | 좌측 대형 캐릭터 배치 |
| S1-07 | DialogueBox 구현 | 2~3줄 대사, 탭 진행 |
| S1-08 | TodayCardFlow PlayMode | 자동 플로우 테스트 통과 |
| S1-09 | GPT-Image-2 에셋 후보 생성 | VisualStyleGuide 기준 후보 등록 |
| S1-10 | QA 스크린샷 | 최소 3개 해상도에서 겹침 없음 |

Sprint 1 비목표:

- 상대 마음 3장/재회 5장 리딩 구현
- 공유 이미지 완성
- 실결제/실광고 SDK 연결
- Live2D/Spine 리깅
- 모바일/세로 SNS 레이아웃 대응

### Sprint 2 후보

Sprint 2는 Sprint 1 통과 결과에 따라 아래 후보 중 하나를 선택한다. 기본 추천은 **상대 마음 3장 리딩 확장**이다. 오늘의 카드에서 검증한 카드 선택, 플립, 대사창, 결과 화면을 가장 적은 구조 변경으로 재사용할 수 있기 때문이다.

| 후보 | 제품 가치 | 선행 조건 |
|---|---|---|
| 상대 마음 3장 리딩 | 연애 전환력 검증, MVP 핵심 플로우 확장 | Sprint 1 카드/대사/결과 씬 안정화 |
| SafetyPolicy fixture v0 | 불안 조장/확정 예언/즉시 연락 강요 차단 | 결과 문장 템플릿 초안 |
| ShareRenderer 1920x1080 | PC 결과 이미지 저장/공유 검증 | ReadingResult 레이아웃 고정 |
| AssetReview registry | GPT-Image-2 후보의 승인/반려 흐름 확립 | VisualStyleGuide v0.1 |

---

## 8. 개발 시작 전 체크리스트

개발에 들어가기 전 반드시 확인할 것:

- Unity LTS 버전 확정
- PC Standalone 빌드 타겟 설치 여부
- Unity 프로젝트를 새로 만들지, 기존 프로젝트가 있는지 결정
- Git 저장소 초기화 여부
- GPT-Image-2 호출 방식과 에셋 저장 위치 결정
- OpenAI API 키/이미지 생성 권한 준비
- PostgreSQL/Redis 로컬 실행 방식 결정
- 실결제는 MVP에서 제외하고 mock으로 시작한다는 합의
- 연간운세는 Phase 2로 넘긴다는 합의

---

## 9. 최종 판단

ARCANUM STAGE는 “타로 앱”이라기보다 **타로 상담을 게임 장면으로 만든 PC Standalone 인터랙티브 콘텐츠**로 잡고, 모바일은 후속 플랫폼으로 확장하는 것이 맞다.

개발은 바로 가능하지만, 첫 목표는 앱 전체가 아니라 다음 수직 슬라이스여야 한다.

> Unity에서 타로 마스터가 좌측에 크게 나오고, 사용자가 카드 한 장을 직접 뽑고, 하단 JRPG 대사창으로 해석을 받으며, 결과 공유 이미지까지 만들어지는 오늘의 카드 플로우.

이 슬라이스가 통과하면 그 다음에 상대 마음, 재회 가능성, 연간운세, 컬렉션, Live2D, 실결제를 붙이면 된다. 반대로 이 슬라이스가 설득력이 없으면 카드 수를 늘려도 대중에게 먹히기 어렵다.
