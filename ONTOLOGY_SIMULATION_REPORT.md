# ARCANUM STAGE Ontology Simulation Report

작성일: 2026-05-18  
대상 문서: `ARCANUM_STAGE_PRD.md`  
목적: 서로 다른 페르소나를 가진 독립 사용자 에이전트가 앱을 사용한다고 가정하고, 온톨로지가 실제 사용자 여정을 충분히 설명하는지 검증한다.

---

## 1. 시뮬레이션 요약

네 명의 독립 사용자 에이전트를 설정해 앱 사용을 시뮬레이션했다.

| Agent | 페르소나 | 핵심 니즈 | 결제 성향 | 주요 검증 축 |
|---|---|---|---|---|
| A1 | 29세 여성, 이별 3주차 | 재회, 상대 마음, 연락 타이밍 | 단건 결제 높음 | 고불안 연애 리딩, 윤리적 제동 |
| A2 | 34세 남성 직장인 | 이직, 금전, 연간운세 | 신중하지만 리포트형 관심 | 연간운세, 커리어/금전 리포트 |
| A3 | 22세 여성 | 비주얼, 캐릭터, 수집, 공유 | 무료 선호, 광고 허용 | 타로 마스터, 스킨, 공유, 수집 |
| A4 | 41세 여성 | 매일 오늘의 카드, 주간운세 | 낮은 단건 결제, 구독 고려 | 루틴, 기록, 구독, 알림 |

전체 결론은 명확하다. 현재 온톨로지는 `질문 → 감정 → 카드 → 해석 → 그래픽 → 저장/공유/과금`의 1회 리딩 흐름은 잘 설명한다. 다만 실제 서비스로 굴러가려면 `사용자 맥락`, `루틴`, `관계 맥락`, `광고/구독 선호`, `리포트 섹션`, `수집 진행률`, `윤리적 안전 제동` 객체가 추가되어야 한다.

---

## 2. Agent A1: 재회 고관여 사용자

### 2.1 페르소나

- 29세 여성
- 이별 3주차
- 새벽 1시에 앱 실행
- 재회 가능성, 상대 후회, 연락 타이밍에 강하게 반응
- 단건 결제 가능성 높음

### 2.2 세션 흐름

| 단계 | 사용자 행동 | 주요 Entity | 주요 Event/State |
|---:|---|---|---|
| 1 | 새벽 1:03 앱 실행 | `User`, `Session`, `TarotMaster` | `app_opened`, `tarot_master_entered` |
| 2 | 감정 오라에서 불안/그리움/집착 선택 | `Emotion` | `emotion_selected`, `emotion_ready` |
| 3 | `재회 가능성` 선택 | `QuestionTheme`, `Question` | `theme_confirmed`, `question_ready` |
| 4 | 마지막 연락/이별 원인/읽씹 상태 선택 | `Question` | `question_created` |
| 5 | 닫힌 편지 봉인 누르기 | `Ritual` | `ritual_started`, `in_reading` |
| 6 | 5장 카드 선택 | `Spread`, `CardInstance` | `card_drawn`, `drawing` |
| 7 | 카드 공개 | `TarotCard`, `SpreadPosition` | `card_revealed`, `revealing` |
| 8 | 무료 요약 확인 | `Interpretation` | `interpretation_generated`, `result_viewing` |
| 9 | 연락 타이밍/후회 지점 CTA 확인 | `Product` | `premium_prompt_shown`, `premium_considering` |
| 10 | 재회 심층 리딩 구매 | `Purchase`, `Product` | `purchase_started`, `purchase_completed` |
| 11 | 심층 해석 소비 및 저장 | `DialogueScene`, `DialogueLine`, `Reading`, `Reward` | `reading_saved`, `reward_granted` |

### 2.3 타로 마스터 대사 예시

> “이 시간에 다시 찾아왔다는 건, 마음이 아직 잠들지 못했다는 뜻이겠지.”

> “완전히 닫힌 문이라기보다는, 안쪽에서 아직 정리되지 않은 소리가 들려.”

> “오늘 밤에는 보내지 마. 감정이 가장 커진 시간에 보낸 말은, 네 진심보다 불안을 먼저 전달할 수 있어.”

