# AssetReview v0.1

Use this checklist before any generated image moves from `Incoming/` to `Approved/`.

## Review States

| State | Meaning |
|---|---|
| `candidate` | Raw GPT-Image-2 output, not processed. |
| `needs_alpha` | Looks usable but requires chroma-key or alpha repair. |
| `needs_regen` | Concept or anatomy/composition issue requires regeneration. |
| `approved_style` | Family direction approved, not yet runtime-ready. |
| `approved_runtime` | Cropped, alpha-checked, named, and ready for Unity import. |

## Pass Criteria

- PC 16:9 screenshot fit: no face, hand, card symbol, or frame focal point is clipped.
- Style match: same lighting, ornament density, palette, and finish as VisualStyleGuide v0.1.
- Generated-art hygiene: no fake text, watermarks, malformed hands, broken eyes, unusable borders, or warped tarot symbols.
- Runtime readability: card face remains identifiable at in-game card size.
- Alpha quality: no green fringe, missing hair strands, smoky edge holes, or semi-transparent border damage.

## Naming

Use stable IDs before final art exists:

- `MASTER_LUNA_BASE_POSE_HALF_BODY_EXPR_CALM`
- `MASTER_LUNA_BASE_POSE_HALF_BODY_EXPR_SMILE`
- `CARD_MAJOR_00_FOOL_FACE`
- `CARD_MAJOR_00_FOOL_PROMPT`
- `CARD_BACK_DEFAULT`
- `BG_RITUAL_DAILY_TABLE_16X9`
- `FRAME_TAROT_DEFAULT`
- `FRAME_SHARE_16X9_GOLD`

## Green-Screen Alpha Notes

- Use pure `#00FF00` only for the removable background, not as rim light or gem color.
- Avoid green glass, emerald jewelry, green candles, and green particles on transparent-needed assets.
- After keying, inspect on black, white, violet, and checkerboard backgrounds.
- Hair, lace, gold filigree, smoke, and translucent sleeves need manual edge-spill review.
