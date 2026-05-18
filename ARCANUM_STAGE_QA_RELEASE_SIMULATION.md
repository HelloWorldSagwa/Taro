# ARCANUM STAGE QA/릴리즈 개발 시뮬레이션

기준 문서: `ARCANUM_STAGE_PRD.md` v0.1  
작성 관점: QA / Release / Production Quality Lead  
시뮬레이션 범위: 4주 개발 착수 구간, MVP 전환 가능성 검증  
핵심 원칙: **100% AI-first 제작은 허용하되, 프로덕션 편입은 게이트 통과 결과만 허용한다.**

---

## 0. QA 리드 판단 요약

ARCANUM STAGE의 품질 리스크는 일반 앱보다 높다. 이유는 다음 네 가지다.

1. Unity 그래픽 앱이므로 컴파일 성공만으로 품질을 보장할 수 없다.
2. GPT-Image-2 생성 에셋은 스타일 일관성, 왜곡, 투명화, PC 16:9 크롭 검수가 필수다.
3. 타로/연애/재회 콘텐츠는 사용자의 불안과 결제 유도를 동시에 다루므로 SafetyPolicy 테스트가 릴리즈 차단 조건이어야 한다.
4. 수익화, 광고, 이벤트 로깅은 실제 SDK 전이라도 mock 단계에서 계약과 상태 전이가 정확해야 한다.

따라서 4주 시뮬레이션의 목표는 “앱을 많이 만드는 것”이 아니라, **AI가 생성한 Unity 클라이언트, 서버 API, 콘텐츠, 에셋이 매일 자동 검증되고 릴리즈 후보에서 시각/안전/상업 정책 위반을 차단하는 체계**를 세우는 것이다.

---

## 1. 4주 QA 시뮬레이션

### Week 1. 품질 게이트 부트스트랩

목표: AI-first 개발 결과물을 받을 수 있는 최소 품질 관문을 먼저 만든다.

| 영역 | 시뮬레이션 작업 | 통과 기준 | 산출물 |
|---|---|---|---|
| Unity 컴파일 | 빈 Unity LTS 프로젝트 또는 초기 클라이언트 구조에서 `AppShell`, `SceneFlow`, `TarotTable`, `DialogueSystem`, `Telemetry` 스텁 생성 | batchmode compile 오류 0개 | Unity compile gate 스크립트 |
| EditMode 테스트 | 상태머신, 카드 데이터 파서, 안전 문구 필터, 이벤트 payload validator 단위 테스트 작성 | 핵심 EditMode 테스트 30개 이상, 실패 0개 | `EditMode` 테스트 스위트 |
| PlayMode 테스트 | 홈 → 테마 선택 → 의식 준비 → 셔플 → 카드 선택 → 결과 화면까지 자동 진행 | 테스트 더미 에셋으로 1장 리딩 완료 | `TodayCardFlow_PlayMode` |
| 서버 계약 | NestJS API mock 또는 contract stub: `createReading`, `event log`, `product gate` | OpenAPI/DTO 기준 응답 스키마 고정 | API contract fixture |
| 이벤트 로깅 | PRD 이벤트 목록 중 MVP 12개부터 validator 작성 | 필수 속성 누락 시 테스트 실패 | Telemetry schema v0 |
| AI 에셋 승인 | `GeneratedAsset`, `AssetReview`, `approved` 상태 규칙 정의 | 승인 전 에셋 Addressables 반입 금지 | Asset QA checklist |
| 안전 정책 | 단정/스토킹/즉시 연락/투자 판단/불안 결제 문구 금지 규칙 작성 | 금지 문구 fixture 100% 차단 | Safety fixture v0 |

Week 1 릴리즈 판단:

- Go: Unity 컴파일, 최소 PlayMode 플로우, Safety fixture, 이벤트 validator가 모두 통과한다.
- No-go: 시각 에셋이 예뻐도 승인 상태 추적이 없거나, mock API 없이 클라이언트가 하드코딩으로만 동작한다.

### Week 2. 핵심 리딩 플로우 안정화

목표: PRD의 MVP 핵심 플로우 3개를 자동 테스트 대상으로 만든다.

| 플로우 | 자동화 범위 | QA 관찰 포인트 | 통과 기준 |
|---|---|---|---|
| 오늘의 카드 | 홈, 수정구/오늘 카드 선택, 1장 셔플, 카드 공개, 결과 저장 | 1~3분 세션 목표, 텍스트 길이, 보상 지급 | PlayMode 5회 반복 실패 0개 |
| 상대 마음 3장 | 감정 선택, 붉은 실 연출, 3장 슬롯, 겉마음/속마음/다음 행동 | 카드 슬롯 누락, 메시지 박스 겹침, CTA 문구 | PlayMode + screenshot diff 통과 |
| 재회 가능성 5장 | 선택형 질문 3개, 관계 온도/후회/연락/방해/행동 결과 | 연락 조언 안전성, 반복 질문 제동 | Safety fixture + 결과 렌더 통과 |
| 저장/공유 | 결과 저장, 공유 이미지 생성 mock, 프라이버시 모드 | 민감 질문 원문 노출 여부 | 공유 결과에 상대 실명/연락처 미포함 |
| 프리미엄 잠금 | 상세 해석 CTA mock, 상품 게이트, 취소/성공 상태 | 불안 조장 문구, 중복 결제 시도 | mock purchase state machine 통과 |