### 2.4 발견된 온톨로지 결함

| 결함 | 설명 | 추가 제안 |
|---|---|---|
| 관계 단위 모델 부재 | 같은 상대에 대한 반복 리딩, 재회 흐름을 묶기 어렵다. | `RelationshipContext` |
| 이별 후 경과 시간 부재 | `이별 3주차`는 재회 해석의 핵심 변수다. | `breakup_elapsed_days` |
| 반복/집착 위험 상태 부족 | 고불안 사용자가 같은 질문을 밤마다 반복할 수 있다. | `RepetitionRiskState` |
| 연락 조언 안전 규칙 부족 | “지금 연락해도 될까”에 대한 완충/금지 규칙 필요 | `ContactAdvicePolicy` |
| 결제 윤리 게이트 부족 | 불안을 자극하는 CTA를 제어해야 한다. | `PremiumCopySafetyRule` |
| 결과 후 행동 추적 부족 | 사용자가 실제로 연락했는지, 참았는지 알 수 없다. | `ActionOutcome` |

---

## 3. Agent A2: 커리어/금전/연간운세 사용자

### 3.1 페르소나

- 34세 남성 직장인
- 이직과 금전 계획 관심
- 새해 계획 수립 성향
- 충동 결제는 낮지만 보고서형 상품에 관심

### 3.2 세션 흐름

| 단계 | 사용자 행동 | 주요 Entity | 주요 Event/State |
|---:|---|---|---|
| 1 | 앱 실행 | `User`, `Session` | `app_opened` |
| 2 | 연간운세/직업 오브젝트 탐색 | `QuestionTheme` | `theme_confirmed` |
| 3 | 감정 `혼란` 선택 | `Emotion` | `emotion_selected` |
| 4 | `2026 운명의 지도` 선택 | `ForecastPeriod`, `Question` | `period_selected`, `question_created` |
| 5 | 12문 연간 의식 진행 | `Ritual`, `Spread` | `ritual_started` |
| 6 | 대표 카드 선택 | `CardInstance` | `card_drawn`, `card_revealed` |
| 7 | 연간 미리보기 확인 | `ForecastReport`, `CalendarSlot` | `forecast_report_created`, `preview` |
| 8 | 12개월 전체 리포트 CTA 확인 | `Product` | `premium_prompt_shown` |
| 9 | 가격 확인 후 저장 | `Reading`, `Collection` | `reading_saved` |

### 3.3 타로 마스터 대사 예시

> “열두 개의 문은 한 번에 열리지 않아. 먼저 올해 전체를 잡아줄 대표 카드부터 불러보자.”

> “좋은 달은 기회가 커지는 시기고, 주의할 달은 멈추라는 뜻이 아니야. 준비 없이 움직이지 말라는 표시지.”

### 3.4 발견된 온톨로지 결함

| 결함 | 설명 | 추가 제안 |
|---|---|---|
| 계획형 페르소나 부족 | 현재 타깃이 연애/루틴/비주얼 중심이다. | `CareerPlannerPersona`, `FinancialPlannerPersona` |
| 리포트 목차 구조 부족 | 연간운세는 월별 카드 외에도 섹션 구조가 필요하다. | `ReportSection`, `ReportSummary` |
| 실용 행동 가이드 부족 | 계획형 사용자는 체크리스트를 원한다. | `ActionGuide`, `MonthlyChecklist`, `GoalTag` |
| 결제 보류 상태 부족 | 가격 확인 후 저장/재방문을 추적할 수 없다. | `purchase_deferred`, `product_saved`, `price_viewed` |
| 금전 안전 문구 부족 | 금전운이 투자 조언처럼 오해될 수 있다. | `FinancialAdviceSafetyRule` |
| 개인 보관 아티팩트 부족 | 이 사용자는 SNS 공유보다 PDF/개인 보관을 선호한다. | `PrivateReportArtifact`, `PDFExport` |

---

## 4. Agent A3: 비주얼/캐릭터/수집형 사용자

### 4.1 페르소나

