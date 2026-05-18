# ARCANUM STAGE Art Pipeline

This folder reserves the Unity-side landing zone for ARCANUM STAGE visuals before any GPT-Image-2 production generation begins. Do not place final generated art directly in runtime folders until it passes AssetReview.

## PC 16:9 MVP Art Slots

| Slot | Runtime purpose | Placeholder token | Final asset family |
|---|---|---|---|
| Tarot master | Left-column half-body guide, expression swaps | `MASTER_LUNA_BASE`, `EXPR_*`, `POSE_*` | transparent PNG sprites |
| Card faces | Major Arcana card illustration under Unity text layer | `CARD_MAJOR_00_FOOL` through `CARD_MAJOR_21_WORLD` | opaque card-face PNG |
| Card backs | Face-down selectable cards and shuffle state | `CARD_BACK_DEFAULT` | opaque card-back PNG |
| Card frame | Reusable tarot border, rarity/reading accents | `FRAME_TAROT_DEFAULT` | transparent PNG overlay |
| Scene backgrounds | Full-screen stage backplates per reading mode | `BG_HOME_TABLE`, `BG_RITUAL_*`, `BG_RESULT_*` | 16:9 opaque PNG |
| Share frame | Result-card export border and text-safe zones | `FRAME_SHARE_16X9_*` | transparent PNG overlay |

## Screen Pipeline Art Coverage

The first GPT-Image-2 game-screen direction is locked as: polished JRPG occult stage, dark glass UI, candle-gold bevels, muted rose accents, readable Korean UI frames, and a calm theatrical tarot master on the left. Required art coverage is grouped by player pipeline, not only by asset type.

| Pipeline | Runtime screen | Required art families |
|---|---|---|
| Main screen | `HomeTable` | `BG_MAIN_STAGE_TABLE_16X9`, `UI_LOGO_ARCANUM_STAGE_KO_FRAME`, `UI_FRAME_TOP_NAV_GLASS`, `UI_BUTTON_PRIMARY_GOLD_*`, `UI_BUTTON_SECONDARY_GLASS_*`, `MASTER_LUNA_HOME_*`, `FX_CANDLE_DUST_LOOP_*` |
| Profile creation | future `ProfileCreate` | `BG_PROFILE_ATELIER_16X9`, `UI_PANEL_PROFILE_FORM_9SLICE`, `UI_INPUT_NAMEPLATE_KO_9SLICE`, `UI_CHOICE_CHIP_GLASS_*`, `UI_AVATAR_FRAME_*`, `MASTER_LUNA_PROFILE_*` |
| Tarot reading | `Ritual` and `ReadingResult` | `BG_RITUAL_*_16X9`, `BG_RESULT_*_16X9`, `CARD_BACK_DEFAULT`, `FRAME_TAROT_DEFAULT`, `UI_DIALOGUE_BOX_JRPG_9SLICE`, `UI_NAMEPLATE_MASTER_KO_9SLICE`, `UI_CHOICE_BUTTON_READING_*`, `FX_CARD_REVEAL_*`, `MASTER_LUNA_READING_*` |

## Korean UI Frame Requirements

- Text is always rendered by Unity; generated UI art must not contain baked Korean, Latin labels, tarot names, numbers, signatures, or fake glyphs.
- Korean button and dialogue frames need wider text-safe centers than English UI. Reserve at least 72% of button width as low-texture interior and keep ornaments on the left/right caps.
- Dialogue boxes use a glass-dark 9-slice base, candle-gold 1-2 px inner trim, muted rose corner accents, and a separate nameplate socket for `루나`.
- Choice buttons use the same material language as the dialogue box but with stronger hover/selected rim-light states. Do not create pill-shaped decorative text blocks as art; runtime buttons own the label.
- Input/nameplate frames for profile creation must support Hangul composition, 2-8 Korean syllable display, and placeholder text without visual crowding.
- Frame corners must remain readable on 1920x1080, 1600x900, and 1280x720 captures.

## Folder Contract

- `SourcePrompts/`: prompt drafts and generation notes only.
- `Incoming/`: raw GPT-Image-2 candidates before processing or approval.
- `ProcessedAlpha/`: chroma-keyed transparent outputs and edge-spill test versions.
- `Review/`: review contact sheets, notes, and rejected/needs-fix manifests.
- `Approved/`: only assets that pass VisualStyleGuide and AssetReview.
- `Placeholders/`: non-final labels, silhouettes, swatches, and taxonomies used by MVP scenes.

## Import Rule

Unity scenes may use placeholder labels and flat-color panels during Sprint 1. Approved GPT-Image-2 assets must keep their `AssetReview` state outside runtime code until Addressables integration is ready.