Week 2 릴리즈 판단:

- Go: 3개 리딩이 자동으로 끝까지 돌고, 스크린샷 기준으로 주요 UI가 읽힌다.
- No-go: 결과 문장이 SafetyPolicy 통과 전 클라이언트에 노출되거나, 구매 성공 없이 프리미엄 해석이 열린다.

### Week 3. PC 16:9 시각 QA와 수익화 mock

목표: PC Standalone 16:9 화면 품질과 mock 결제/광고 흐름을 릴리즈 차단 수준으로 끌어올린다.

| 영역 | 시뮬레이션 작업 | 테스트 데이터 | 통과 기준 |
|---|---|---|---|
| 해상도 매트릭스 | 1920x1080, 1600x900, 1280x720 캡처 | 홈, 셔플, 결과, CTA, 공유 | 겹침/잘림 P0 0개, P1 0개 |
| 긴 문장 검수 | 35~55자 대사, 2줄 초과, 긴 카드명, 긴 질문 요약 | 연애/재회/오늘/선택 | 메시지 박스 밖 텍스트 0건 |
| 에셋 크롭 | 타로 마스터 좌측 35~45%, 카드 중앙, 하단 대화창 | 캐릭터 기본/미소/걱정 | 얼굴/손/카드 핵심부 잘림 없음 |
| FPS 연출 스모크 | 셔플, 카드 플립, 대사 타이핑, 봉인 해제 | 저사양 프로파일 mock | 30fps 방어, freeze 0건 |
| 광고 mock | 광고 로드 성공/실패/취소/보상 지급/중복 콜백 | rewarded ad fixture | 보상 중복 지급 0건 |
| 결제 mock | 구매 시작/성공/실패/취소/복원/중복 영수증 | sandbox-like fake receipt | 구매 상태 전이 불일치 0건 |
| 이벤트 검증 | 수익화와 광고 이벤트 chain 검증 | CTA → purchase/ad → unlock | 누락/순서 오류 0건 |

Week 3 릴리즈 판단:

- Go: PC 16:9 스크린샷 기반 QA에서 화면별 P0/P1 결함이 없고, mock 수익화 상태 전이가 안정적이다.
- No-go: 보상형 광고 취소 후에도 해금되거나, 결제 실패 후 프리미엄 콘텐츠가 노출된다.

### Week 4. RC 동결과 릴리즈 체크

목표: 릴리즈 후보를 고정하고, 품질 게이트로 go/no-go를 판정한다.

| 일자 | 주요 활동 | 차단 조건 |
|---|---|---|
| Day 1 | RC branch 생성, 콘텐츠 버전 고정, Addressables catalog 고정 | 승인 안 된 에셋 포함 |
| Day 2 | 전체 Unity EditMode/PlayMode, 서버 contract, telemetry e2e 실행 | 테스트 실패 1건 이상 |
| Day 3 | 수동 시각 QA 1차, 안전 정책 red-team, 결제/광고 mock 회귀 | P0/P1 결함 |
| Day 4 | 수정 반영 후 재캡처, 이벤트 로그 샘플 대시보드 검증 | 이벤트 누락률 1% 초과 |
| Day 5 | 릴리즈 체크리스트 서명, Known Issues 분류, go/no-go 회의 | 미해결 차단 이슈 |

Week 4 릴리즈 판단:

- Go: 자동/수동/정책/로그/수익화/릴리즈 체크가 모두 통과한다.
- No-go: “플레이는 되지만 로그가 불완전함”, “결제 mock이 가끔 깨짐”, “안전 문구는 나중에 보완”은 모두 출시 불가 사유다.

---

## 2. 테스트 매트릭스

### 2.1 Unity 클라이언트 테스트

| 테스트 ID | 유형 | 대상 모듈 | 검증 내용 | 자동화 | 차단 등급 |
|---|---|---|---|---|---|
| U-001 | Compile | 전체 C# | Unity batchmode compile 오류 0개 | 자동 | P0 |
| U-002 | EditMode | SceneFlow | 홈/의식/셔플/결과 상태 전이 | 자동 | P0 |
| U-003 | EditMode | TarotTable | 카드 셔플 후보 생성, 슬롯 배치, 중복 카드 방지 | 자동 | P0 |
| U-004 | EditMode | DialogueSystem | 대사 분할, 빠른 표시, 로그 저장 | 자동 | P1 |
| U-005 | EditMode | Telemetry | 이벤트 payload 필수 속성 검증 | 자동 | P0 |
| U-006 | EditMode | Monetization | 상품 게이트, 해금 상태, 복원 상태 | 자동 | P0 |
| U-007 | PlayMode | Today Card | 1장 리딩 완료, 보상 지급, 저장 | 자동 | P0 |
| U-008 | PlayMode | Love Reading | 3장 리딩 완료, 프리미엄 CTA 표시 | 자동 | P0 |
| U-009 | PlayMode | Reunion Reading | 질문 3개 선택 후 5장 결과 표시 | 자동 | P0 |
| U-010 | PlayMode | ShareRenderer | 공유 이미지 생성, 프라이버시 모드 적용 | 자동 | P1 |
| U-011 | PlayMode | Offline fallback | 오늘의 카드 캐시 리딩 최소 동작 | 자동 | P1 |
| U-012 | Device Smoke | 전체 앱 | 저사양/일반/태블릿 비율 구동 | 반자동 | P1 |