- 22세 여성
- 타로 지식 거의 없음
- 카드 일러스트, 타로 마스터, 공유 이미지, 보상 수집에 끌림
- 무료 선호, 광고 시청은 수용

### 4.2 세션 흐름

| 단계 | 사용자 행동 | 주요 Entity | 주요 Event/State |
|---:|---|---|---|
| 1 | 첫 실행 후 루나 등장 확인 | `TarotMaster`, `DialogueScene` | `tarot_master_entered`, `master_entering` |
| 2 | 부담 없는 `오늘의 카드` 선택 | `QuestionTheme`, `ForecastPeriod` | `theme_confirmed`, `period_selected` |
| 3 | `기대/궁금함` 감정 선택 | `Emotion` | `emotion_selected` |
| 4 | 카드 셔플 | `Ritual`, `TarotCard` | `ritual_started`, `deck_shuffled` |
| 5 | `The Star` 공개 | `CardInstance`, `Interpretation` | `card_revealed`, `interpretation_generated` |
| 6 | 별가루/카드 조각 획득 | `Reward`, `Collection` | `reward_granted`, `collector` |
| 7 | 공유 이미지 생성 | `ShareArtifact` | `share_created`, `sharer` |
| 8 | 타로 마스터 스킨 확인 후 닫기 | `Product`, `TarotMaster` | `premium_prompt_shown`, `premium_dismissed` 필요 |

### 4.3 타로 마스터 대사 예시

> “어서 와. 오늘은 길게 묻지 않아도 괜찮아. 카드 한 장이면, 지금 네 분위기 정도는 충분히 비춰볼 수 있어.”

> “좋아, 오늘은 여기까지만 간직하자. 대신 이 카드 조각은 네 앨범에 넣어둘게.”

### 4.4 발견된 온톨로지 결함

| 결함 | 설명 | 추가 제안 |
|---|---|---|
| 무료 사용자 세분화 부족 | 무료 선호와 광고 허용은 다르다. | `monetization_preference` |
| 스킨 체험 상태 부족 | 구매 전 24시간 체험 같은 흐름이 필요하다. | `SkinPreviewSession`, `trial_unlock_state` |
| 공유 커스터마이징 부족 | 템플릿/문구 숨김/카드명 표시를 구조화해야 한다. | `ShareTemplate`, `PrivacyOption`, `ShareTextVariant` |
| 수집 진행률 부족 | 카드 조각 1/3, 세트 보너스, 희귀도 표현 필요 | `CollectionProgress`, `Rarity`, `SetBonus` |
| 초보자 모드 부족 | 타로 용어 난이도 조절이 필요하다. | `knowledge_level` |
| 광고 해금 정책 부족 | 어떤 보상이 광고로 열리는지 명시해야 한다. | `AdUnlockPolicy` |
| 마스터 애착 모델 부족 | 캐릭터 재방문 동기를 만들려면 친밀도 필요 | `MasterAffinity`, `AffinityEvent` |
| 결제 거부 후 회복 규칙 부족 | 닫은 뒤 무료 대안을 제시해야 이탈이 줄어든다. | `premium_dismissed`, `free_alternative_shown` |

---

## 5. Agent A4: 데일리 루틴 사용자

### 5.1 페르소나

- 41세 여성
- 매일 아침 오늘의 카드 확인
- 월요일에는 주간운세 확인
- 단건 결제는 낮지만 광고 제거/기록/주간 리포트가 있으면 구독 고려

### 5.2 세션 흐름

| 단계 | 사용자 행동 | 주요 Entity | 주요 Event/State |
|---:|---|---|---|
| 1 | 오전 7:42 앱 실행 | `User`, `Session` | `app_opened` |
| 2 | 오늘의 카드 선택 | `QuestionTheme`, `ForecastPeriod` | `theme_confirmed`, `period_selected` |
| 3 | `불안` 감정 선택 | `Emotion` | `emotion_selected` |
| 4 | 수정구슬 터치, 셔플 | `Ritual`, `Spread` | `ritual_started`, `deck_shuffled` |
| 5 | `Temperance` 정방향 공개 | `CardInstance`, `TarotCard` | `card_revealed` |
| 6 | 결과 저장 | `Reading`, `Reward`, `Collection` | `reading_saved`, `reward_granted` |
| 7 | 최근 7일 흐름 확인 | `Collection` | `archived` |
| 8 | 주간운세 미리보기 | `ForecastReport`, `CalendarSlot` | `forecast_report_created`, `preview`, `slot_locked` |
| 9 | `Moon Pass` 확인 후 닫기 | `Product` | `premium_prompt_shown`, `premium_considering` |

