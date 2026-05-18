# GPT-Image-2 Generation Backlog

Do not generate these yet. This list defines the slots the MVP art pass must fill after VisualStyleGuide approval.

## Sprint 1 Prototype

| Priority | Asset ID | Count | Format | Notes |
|---|---|---:|---|---|
| P0 | `BG_MAIN_STAGE_TABLE_16X9` | 1 | opaque PNG | Main/HomeTable backplate based on latest game-screen direction. |
| P0 | `UI_DIALOGUE_BOX_JRPG_9SLICE` | 1 | transparent PNG | Blank Korean-safe bottom dialogue frame. |
| P0 | `UI_NAMEPLATE_MASTER_KO_9SLICE` | 1 | transparent PNG | Blank nameplate for `루나` Unity text. |
| P0 | `UI_BUTTON_PRIMARY_GOLD_IDLE` | 1 | transparent PNG | Main CTA button frame, no baked label. |
| P0 | `UI_BUTTON_PRIMARY_GOLD_HOVER` | 1 | transparent PNG | Hover/selected state. |
| P0 | `UI_BUTTON_SECONDARY_GLASS_IDLE` | 1 | transparent PNG | Secondary menu/choice state. |
| P0 | `MASTER_LUNA_BASE_POSE_HALF_BODY_EXPR_CALM` | 1 | transparent PNG | Left-column tarot master baseline. |
| P0 | `CARD_BACK_DEFAULT` | 1 | opaque PNG | Face-down selectable card. |
| P0 | `FRAME_TAROT_DEFAULT` | 1 | transparent PNG | Shared border overlay for prototype cards. |
| P0 | `BG_HOME_TABLE_16X9` | 1 | opaque PNG | Home table backplate. |
| P0 | `BG_RITUAL_DAILY_TABLE_16X9` | 1 | opaque PNG | Daily card ritual backplate. |
| P0 | `BG_RESULT_DAILY_TABLE_16X9` | 1 | opaque PNG | Reading result backplate. |
| P0 | `MASTER_LUNA_IDLE_CALM` | 1 | transparent PNG | Default animated idle sprite. Generate on #00FF00 and remove chroma key. |
| P0 | `MASTER_LUNA_IDLE_BLINK` | 1 | transparent PNG | Blink replacement frame, same silhouette as idle. Generate on #00FF00 and remove chroma key. |
| P0 | `MASTER_LUNA_SMILE` | 1 | transparent PNG | Welcome/smile expression sprite. Generate on #00FF00 and remove chroma key. |
| P0 | `MASTER_LUNA_FOCUSED` | 1 | transparent PNG | Listening/focused expression sprite. Generate on #00FF00 and remove chroma key. |
| P0 | `MASTER_LUNA_REVEAL_GESTURE` | 1 | transparent PNG | Card reveal gesture pose, gaze and hand toward table. Generate on #00FF00 and remove chroma key. |
| P0 | `MASTER_LUNA_REASSURANCE` | 1 | transparent PNG | Gentle reassurance pose for softer readings. Generate on #00FF00 and remove chroma key. |
| P0 | `MASTER_LUNA_SERIOUS` | 1 | transparent PNG | Result/certain expression sprite. Generate on #00FF00 and remove chroma key. |
| P0 | `FX_MASTER_AURA_IDLE` | 1 | transparent PNG | Subtle master aura behind portrait. Generate on #00FF00 and remove chroma key. |
| P0 | `FX_MASTER_AURA_REVEAL` | 1 | transparent PNG | Stronger reveal aura behind portrait. Generate on #00FF00 and remove chroma key. |
| P1 | `CARD_MAJOR_00_FOOL_FACE` | 1 | opaque PNG | Prototype card face. |
| P1 | `CARD_MAJOR_01_MAGICIAN_FACE` | 1 | opaque PNG | Prototype card face. |
| P1 | `CARD_MAJOR_06_LOVERS_FACE` | 1 | opaque PNG | Prototype card face. |
| P1 | `CARD_MAJOR_13_DEATH_FACE` | 1 | opaque PNG | Prototype card face, non-gory. |
| P1 | `CARD_MAJOR_18_MOON_FACE` | 1 | opaque PNG | Prototype card face. |
| P1 | `CARD_MAJOR_21_WORLD_FACE` | 1 | opaque PNG | Prototype card face. |

