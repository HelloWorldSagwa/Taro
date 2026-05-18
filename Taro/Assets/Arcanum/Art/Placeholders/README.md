# Placeholder Taxonomy

These placeholders are naming anchors only. They reserve runtime slots for later GPT-Image-2 output without pretending final art exists.

## Families

- `MASTER_*`: tarot master portrait, pose, and expression sprites.
- `CARD_MAJOR_*`: Major Arcana face art.
- `CARD_BACK_*`: face-down card backs.
- `BG_*`: 16:9 scene backplates.
- `FRAME_TAROT_*`: reusable card overlays.
- `FRAME_SHARE_*`: result/share overlays.

## Replacement Rule

When final art arrives, keep the semantic ID stable and replace only the asset reference. UI text, card data, and reading logic should not depend on generated image filenames.