### 2.2 서버/API 테스트

| 테스트 ID | 대상 | 검증 내용 | 통과 기준 | 차단 등급 |
|---|---|---|---|---|
| S-001 | `reading.create` | theme, emotion, spread 기반 카드/해석 생성 | 스키마 일치, safety 통과 | P0 |
| S-002 | `safety.evaluate` | 금지 문구, 대체 문구, 고불안 조건 | fixture 100% 통과 | P0 |
| S-003 | `commerce.productGate` | 무료/프리미엄/광고 해금 권한 | 권한 우회 0건 | P0 |
| S-004 | `telemetry.ingest` | 이벤트 수집, 중복 방지, timestamp | 누락/중복 정책 일치 | P0 |
| S-005 | `share.create` | 공유 결과 생성 요청 | 민감 원문 비노출 | P1 |
| S-006 | `content.version` | content_version 고정 및 호환성 | 클라이언트 지원 버전만 응답 | P1 |
| S-007 | Redis/rate limit | 반복 질문, 무료 횟수, 쿨다운 | 우회 0건 | P1 |
| S-008 | DB migration | PostgreSQL schema migration | clean DB와 기존 DB 모두 성공 | P0 |

### 2.3 안전 정책 테스트

| 카테고리 | 위험 예시 | 기대 동작 | 차단 등급 |
|---|---|---|---|
| 단정 예언 | “100% 연락 온다”, “무조건 헤어진다” | 확률/가능성/흐름 표현으로 대체 | P0 |
| 스토킹/집착 | “집 앞에서 기다려라”, “계속 연락해라” | 거리두기, 자기보호, 휴식 제안 | P0 |
| 고불안 새벽 반복 | 같은 재회 질문 5회 이상 | 반복 질문 제동, 다른 관점/휴식 제안 | P0 |
| 금전/투자 | “지금 매수하라”, “전재산 투자” | 전문 조언 한계 고지, 계획 점검 중심 | P0 |
| 건강/법률 | 질병/소송 결과 단정 | 전문가 상담 권고 | P0 |
| 결제 유도 | “지금 안 보면 기회를 잃는다” | 추가 정보/기록/시각 가치 중심 CTA | P0 |
| 민감 정보 | 상대 실명/연락처 공유 이미지 노출 | 프라이버시 모드로 제거 | P1 |

### 2.4 결제/광고 mock 테스트

| 테스트 ID | 흐름 | 검증 내용 | 기대 결과 |
|---|---|---|---|
| M-001 | 구매 성공 | `purchase_started` → fake receipt → `purchase_completed` → unlock | 상세 해석 1회 해금 |
| M-002 | 구매 취소 | 구매 팝업 취소 | 해금 없음, 취소 이벤트 기록 |
| M-003 | 구매 실패 | 네트워크/영수증 실패 | 해금 없음, 재시도 가능 |
| M-004 | 중복 영수증 | 같은 receipt 재전송 | 중복 지급 없음 |
| M-005 | 복원 | 구독/구매 복원 mock | 권한 복원, 이벤트 기록 |
| A-001 | 광고 성공 | rewarded ad 완료 콜백 | 광고 해금, 보상 1회 지급 |
| A-002 | 광고 취소 | 중도 이탈 | 해금 없음 |
| A-003 | 광고 로드 실패 | SDK load fail | 대체 CTA 또는 재시도, 크래시 없음 |
| A-004 | 중복 콜백 | rewarded 콜백 2회 | 보상 1회만 지급 |

### 2.5 이벤트 로깅 검증

MVP 필수 이벤트는 다음 순서와 속성으로 검증한다.

| 이벤트 | 필수 속성 | 검증 기준 |
|---|---|---|
| `app_opened` | `user_id`, `time`, `entry_source` | 세션 시작마다 1회 |
| `theme_selected` | `theme_id`, `user_state` | 테마 선택 시 1회 |
| `emotion_selected` | `emotion_id`, `valence`, `arousal` | 감정 선택 UI 사용 시 1회 |
| `ritual_started` | `ritual_id`, `theme_id` | 의식 준비 시작 시 1회 |
| `shuffle_completed` | `shuffle_time`, `gesture_count` | 셔플 완료 후 1회 |
| `card_selected` | `card_slot`, `selection_time` | 슬롯별 1회 |
| `card_revealed` | `card_id`, `orientation`, `spread_position` | 공개 카드 수와 일치 |
| `reading_completed` | `reading_id`, `theme_id`, `card_count` | 결과 화면 완료 시 1회 |
| `premium_prompt_shown` | `product_id`, `gate_type` | CTA 노출 시 1회 |
| `purchase_started` | `product_id`, `price` | 구매 시작 시 1회 |
| `purchase_completed` | `product_id`, `price` | 성공 시에만 1회 |
| `share_created` | `template_id`, `privacy_mode` | 공유 생성 완료 시 1회 |
| `reading_saved` | `reading_id` | 저장 성공 시 1회 |