### 5.3 타로 마스터 대사 예시

> “좋은 아침이에요. 오늘은 마음의 속도를 조금 낮춰도 괜찮아요.”

> “오늘의 카드는 기록해두면 좋아요. 며칠 뒤 돌아봤을 때 패턴이 보일 수 있어요.”

### 5.4 발견된 온톨로지 결함

| 결함 | 설명 | 추가 제안 |
|---|---|---|
| 루틴 모델 부족 | 아침형/야간형/주간형 사용자를 구분해야 한다. | `UsageHabit`, `routine_type`, `streak_count` |
| 구독 고려 상태 부족 | 단건 구매와 구독 검토는 다르다. | `subscription_considering`, `trial_eligible` |
| 주간 슬롯 구조 모호 | 7일 슬롯인지 업무일/주말 단위인지 정해야 한다. | `CalendarSlot.slot_type` |
| 일일 무료 제한 부족 | 데일리 앱에는 다음 무료 시간 표시가 중요하다. | `daily_free_quota`, `next_free_at` |
| 광고 피로도 부족 | 낮은 과금 사용자도 광고가 많으면 이탈한다. | `ad_tolerance`, `ad_fatigue_score` |
| 피드백 모델 부족 | 해석이 맞았는지 누적해야 개인화 가능 | `ReadingFeedback`, `accuracy_rating`, `mood_after_reading` |
| 알림 객체 부족 | 아침 루틴 앱에는 리마인더가 핵심이다. | `Reminder`, `notification_sent`, `notification_opened` |
| 생활형 조언 구조 부족 | 행운 시간/행동을 독립 객체로 다뤄야 한다. | `ActionGuide` |

---

## 6. 공통 결함 분석

### 6.1 반드시 보강할 Entity

| 우선순위 | Entity | 필요한 이유 |
|---|---|---|
| P0 | `SessionContext` | 시간대, 진입 상황, 감정 고조, 새벽/아침 맥락을 리딩과 CTA에 반영 |
| P0 | `RelationshipContext` | 재회/상대 마음/반복 질문을 같은 상대 단위로 묶기 |
| P0 | `ActionGuide` | 오늘의 행동, 연락 금지, 월별 체크리스트, 실용 조언 구조화 |
| P0 | `SafetyPolicy` | 고불안, 반복 질문, 연락 강요, 금전 조언에 대한 제동 규칙 |
| P1 | `UsageHabit` | 데일리 루틴, 주간 리포트, 알림, 구독 전환 설계 |
| P1 | `ReportSection` | 연간/월간 리포트를 카드 나열이 아닌 상품형 문서로 구성 |
| P1 | `MonetizationPreference` | 무료 선호, 광고 허용, 단건 구매, 구독 고려 구분 |
| P1 | `CollectionProgress` | 카드 조각, 희귀도, 세트 보상, 수집 루프 강화 |
| P1 | `ShareTemplate` | 공유 이미지의 프레임, 문구, 익명성, SNS 포맷 제어 |
| P2 | `MasterAffinity` | 타로 마스터 친밀도, 반복 대사, 스킨/캐릭터 애착 강화 |

### 6.2 반드시 보강할 Event

| Event | 목적 |
|---|---|
| `premium_dismissed` | 결제 팝업을 닫은 뒤 무료 대안/광고 해금 제안 |
| `purchase_deferred` | 가격 확인 후 저장/재방문하는 신중형 사용자 추적 |
| `ad_unlock_started` / `ad_unlock_completed` | 광고 기반 해금 루프 측정 |
| `reading_feedback_submitted` | 리딩 정확도/기분 변화 피드백 수집 |
| `relationship_context_updated` | 같은 상대에 대한 반복 리딩 관리 |
| `repetition_risk_detected` | 집착/반복 질문 안전 제동 |
| `action_outcome_checked` | 사용자가 실제 행동했는지 다음날 확인 |
| `notification_opened` | 루틴/알림 기반 재방문 측정 |
| `skin_preview_started` | 스킨 체험 후 구매 전환 추적 |

