Project: Escape-The-Killer-s-House

Current structure:
- Unity 2D
- Existing flow uses RoomViewManager, Hotspot, and ZoomUIManager
- RoomViewManager handles main/right/left room views
- Hotspot opens zoom view or locked message
- ZoomUIManager handles zoom panel open/close

Design direction:
- Visual point-and-click puzzle game
- Focus on room views, hotspots, zoomed puzzles, and state changes
- Keep puzzle flow simple and medium difficulty
- Do not rely on inventory unless explicitly requested

Constraints:
- Keep existing hierarchy intact
- Add only what is necessary
- Prefer wiring existing systems over building new ones
- Keep compile/runtime stable