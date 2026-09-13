# Batch cleanup custom build

This local build adds two cleanup workflows without changing the existing backup workflows:

- A refreshed Fluent-style interface includes persistent light and dark themes, while retaining the Chinese and English layouts.
- Conversation cards show the recorded Codex model when it is available from the thread index or rollout metadata.
- Archived sessions can be filtered explicitly, and selected conversations can be previewed before deletion.

- In **Conversation only**, select conversations across different projects and use **Delete selected conversations** to process them in one batch. Related subagents are included. Project folders remain unchanged unless every related main conversation for that folder is selected and the folder passes the existing safety checks.
- In **Batch cleanup**, select multiple projects from the left pane and process all of their main conversations and related subagents. Each project folder can be kept, moved to the Windows Recycle Bin, or permanently deleted.
- The batch dialog has one-click **keep all**, **move all to Windows Recycle Bin**, and **permanently delete all** folder actions. Individual rows remain available only for exceptions. Permanent folder deletion uses one global confirmation phrase instead of requiring every folder name.

Deletion still requires Codex/ChatGPT Desktop to be fully closed and reuses the original application's confirmations, CLI checks, app trash, path validation, index cleanup, and sidebar repair behavior.

This is an unofficial local modification of v1.0.3. It has not been signed by the upstream author.

The refresh/repair flow also detects `DesktopOnly` ghost tasks: a local desktop catalog entry whose Thread ID is absent from the active core index and from every readable session/archive rollout. Confirmed ghosts can be removed in one batch together with their global-state and desktop-cache references. Detection fails closed if rollout storage cannot be scanned.