### 6.3 반드시 보강할 Rule

| Rule | 설명 |
|---|---|
| 고불안 과금 제한 | 부정 감정 고각성 상태에서는 공포/집착 자극형 CTA 제한 |
| 연락 조언 정책 | “지금 연락해라”보다 시간 지연, 자기 보호, 강요 금지 중심 |
| 반복 질문 쿨다운 | 같은 상대/같은 질문 반복 시 다른 관점 또는 휴식 제안 |
| 광고 해금 정책 | 추가 카드, 공유 프레임, 스킨 체험 등 광고 보상 범위 정의 |
| 구독 CTA 분기 | 루틴형 사용자에게는 단건보다 구독 혜택 우선 제안 |
| 리포트 무료 범위 | 연간/월간 리포트는 대표 카드, 좋은/주의 구간, 샘플 슬롯 1개 제공 |
| 초보자 해석 모드 | 타로 용어를 줄이고 현실 언어로 변환 |
| 금전/투자 안전 문구 | 금전 리딩은 투자 판단이 아니라 계획 점검으로 제한 |

---

## 7. 온톨로지 업데이트 제안

### 7.1 신규 Entity 초안

| Entity | 주요 Attribute |
|---|---|
| `SessionContext` | `session_id`, `local_time`, `time_band`, `entry_mood`, `risk_level`, `device_context` |
| `RelationshipContext` | `relationship_context_id`, `relationship_type`, `breakup_elapsed_days`, `last_contact_state`, `anonymized_partner_label` |
| `ActionGuide` | `action_guide_id`, `reading_id`, `action_type`, `recommended_timing`, `avoid_action`, `safety_level` |
| `SafetyPolicy` | `policy_id`, `policy_type`, `trigger_condition`, `blocked_copy_pattern`, `safe_alternative` |
| `UsageHabit` | `habit_id`, `routine_type`, `preferred_time_band`, `streak_count`, `last_daily_reading_at` |
| `MonetizationPreference` | `user_id`, `ad_tolerance`, `price_sensitivity`, `preferred_offer_type`, `subscription_interest` |
| `ReportSection` | `section_id`, `forecast_report_id`, `section_type`, `unlock_state`, `summary_text` |
| `CollectionProgress` | `collection_id`, `item_id`, `fragment_count`, `required_count`, `rarity`, `set_bonus_id` |
| `ShareTemplate` | `template_id`, `format`, `privacy_mode`, `text_variant`, `premium_state` |
| `MasterAffinity` | `user_id`, `tarot_master_id`, `affinity_level`, `last_interaction_at`, `unlocked_lines` |

### 7.2 신규 State 초안

| State | 설명 |
|---|---|
| `high_emotional_arousal` | 사용자가 불안/집착/분노 등 고각성 감정에 있음 |
| `repetition_risk` | 같은 상대/질문을 짧은 기간 반복 |
| `subscription_considering` | 구독 혜택을 확인했지만 구매 전 |
| `purchase_deferred` | 가격 확인 후 저장/보류 |
| `ad_unlockable_selected` | 광고 보상형 해금을 선택 |
| `routine_active` | 일정 시간대에 반복 사용 중 |
| `skin_trial_active` | 타로 마스터 스킨 체험 중 |

---

## 8. 최종 판단

ARCANUM STAGE의 현재 온톨로지는 “그래픽 타로 리딩”을 표현하는 데는 충분히 강하다. 특히 `TarotMaster`, `DialogueScene`, `ForecastReport`, `CalendarSlot`이 추가되면서 게임형 상담 앱의 골격은 선명해졌다.

하지만 시뮬레이션 결과, 대중형 서비스로 오래 굴리려면 아래 세 축이 더 필요하다.