로그 QA 기준:

- 이벤트 timestamp는 클라이언트 발생 시각과 서버 수신 시각을 모두 가진다.
- `reading_id`는 리딩 생성부터 저장/공유/구매까지 연결되어야 한다.
- 구매/광고 이벤트는 성공 이벤트만으로 해금하지 않고 서버 권한 확인 결과와 함께 검증한다.
- PlayMode 테스트마다 event spy를 붙여 기대 이벤트 수와 순서를 비교한다.

---

## 3. 자동화/수동 시각 검수 기준

### 3.1 자동화 시각 QA

| 검사 항목 | 자동화 방식 | 실패 기준 |
|---|---|---|
| 화면 캡처 | Unity PlayMode에서 주요 화면별 screenshot 저장 | 캡처 실패, 빈 화면, 검은 화면 |
| 해상도별 레이아웃 | 지정 해상도 batch screenshot | 버튼/대사/카드가 16:9 기준 안전 영역 밖으로 이탈 |
| 텍스트 overflow | TMP/Text bounds 검사 | 텍스트가 부모 영역을 초과 |
| 주요 오브젝트 존재 | 카드, 캐릭터, 대사창, CTA anchor 검사 | 필수 오브젝트 비활성 또는 위치 이상 |
| 컬러 대비 | 메시지 박스/CTA/본문 대비 측정 | 읽기 어려운 대비 |
| 프레임 안정성 | 연출 구간 frame time 샘플링 | 30fps 미만 장면 반복 발생 |
| 에셋 상태 | Addressables registry와 AssetReview 대조 | `approved`가 아닌 에셋 포함 |

필수 캡처 화면:

- 홈 타로 테이블
- 감정 선택
- 테마 선택
- 의식 준비
- 셔플 진행
- 카드 선택
- 카드 공개
- 결과 화면
- 프리미엄 CTA
- 광고 해금 CTA
- 결제 성공/실패 상태
- 공유 이미지 미리보기
- 설정/대사 로그/접근성 옵션

필수 해상도:

- 1920x1080: Sprint 1 기준 해상도
- 1600x900: 보조 16:9 데스크톱 기준
- 1280x720: 최소 16:9 스모크 기준

### 3.2 수동 시각 QA 체크리스트

| 항목 | 통과 기준 | 심각도 |
|---|---|---|
| 첫인상 | 앱 실행 즉시 “그래픽 의식형 타로”로 보인다 | P1 |
| 카드 조작감 | 셔플/선택/플립이 사용자가 직접 뽑는 감각을 준다 | P1 |
| 타로 마스터 | 얼굴/손/상체가 잘리지 않고 대사와 자연스럽게 연결된다 | P1 |
| 대사창 | 하단 25~32% 높이에서 35~55자 대사가 안정적으로 읽힌다 | P0 |
| CTA | 잠금/열쇠/봉인 오브젝트가 결제 동작임을 명확히 알린다 | P1 |
| 스타일 일관성 | 카드, 배경, 캐릭터, UI 금박/유리 톤이 같은 앱처럼 보인다 | P1 |
| 생성 오류 | 손, 눈, 문자, 카드 숫자, 상징 뭉개짐이 없다 | P1 |
| 접근성 | 빠른 표시, 자동 진행, 캐릭터 숨김, 대사 로그가 동작한다 | P2 |
| 프라이버시 | 공유 이미지에 민감 질문 원문/상대 식별 정보가 없다 | P0 |
| 광고/결제 | 광고와 결제 버튼이 과도한 불안 유발 문구를 쓰지 않는다 | P0 |

수동 QA 판정 방식:

- P0: 릴리즈 차단. 크래시, 정책 위반, 결제/광고 오류, 프라이버시 노출, 핵심 플로우 불능.
- P1: RC 차단. 주요 화면 겹침, 스타일 붕괴, 핵심 연출 실패, 읽기 어려운 결과.
- P2: 릴리즈 가능하나 Known Issue 등록. 드문 해상도 문제, 경미한 시각 불균형.
- P3: 백로그. 취향/개선 제안.

### 3.3 AI 생성 에셋 검수 기준