## MVP Expansion

| Family | Count | Notes |
|---|---:|---|
| `MASTER_LUNA_BASE_POSE_HALF_BODY_EXPR_*` | 6 | calm, smile, concern, focus, surprise, reassurance. |
| `MASTER_LUNA_HOME_POSE_WELCOME_EXPR_*` | 2 | calm, smile for main screen greeting. |
| `MASTER_LUNA_PROFILE_POSE_GUIDE_EXPR_*` | 3 | calm, reassurance, focus for profile creation. |
| `MASTER_LUNA_READING_POSE_GUIDE_EXPR_*` | 6 | reading-state expression set. |
| `CARD_MAJOR_02` through `CARD_MAJOR_20` missing faces | 16 | Complete Major Arcana 22-card set. |
| `BG_PROFILE_ATELIER_16X9` | 1 | Profile creation atelier/consultation backplate. |
| `BG_RITUAL_RELATIONSHIP_16X9` | 1 | Rose, mirror, red-thread theme. |
| `BG_RESULT_RELATIONSHIP_16X9` | 1 | Relationship reading result stage. |
| `BG_RITUAL_REUNION_16X9` | 1 | Door, letter, silver-crack theme. |
| `BG_RESULT_REUNION_16X9` | 1 | Reunion result stage. |
| `UI_LOGO_ARCANUM_STAGE_KO_FRAME` | 1 | Ornamental blank logo holder; final text is typography, not generated glyphs. |
| `UI_FRAME_TOP_NAV_GLASS` | 1 | Main top navigation frame, 9-slice capable. |
| `UI_PANEL_PROFILE_FORM_9SLICE` | 1 | Profile creation form panel. |
| `UI_INPUT_NAMEPLATE_KO_9SLICE` | 1 | Korean name input frame. |
| `UI_CHOICE_CHIP_GLASS_IDLE/HOVER/SELECTED` | 3 | Emotion/theme chips for profile and question selection. |
| `UI_AVATAR_FRAME_DEFAULT` | 1 | Profile avatar/reader identity frame, no baked icon or text. |
| `UI_CHOICE_BUTTON_READING_IDLE/HOVER/SELECTED` | 3 | Reading choice buttons. |
| `FX_CARD_REVEAL_GOLD_SMOKE_SPRITE` | 4 | Transparent reveal effect sprites or flipbook candidates. |
| `FX_CANDLE_DUST_LOOP_SPRITE` | 4 | Transparent ambient particles for main screen. |
| `FRAME_SHARE_16X9_GOLD` | 1 | PC result-card export frame. |
| `FRAME_SHARE_16X9_ROSE` | 1 | Relationship share variant. |
| `FRAME_SHARE_16X9_SILVER` | 1 | Reunion share variant. |

## Candidate Counts

Generate 4-8 candidates for the first asset in each family, approve the family direction, then batch the remaining IDs. Card faces should be reviewed as a set, not one by one in isolation.

## Pipeline Prompt Briefs

Use these as source briefs before generating. Do not paste Korean UI text into GPT-Image-2 prompts; keep labels in Unity.

### Main Screen

`BG_MAIN_STAGE_TABLE_16X9`

```text
Premium 2D JRPG occult tarot game main screen background, 16:9 1920x1080, playable tarot table as the central focus, dark violet glass stage, warm candle-gold light, muted rose accents, elegant card deck area on center-right, left side reserved for a half-body tarot master sprite, lower 22 percent kept low-contrast for dialogue UI, subtle moon and constellation motifs, polished game UI mood, no text, no logos, no letters, no watermark, no characters baked into background
```

`UI_BUTTON_PRIMARY_GOLD_IDLE`

```text
Blank Korean-safe JRPG fantasy UI button frame, candle-gold bevel with dark violet glass center, wide empty text-safe interior for Hangul label, small muted rose corner accents, premium occult tarot style, symmetrical horizontal button, solid chroma key green background #00FF00, no text, no letters, no glyphs, no shadows on background, no green object parts
```