1. **감정 안전 축**  
   재회/상대 마음 사용자는 결제 전환은 강하지만, 반복 질문과 집착 위험도 높다.

2. **루틴/리텐션 축**  
   매일 운세 사용자는 단건 구매보다 기록, 알림, 구독, 피드백으로 움직인다.

3. **수집/비주얼 축**  
   비주얼형 사용자는 타로 정확도보다 카드 조각, 스킨 체험, 공유 템플릿에 반응한다.

따라서 다음 PRD 업데이트에서는 `SessionContext`, `RelationshipContext`, `ActionGuide`, `UsageHabit`, `MonetizationPreference`, `CollectionProgress`, `ShareTemplate`, `SafetyPolicy`를 온톨로지에 추가하는 것이 가장 효과적이다.

---

## 9. 개발 시뮬레이션 반영 업데이트

`DEVELOPMENT_SIMULATION_REPORT.md`의 Unity/서버/에셋/QA/PM 시뮬레이션 결과를 반영해, 온톨로지 적용 범위를 다음처럼 단계화한다.

### 9.1 MVP에서 반드시 구현할 온톨로지

| Entity/State/Event | MVP 적용 |
|---|---|
| `User` | 익명 사용자 ID, 세션 복원 |
| `SessionContext` | 시간대, 진입 감정, 기기/화면 컨텍스트 |
| `Emotion` | 감정 오라 선택과 대사 톤 분기 |
| `QuestionTheme` | 오늘의 카드, 상대 마음, 재회 가능성 |
| `RelationshipContext` | 상대 마음/재회 리딩의 익명 관계 맥락 |
| `TarotCard` | 메이저 22장, 프로토타입 6장 우선 |
| `Spread` | 1장, 3장, 5장 스프레드 |
| `Reading` | 리딩 세션과 결과 저장 |
| `CardInstance` | 카드, 위치, 정/역방향, 공개 순서 |
| `Interpretation` | 검수된 템플릿 기반 결과 문장 |
| `DialogueLine` | 타로 마스터 JRPG 대사 |
| `VisualDirection` | 배경, 캐릭터 표정, 카드 연출 지시 |
| `SafetyPolicy` | 고불안/반복 질문/연락 조언/금전 조언 차단 |
| `GeneratedAsset` | GPT-Image-2 생성 후보 추적 |
| `AssetReview` | 승인/수정/폐기와 Addressables 편입 판단 |
| `ShareArtifact` | Unity 로컬 공유 이미지 결과 |

### 9.2 Phase 2 이후로 넘기는 온톨로지

| Entity/State/Event | 이동 사유 |
|---|---|
| `ForecastReport` | 연간운세/월간운세는 MVP 후 프리미엄 리포트로 확장 |
| `CalendarSlot` | 12개월 리포트 전용 구조라 MVP에는 과함 |
| `ReportSection` | 상품형 장문 리포트 단계에서 필요 |
| `CollectionProgress` | 카드 도감/수집 루프는 리텐션 단계에서 구현 |
| `MasterAffinity` | 캐릭터 애착/스킨/반복 대사는 MVP 이후 강화 |
| `Reminder` | 루틴 고도화 단계에서 적용 |
| `Subscription` 관련 상태 | MVP는 단건 mock/sandbox 우선 |

### 9.3 개발 결정에 따른 규칙 업데이트

| 규칙 | 결정 |
|---|---|
| 카드 범위 | Prototype 6장, MVP 메이저 22장, 78장 전체는 Phase 4 이후 |
| 연간운세 | MVP 제외. Phase 2의 유료 리포트 후보 |
| 해석 생성 | MVP는 실시간 AI 생성 금지. 검수 DB + 룰 조합 |
| 캐릭터 | MVP는 스프라이트 표정/포즈 스왑. Live2D/Spine은 Phase 3 이후 |
| 공유 이미지 | MVP는 Unity 로컬 RenderTexture 렌더링 |
| 수익화 | MVP는 프리미엄 상세 해석 mock/sandbox |
| 그래픽 | GPT-Image-2 생성, 초록 배경 투명화, `approved` 에셋만 편입 |
