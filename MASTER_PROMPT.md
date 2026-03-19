Read the repo before making changes.

Work within the current Unity 2D point-and-click architecture.
Keep changes minimal, safe, and compatible with existing scripts and hierarchy.

Rules:
If scene/UI setup is needed, prefer an Editor-only Scene Builder over manual Unity setup steps.
Do not delete existing objects; only add missing ones.
Builder must be safe to run multiple times.
- Reuse existing systems first
- Do not rewrite working systems unnecessarily
- Preserve current hierarchy and flow
- Avoid unnecessary new managers, scenes, or frameworks
- Use clean, inspector-friendly code
- Prevent null/missing reference issues

Gameplay focus:
room view -> hotspot click -> zoom view -> close/return

Preferred setup method: Editor-only Scene Builder instead of manual Unity setup.
Only add missing objects and keep existing hierarchy intact.
türkçe yanıtlar ver.