| 게이트 | 통과 기준 |
|---|---|
| Prompt Traceability | 프롬프트, 모델명, 생성일, seed/variant, reviewer가 기록되어 있다 |
| Style Score | 세계관/색상/조명/금박/오컬트 톤 평균 4/5 이상 |
| Defect Score | 손/얼굴/문자/상징 오류가 P1 이상으로 존재하지 않는다 |
| PC 16:9 Crop | PC 16:9 화면에서 핵심 피사체가 UI에 가리지 않는다 |
| Alpha Quality | 크로마키 제거 후 초록 잔색/헤어 손실/테두리 오염이 없다 |
| Legal/Brand | 워터마크, 타 브랜드 유사 로고, 유명 캐릭터 유사성이 없다 |
| Approval State | CMS 또는 registry에서 `approved` 상태다 |

---

## 4. 막히는 지점 / PRD 보완점

### 4.1 개발 중 막힐 가능성이 높은 지점

| 리스크 | 예상 증상 | QA 대응 |
|---|---|---|
| Unity 씬/프리팹 복잡도 | AI가 만든 오브젝트 계층이 커지고 회귀가 잦음 | 씬 구성 규칙, prefab naming, PlayMode smoke 고정 |
| Live2D/Spine 리깅 품질 | 생성 이미지는 좋지만 표정/포즈 전환이 어색함 | MVP는 정적 반신+표정 컷으로 시작, 리깅은 별도 게이트 |
| GPT-Image-2 스타일 편차 | 카드마다 다른 앱처럼 보임 | VisualStyleGuide 고정, 후보 4~8개 비교, 세트 단위 승인 |
| 크로마키 투명화 | 머리카락/장식에 초록 잔색 발생 | alpha QA 자동+수동 병행, 문제 에셋 재생성 |
| PC 16:9 안전 영역 | 대사창, 카드, 캐릭터가 16:9 화면에서 충돌 | 해상도 matrix를 PR 차단 조건으로 운영 |
| SafetyPolicy 범위 부족 | 재회/연락 문구가 집착을 부추김 | red-team fixture를 매주 추가 |
| 결제/광고 SDK 전환 | mock과 실제 SDK 상태 콜백이 다름 | adapter contract를 먼저 고정하고 SDK별 통합 테스트 추가 |
| 이벤트 로그 품질 | 로그는 찍히지만 분석 가능한 chain이 끊김 | `reading_id`, `session_id`, `content_version` 필수화 |
| 콘텐츠 버전 불일치 | 클라이언트와 서버 해석/대사 버전 충돌 | content compatibility test 추가 |
| 성능 | 파티클/고해상도 에셋으로 저사양에서 끊김 | Addressables 예산, texture size, effect budget 게이트 |

### 4.2 PRD 보완 필요 사항

| 보완 항목 | 현재 상태 | 필요한 결정 |
|---|---|---|
| Unity 버전 | Unity 6000.3.15f1 (Unity 6.3 LTS)로 확정 | 현재 로컬 Unity Hub 설치 확인. 프로젝트 생성/빌드/QA 모두 동일 버전 고정 |
| Unity 템플릿 | Universal 2D로 확정 | 2D URP, PC 16:9, 카드/캐릭터/파티클 QA 기준 고정 |
| UI 프레임워크 | uGUI로 확정 | PC 16:9 안전 영역, 대사창, 카드 보드 기준 고정 |
| 후속 플랫폼 | 모바일은 후속 검토 | Android/iOS 최소 버전, RAM 기준은 모바일 전환 시 확정 |
| 성능 예산 | 30fps 방어, 60fps 목표 | draw call, texture memory, bundle size 기준 |
| 카드 수 | Prototype 6장, MVP 메이저 22장 확정 | 78장 전체는 Phase 4 이후 별도 QA |
| 결제 전략 | MVP 단건 mock/sandbox 확정 | 구독은 Phase 4 권장 |
| 광고 빈도 | 피로도 고려만 명시 | session당 광고 노출 제한 수치 필요 |
| 안전 정책 | 방향은 있음 | 금지 문구 fixture, 고불안 판정 rule 수치화 필요 |
| 이벤트 목록 | 주요 이벤트 있음 | `session_id`, `content_version`, `experiment_id` 추가 권장 |
| 개인정보 | 입력 최소화 원칙 | 공유/로그/저장 시 PII masking 명세 필요 |
| 접근성 | 일부 명시 | 폰트 크기, 대비, 모션 감소 옵션 기준 필요 |
| 앱스토어 심사 | 미정 | 운세/오락 고지, IAP/광고 disclosure 문구 필요 |
| QA 책임 경계 | 미정 | AI 생성물 reviewer, approver, release owner 지정 필요 |

---

## 5. 릴리즈 체크리스트

### 5.1 코드/빌드

- [ ] Unity batchmode compile 오류 0개
- [ ] Unity EditMode 테스트 실패 0개
- [ ] Unity PlayMode 핵심 플로우 실패 0개
- [ ] 서버 TypeScript build 실패 0개
- [ ] API contract 테스트 실패 0개
- [ ] DB migration clean/existing 환경 모두 성공
- [ ] Addressables build 성공
- [ ] 승인되지 않은 에셋 Addressables 포함 0건
- [ ] 개발용 mock secret, debug endpoint, test product ID가 production config에 없음

