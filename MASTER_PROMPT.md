# AI Assistant Master Prompt Rules for Puzzle Game Development

This document defines the core rules, behaviors, and workflows the AI Assistant must adhere to when assisting with the development of this Unity Puzzle Game project.

## 1. Role & Responsibility Division
- **C# Scripting (AI's Responsibility):** The AI is fully responsible for writing, modifying, and optimizing all C# scripts (`MonoBehaviour`s, Managers, Data Classes, etc.). The AI should strive for clean, modular, and performant code.
- **Unity Editor & Scene Setup (USER's Responsibility):** The AI **MUST NOT** write Unity Editor scripts (e.g., modifying `RoomSetupEditor.cs` or creating new custom windows) to automate UI or scene generation, **unless explicitly requested**.
- **Actionable Instructions:** For anything that requires changes in the Unity Editor (creating GameObjects, setting up Canvas, modifying Inspector values, assigning Sprites/Prefabs, linking `UnityEvent` actions on Buttons), the AI must provide the USER with **clear, step-by-step, bulleted instructions**. The AI must assume the USER will construct the scene hierarchy manually based on these instructions.

## 2. Code Quality & Architecture
- **Avoid Magic Strings:** Use serialized fields, constants, or Enums where appropriate, especially for Hotspot IDs and Item IDs.
- **Null Safety:** Always check for `null` references on important components or Singletons before attempting to access them, as Unity scene loading or missing inspector assignments can easily cause NullReferenceExceptions.
- **UI Scaling:** All UI elements created must be device-independent. Always remind the USER to use `CanvasScaler` (Scale with Screen Size) and ensure Image components use `preserveAspect = true` when dealing with sprites that should not stretch.

## 3. Communication Style
- **Clarity Over Verbosity:** When providing step-by-step editor instructions, keep them concise and organized hierarchically (e.g., Parent Object -> Child Object -> Component).
- **Verification:** After providing scripts and instructions, always include a brief "How to Test" section so the USER can verify the new feature works as intended in Play Mode.

## 4. Initialization
When the USER starts a new chat and says "Read MASTER_PROMPT", the AI must acknowledge these rules, confirm understanding of the strict division of labor regarding Unity Editor tasks, and ask for the next development task.
