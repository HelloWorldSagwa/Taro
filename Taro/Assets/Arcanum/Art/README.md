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

## Folder Contract

- `SourcePrompts/`: prompt drafts and generation notes only.
- `Incoming/`: raw GPT-Image-2 candidates before processing or approval.
- `ProcessedAlpha/`: chroma-keyed transparent outputs and edge-spill test versions.
- `Review/`: review contact sheets, notes, and rejected/needs-fix manifests.
- `Approved/`: only assets that pass VisualStyleGuide and AssetReview.
- `Placeholders/`: non-final labels, silhouettes, swatches, and taxonomies used by MVP scenes.

## Import Rule

Unity scenes may use placeholder labels and flat-color panels during Sprint 1. Approved GPT-Image-2 assets must keep their `AssetReview` state outside runtime code until Addressables integration is ready.