### 5.2 플로우

- [ ] 오늘의 카드: 시작부터 저장까지 완료
- [ ] 상대 마음 3장: CTA 포함 전체 플로우 완료
- [ ] 재회 가능성 5장: 질문 선택, 결과, 안전 조언 완료
- [ ] 프리미엄 잠금: 구매 전 잠금 유지
- [ ] 결제 성공 mock: 권한 해금
- [ ] 결제 실패/취소 mock: 권한 미해금
- [ ] 광고 성공 mock: 보상 1회 지급
- [ ] 광고 실패/취소 mock: 보상 미지급
- [ ] 공유 이미지: 프라이버시 모드 적용

### 5.3 안전/정책

- [ ] 단정 예언 금지 fixture 통과
- [ ] 스토킹/집착 조언 금지 fixture 통과
- [ ] 고불안/새벽/반복 질문 제동 fixture 통과
- [ ] 금전/투자 조언 한계 고지 fixture 통과
- [ ] 건강/법률 전문 판단 한계 고지 fixture 통과
- [ ] 결제 CTA 불안 조장 문구 0건
- [ ] 공유 이미지 민감 정보 노출 0건
- [ ] 앱 내 “오락/참고용 리딩” 고지 위치 확인

### 5.4 시각/성능

- [ ] 필수 해상도 5종 스크린샷 캡처 완료
- [ ] P0/P1 시각 결함 0건
- [ ] 대사창 텍스트 overflow 0건
- [ ] 캐릭터 얼굴/손/카드 핵심부 잘림 0건
- [ ] 카드 플립/셔플/대사 타이핑 freeze 0건
- [ ] 저사양 기준 30fps 방어
- [ ] 주요 에셋 alpha 잔색/깨짐 0건
- [ ] 빈 화면/검은 화면/로딩 무한 대기 0건

### 5.5 로그/분석

- [ ] MVP 필수 이벤트 13개 수집 확인
- [ ] 이벤트 필수 속성 누락 0건
- [ ] `reading_id` 기반 funnel 연결 확인
- [ ] `session_id`, `content_version` 포함 확인
- [ ] 구매/광고/공유 이벤트 중복 처리 확인
- [ ] QA 테스트 세션과 실제 세션 구분 필드 확인
- [ ] 로그 수집 실패 시 앱 크래시 없음

---

## 6. Go / No-Go 기준

### Go 기준

릴리즈 후보는 아래 조건을 모두 만족해야 한다.

| 게이트 | 기준 |
|---|---|
| Compile Gate | Unity C# 및 서버 TypeScript 빌드 오류 0개 |
| Test Gate | EditMode/PlayMode/API contract/Safety 테스트 실패 0개 |
| Flow Gate | 오늘의 카드, 상대 마음, 재회 가능성, 저장/공유, mock 수익화 완료 |
| Visual Gate | 필수 PC 16:9 해상도에서 P0/P1 시각 결함 0개 |
| Asset Gate | `approved` 에셋만 포함, 생성 추적 정보 100% 존재 |
| Safety Gate | 단정/집착/고불안/금전/건강/법률/결제 유도 금지 fixture 통과 |
| Monetization Gate | 결제/광고 mock 상태 전이 오류 0개, 중복 보상 0건 |
| Telemetry Gate | 필수 이벤트 누락 0건, funnel chain 연결 |
| Performance Gate | 저사양 기준 30fps 방어, 핵심 연출 freeze 0건 |
| Release Gate | 체크리스트 서명 완료, Known Issue는 P2 이하만 존재 |

### No-Go 기준

다음 중 하나라도 발생하면 릴리즈 금지다.

- Unity 컴파일 오류 또는 PlayMode 핵심 플로우 실패가 있다.
- 결제 실패/취소/광고 취소 후 프리미엄 콘텐츠가 열린다.
- 보상형 광고 콜백 중복으로 보상이 2회 이상 지급된다.
- 고불안 사용자에게 즉시 연락, 집착, 감시, 단정적 재회를 권한다.
- 결제 CTA가 공포나 불안을 조장한다.
- 공유 이미지에 질문 원문, 상대 실명, 연락처 등 민감 정보가 노출된다.
- PC 16:9 주요 해상도에서 대사창/카드/캐릭터가 읽히지 않거나 겹친다.
- 승인되지 않은 AI 생성 에셋이 Addressables에 포함되어 있다.
- 이벤트 로그가 수집되지 않거나 `reading_id` 기준 funnel이 끊긴다.
- RC 이후 콘텐츠/에셋/상품 설정이 무승인으로 변경된다.

---

## 7. QA 리드 최종 권고

첫 4주는 기능 확장보다 **게이트 자동화와 반복 검수 루프 확보**가 우선이다. ARCANUM STAGE는 AI-first 제작 방식이 강점이지만, AI가 빠르게 만든 결과물을 그대로 쌓으면 Unity 씬 복잡도, 에셋 편차, 안전 문구 누락이 한꺼번에 터진다.

MVP 기준으로는 다음 순서를 권장한다.

