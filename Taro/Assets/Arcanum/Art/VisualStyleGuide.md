# Visual Style Guide v0.1

## North Star

ARCANUM STAGE should read as a polished JRPG tarot stage: glass-dark UI, warm candle gold, rose accents, readable card silhouettes, and a calm but theatrical tarot master. The MVP target is PC 16:9 at 1920x1080.

## Composition

- Tarot master: left 35-45% of screen, half-body crop, face and hands never clipped, transparent background after processing.
- Cards: center/right play area, strong silhouette at small sizes, no embedded text, numbers, or fake glyphs in generated art.
- Backgrounds: full 16:9 stage plates with clear low-contrast lower band for dialogue UI.
- Frames: thin ornamental borders, transparent center, text-safe interior, no hard-to-key green reflections.

## Palette

- Base: near-black violet, ink purple, candle gold, muted rose.
- Accent: cool moon silver for result highlights, restrained crimson for relationship readings.
- Avoid: neon rainbow, heavy beige parchment, muddy brown, sci-fi cyan dominance, and unreadable high-frequency texture behind text.

## GPT-Image-2 Prompt Constraints

- Generate card art without readable text, card names, roman numerals, signatures, watermarks, or UI labels.
- Generate transparent-needed master/frame assets on flat chroma green `#00FF00` only when direct alpha is unavailable.
- Keep recurring details consistent: Luna's hair, eyes, jewelry, sleeve shape, and silhouette must survive expression swaps.
- Request 4-8 candidates for each new family, then approve the family style before expanding the set.

## MVP Safe Areas

- Dialogue box occupies bottom 18-24% of 16:9.
- Master face target: upper-left third, never behind dialogue or card slots.
- Card focal symbol target: center 60% of card face.
- Share frame must preserve a central result text block and card title block.