### Profile Creation

`BG_PROFILE_ATELIER_16X9`

```text
Premium 2D JRPG tarot profile creation atelier background, 16:9 1920x1080, calm consultation desk with moonlit mirror, crystal, quill, sealed cards, dark glass and candle-gold palette, muted rose accents, right-center area kept clean for profile form panel, left area reserved for tarot master sprite, lower band low-contrast for dialogue frame, polished Korean game UI mood, no text, no letters, no logo, no watermark
```

`UI_PANEL_PROFILE_FORM_9SLICE`

```text
Blank 9-slice profile form panel frame for Korean game UI, translucent dark violet glass, thin candle-gold inner trim, subtle zodiac etching only near outer border, broad clean center for Hangul input fields, premium occult JRPG tarot style, solid chroma key green background #00FF00, no text, no letters, no fake symbols, no shadow on background, no green object parts
```

### Tarot Reading

`MASTER_LUNA_READING_POSE_GUIDE_EXPR_FOCUS`

```text
Half-body young adult tarot master Luna for JRPG tarot game, consistent moon-silver hair with soft violet shadows, clear calm eyes, crescent gold jewelry, black-violet sleeves, rose-gold tarot accents, focused expression, one hand gently guiding an unseen card lane to the right, theatrical but warm, polished 2D game character illustration, left-facing screen composition, face and hands fully visible, solid chroma key green background #00FF00, no text, no letters, no watermark, no green jewelry, no green particles, no cast shadow on background
```

`MASTER_LUNA_IDLE_CALM`

```text
Half-body young adult tarot master Luna for JRPG tarot game, consistent moon-silver hair with soft violet shadows, clear calm eyes, crescent gold jewelry, black-violet sleeves, rose-gold tarot accents, calm idle expression, hands relaxed near tarot deck height, theatrical but warm, polished 2D game character illustration, left-column screen composition, face and hands fully visible, solid chroma key green background #00FF00, no text, no letters, no watermark, no green jewelry, no green particles, no cast shadow on background
```

`MASTER_LUNA_IDLE_BLINK`

```text
Same character, costume, pose, silhouette, lighting, and framing as MASTER_LUNA_IDLE_CALM, but with eyes gently closed for a natural blink frame, polished 2D JRPG tarot master sprite, solid chroma key green background #00FF00, no text, no letters, no watermark, no green jewelry, no green particles, no cast shadow on background
```

`MASTER_LUNA_SMILE`

```text
Half-body young adult tarot master Luna for JRPG tarot game, same identity as idle sprite, gentle welcoming smile, one hand slightly open toward the viewer, moon-silver hair, black-violet robe, antique gold crescent jewelry, warm candle-gold rim light, polished 2D game character illustration, left-column screen composition, solid chroma key green background #00FF00, no text, no letters, no watermark, no green object parts
```

`MASTER_LUNA_REVEAL_GESTURE`

```text
Half-body young adult tarot master Luna for JRPG tarot game, same identity and costume, focused reveal gesture, one hand extended toward an unseen tarot card lane to the right, eyes following the card, subtle dramatic confidence, moon-silver and candle-gold rim light, polished 2D game character illustration, left-column screen composition, solid chroma key green background #00FF00, no text, no letters, no watermark, no green object parts
```

`UI_DIALOGUE_BOX_JRPG_9SLICE`

```text
Blank bottom JRPG dialogue box frame for Korean tarot game UI, wide horizontal 9-slice frame, translucent dark violet glass center, candle-gold 1-2 pixel inner trim, muted rose corner brackets, separate small socket area for speaker nameplate, generous empty text-safe interior for Hangul dialogue, premium occult stage style, solid chroma key green background #00FF00, no text, no letters, no glyphs, no shadows on background, no green object parts
```

`FX_CARD_REVEAL_GOLD_SMOKE_SPRITE`

```text
Transparent-style card reveal effect sprite for 2D tarot game, candle-gold smoke arc, tiny moon-silver spark particles, muted rose magical dust, centered burst with soft feathered edges, no card, no text, no symbols, solid chroma key green background #00FF00, no green particles, no cast shadow on background
```