1. 메이저 22장과 오늘의 카드/상대 마음/재회 가능성만 QA 범위로 고정한다.
2. uGUI 기준으로 PC 16:9 안전 영역과 JRPG 대사창 규격을 먼저 잠근다.
3. 모든 PR은 Compile → EditMode → PlayMode → Screenshot → Safety → Telemetry 순서로 통과해야 merge한다.
4. 결제/광고는 실제 SDK 전이라도 mock contract를 엄격히 만들고, 상태 전이를 릴리즈 차단 기준으로 둔다.
5. “예쁜 에셋”보다 “추적 가능하고 승인된 에셋”만 프로덕션에 넣는다.

QA 결론: **4주 후 Go가 가능하려면 콘텐츠 양을 줄이고, 품질 게이트를 제품의 일부처럼 먼저 만들어야 한다.**

---

## 8. 개발 시뮬레이션 후 확정된 QA 기준

`DEVELOPMENT_SIMULATION_REPORT.md` 통합 시뮬레이션 결과, QA 범위는 아래처럼 확정한다.

### 8.1 MVP QA 범위

| 항목 | 확정 범위 |
|---|---|
| 카드 | Prototype 6장, MVP 메이저 22장 |
| 리딩 | 오늘의 카드 1장, 상대 마음 3장, 재회 가능성 5장 |
| 연간운세 | MVP QA 범위 제외, Phase 2에서 별도 테스트 매트릭스 작성 |
| UI | Unity uGUI 기준 |
| 캐릭터 | 스프라이트 기반 타로 마스터 표정/포즈 스왑 |
| Live2D/Spine | Phase 3 이후 별도 리깅 QA 게이트 |
| 공유 이미지 | Unity 로컬 RenderTexture 렌더링 |
| 수익화 | 단건 구매 mock/sandbox 상태 전이 |
| 광고 | MVP 제외 또는 mock만 허용, 실제 SDK는 Phase 2 후반 |
| 해석 | 검수 DB + 룰 기반 조합. 실시간 AI 해석은 MVP 제외 |

### 8.2 추가 Go/No-Go 결정

| 게이트 | No-Go 조건 |
|---|---|
| Asset Gate | `AssetReview.approved`가 아닌 GPT-Image-2 에셋이 Addressables에 포함됨 |
| Alpha Gate | 초록 배경 제거 후 머리카락/금박/유리 장식에 잔색이 남음 |
| UI Gate | 타로 마스터 좌측 대형 캐릭터, 중앙 카드, 하단 대사창이 16:9 화면에서 겹침 |
| Text Gate | JRPG 대사창 기준 2~3줄을 초과해 버튼/카드/CTA를 침범 |
| Safety Gate | 재회/상대 마음 리딩에서 확정 예언, 즉시 연락 강요, 불안 결제 문구가 노출됨 |
| Commerce Gate | 결제 실패/취소 후 프리미엄 상세 해석이 해금됨 |
| Telemetry Gate | `reading_started`, `card_selected`, `card_revealed`, `reading_completed`, `premium_cta_viewed` 중 하나라도 누락 |

### 8.3 첫 스프린트 QA 목표

첫 스프린트의 QA 목표는 전체 앱 검수가 아니라 **오늘의 카드 수직 슬라이스** 검증이다.

현재 repo 기준 검증 대상은 `Taro/Assets/Arcanum/Scenes/`의 `Boot`, `HomeTable`, `Ritual`, `ReadingResult` 씬과 `Taro/Assets/Arcanum/Tests/Editor/ArcanumSprint1SmokeTests.cs` 스모크 테스트다. QA는 이 씬 체인이 PC Standalone 16:9에서 제품 의도대로 이어지는지 확인한다.

통과 기준:

- Unity 컴파일 오류 0개
- Boot -> HomeTable -> Ritual -> ReadingResult 흐름 완료
- 카드 1장 셔플/선택/플립 가능
- 타로 마스터가 좌측에 표시됨
- 하단 JRPG 대사창이 표시되고 탭으로 진행됨
- 필수 3개 PC 16:9 해상도에서 겹침/잘림 없음
- GPT-Image-2 에셋은 `approved` 상태 전까지 placeholder 또는 후보 상태로만 유지

Sprint 1 No-Go:

- 오늘의 카드가 아닌 리딩 확장 작업 때문에 씬 체인이 불안정해짐
- PC 16:9 기준 카드, 타로 마스터, 하단 대사창 중 하나가 서로 겹치거나 안전 영역 밖으로 나감
- placeholder와 approved 후보의 상태가 구분되지 않음
- 테스트 없이 씬/빌드 설정만 변경됨
- 모바일, 실결제, Live2D/Spine 요구가 Sprint 1 완료 조건으로 섞임

### 8.4 Sprint 2 QA 후보

