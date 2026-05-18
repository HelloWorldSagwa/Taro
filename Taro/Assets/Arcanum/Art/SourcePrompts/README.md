# SourcePrompts

Prompt drafts, negative prompt notes, seed notes, and family direction records live here. No generated bitmap belongs in this folder.

## GPT-Image-2 Prompt Rules

- Use the asset IDs from `../GenerationBacklog.md` as the first line of every prompt note.
- Record pipeline tags: `main_screen`, `profile_create`, `tarot_reading`, `korean_ui`, `green_screen_alpha`, `opaque_background`.
- For transparent-needed assets, include: `solid chroma key green background #00FF00, no shadows on background, no green object parts`.
- For Korean UI frames, include: `blank text-safe Korean JRPG UI frame, no text, no letters, no glyphs`.
- Keep Unity-owned text out of generated pixels: `루나`, `시작하기`, `프로필 생성`, `오늘의 카드`, card names, and card numbers are runtime text only.
- Save candidate notes with prompt, negative prompt, candidate count, selected direction, and rejection tags from `../AssetReview.md`.

## Standard Negative Prompt

```text
text, letters, Hangul, roman numerals, numbers, watermark, signature, logo, fake glyphs, pseudo-writing, unreadable UI labels, green object parts, green rim light, emerald jewelry, green particles, cast shadow on green background, cropped face, cropped hands, malformed hands, broken eyes, noisy text area
```
