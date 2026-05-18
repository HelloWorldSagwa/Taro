# Visual Style Guide v0.1

## North Star

ARCANUM STAGE should read as a polished JRPG tarot stage: glass-dark UI, warm candle gold, rose accents, readable card silhouettes, and a calm but theatrical tarot master. The MVP target is PC 16:9 at 1920x1080.

The latest GPT-Image-2 screen direction is treated as the target look: a premium game screen rather than a utility app, with the tarot table already playable on first view, a left-side character presence, and Korean UI frames that feel native to a JRPG dialogue system.

## Composition

- Tarot master: left 35-45% of screen, half-body crop, face and hands never clipped, transparent background after processing.
- Cards: center/right play area, strong silhouette at small sizes, no embedded text, numbers, or fake glyphs in generated art.
- Backgrounds: full 16:9 stage plates with clear low-contrast lower band for dialogue UI.
- Frames: thin ornamental borders, transparent center, text-safe interior, no hard-to-key green reflections.
- Main screen: table and deck are the first-viewport signal. Logo and menu chrome must not dominate the card table.
- Profile creation: form panel sits right/center, Luna remains left-side guide, and the background supports a calm atelier/consultation feeling without becoming a character portrait scene.
- Tarot reading: selected card lane remains center-right, dialogue UI occupies bottom 18-24%, and reveal effects never cover Korean dialogue text.

## Palette

- Base: near-black violet, ink purple, candle gold, muted rose.
- Accent: cool moon silver for result highlights, restrained crimson for relationship readings.
- Avoid: neon rainbow, heavy beige parchment, muddy brown, sci-fi cyan dominance, and unreadable high-frequency texture behind text.

## GPT-Image-2 Prompt Constraints

- Generate card art without readable text, card names, roman numerals, signatures, watermarks, or UI labels.
- Generate transparent-needed master/frame assets on flat chroma green `#00FF00` only when direct alpha is unavailable.
- Keep recurring details consistent: Luna's hair, eyes, jewelry, sleeve shape, and silhouette must survive expression swaps.
- Request 4-8 candidates for each new family, then approve the family style before expanding the set.
- For Korean UI assets, request blank ornamental frames only. Korean labels such as `시작하기`, `프로필 생성`, `오늘의 카드`, `다음`, and `루나` are Unity text layers.
- Do not ask GPT-Image-2 to draw Hangul. Use prompts such as "blank text-safe Korean JRPG dialogue frame, no letters, no glyphs" rather than embedding actual UI copy.
- For green-screen outputs, include "solid chroma key green background #00FF00, no cast shadow on background, no green rim light, no green gemstones, no green particles."
- For opaque 16:9 screen backgrounds, reserve clean UI bands and avoid bright focal objects behind expected text zones.

## MVP Safe Areas

- Dialogue box occupies bottom 18-24% of 16:9.
- Master face target: upper-left third, never behind dialogue or card slots.
- Card focal symbol target: center 60% of card face.
- Share frame must preserve a central result text block and card title block.

## Screen Material Language

| Surface | Style rule | Alpha mode |
|---|---|---|
| Main navigation frame | Thin glass-dark strip, gold dividers, muted rose hover caps, low ornament density | green-screen to alpha |
| Primary button | Candle-gold bevel, dark glass fill, strong selected rim, wide blank center for Korean labels | green-screen to alpha |
| Secondary button | Smoky violet glass, silver-gold hairline, quiet hover glow | green-screen to alpha |
| Dialogue box | Bottom JRPG frame, transparent smoky center, gold inner trim, rose corner brackets, separate nameplate socket | green-screen to alpha |
| Nameplate | Compact gold/glass plate sized for 2-6 Hangul syllables, no baked text | green-screen to alpha |
| Profile form panel | 9-slice glass panel, subtle zodiac etching outside text fields, flat readable center | green-screen to alpha |
| Card frame | Thin ornamental tarot border, transparent center, no text, no numerals | green-screen to alpha |
| Background plate | Painterly 2D stage backplate, UI-safe lower band, no text | opaque |

## Luna Continuity

- Base identity: calm young adult tarot master, moon-silver hair with soft violet shadows, clear eyes, crescent/gold jewelry, black-violet sleeves, rose-gold tarot accents.
- Main screen Luna is welcoming and slightly angled toward the table.
- Profile creation Luna is attentive, hand near a quill/crystal/name card, expression reassuring.
- Tarot reading Luna is focused, one hand guiding the card lane, expression variants mapped to calm, smile, concern, focus, surprise, and reassurance.
- Expression swaps must preserve face structure, hair mass, jewelry placement, sleeve silhouette, and left-side half-body crop.