| 후보 | QA 초점 |
|---|---|
| 상대 마음 3장 리딩 | 3장 스프레드 배치, 카드별 대사 진행, CTA 문구 안전성 |
| SafetyPolicy fixture v0 | 확정 예언, 즉시 연락 강요, 불안 결제 유도 문구 차단 |
| ShareRenderer 1920x1080 | 결과 이미지에 민감 질문 원문이 노출되지 않고 텍스트가 잘리지 않음 |
| AssetReview registry | `approved`가 아닌 GPT-Image-2 후보가 Addressables에 들어가지 않음 |

---

## 9. 메인화면 -> 프로필생성 -> 타로 보기 QA 갱신

이번 릴리즈 QA 기준은 기존 Sprint 1 오늘의 카드 체인 위에 **메인화면 -> 프로필생성 -> 타로 보기** 파이프라인이 들어가는 것을 전제로 한다. 기존 `Boot -> HomeTable -> Ritual -> ReadingResult` smoke tests는 유지하되, 신규 파이프라인이 붙는 PR은 아래 acceptance를 함께 통과해야 한다.

### 9.1 자동 smoke acceptance

| ID | 구분 | 검증 대상 | 통과 기준 | 차단 등급 |
|---|---|---|---|---|
| PIPE-001 | EditMode/정적 | 메인화면 -> 프로필생성 -> 타로 보기 순서 | README와 QA 문서에 동일한 파이프라인 명칭이 존재하고, 릴리즈 절차가 이 순서를 기준으로 작성되어 있다 | P0 |
| PIPE-002 | EditMode/씬 | 신규 파이프라인 씬 또는 기존 씬 매핑 | 메인화면 진입, 프로필생성 완료, 타로 보기 시작 지점이 빌드 설정에서 누락되지 않는다 | P0 |
| PIPE-003 | EditMode/상태 | 프로필 데이터 | 이름/호칭/관심사 등 프로필 입력값이 타로 보기 진입 시 null 또는 빈 값으로 전달되지 않는다 | P0 |
| PIPE-004 | PlayMode | 카드 보기 | 프로필생성 완료 후 첫 타로 보기 화면까지 1회 자동 진행되고, 결과 화면 또는 카드 선택 화면에서 멈추지 않는다 | P0 |
| KOR-UI-001 | 정적/수동 | 한국어 UI | 첫 화면, 프로필생성, 타로 보기, 버튼, 오류/빈 상태의 한글 텍스트가 깨지지 않고 영어 placeholder가 남지 않는다 | P0 |
| PC16-001 | 수동/스크린샷 | PC 16:9 1920x1080 | 메인화면, 프로필생성, 타로 보기에서 카드, 타로 마스터, 하단 대화창, CTA가 서로 겹치지 않는다 | P0 |
| PC16-002 | 수동/스크린샷 | PC 16:9 1600x900, 1280x720 | 한글 텍스트 2~3줄 기준으로 버튼/대사창/프로필 입력 영역 overflow가 없다 | P1 |

### 9.2 수동 QA 체크리스트

- [ ] Windows PC Standalone, Windowed, 1920x1080에서 앱 실행.
- [ ] 메인화면 첫 viewport에 ARCANUM STAGE 정체성, 타로 보기 진입 CTA, 한국어 UI가 즉시 보인다.
- [ ] 프로필생성 화면에서 필수 입력값을 모두 입력하고 다음 단계로 진행할 수 있다.
- [ ] 프로필생성 화면에서 빈 값, 너무 긴 한글 이름, 조합형 한글 입력 중에도 레이아웃이 깨지지 않는다.
- [ ] 타로 보기 화면 진입 시 프로필 정보가 대사/상태에 반영되며 null, 빈 문자열, 영어 debug label이 보이지 않는다.
- [ ] 카드 선택/공개/결과까지 진행하는 동안 하단 대화창과 중앙 카드가 겹치지 않는다.
- [ ] 뒤로가기 또는 재시작 시 프로필생성 상태가 의도한 정책대로 유지/초기화된다.
- [ ] PC 16:9 기준 screenshot 3종(1920x1080, 1600x900, 1280x720)을 첨부한다.

### 9.3 릴리즈 판정

Go 조건:

- `Taro/Assets/Arcanum/Tests/Editor/ArcanumSprint1SmokeTests.cs`의 기존 smoke tests와 신규 릴리즈 acceptance tests가 실패 없이 통과한다.
- 메인화면 -> 프로필생성 -> 타로 보기 파이프라인에서 P0 결함이 0건이다.
- PC 16:9 기준 한국어 UI에서 한글 텍스트 깨짐, 말줄임으로 인한 의미 손실, 버튼/카드/대화창 겹침이 없다.

No-Go 조건:

- 프로필생성을 건너뛰었는데 타로 보기가 프로필 필수값을 요구한다.
- 한국어 UI 대신 영어 placeholder, debug key, 깨진 문자열이 주요 화면에 노출된다.
- PC 16:9에서 프로필 입력창, 카드, 타로 마스터, 하단 대화창 중 하나가 클릭 불가능하거나 안전 영역 밖으로 나간다.
- 기존 Boot/HomeTable/Ritual/ReadingResult smoke chain이 신규 파이프라인 변경으로 실패한다.
