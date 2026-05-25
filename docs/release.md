# Release

Packages publish to **GitHub Packages** on merge to `main` (`2026.1.*` versioning).

1. PR passes CI (`novolis-workflows` reusable build)
2. Merge to `main`
3. Release workflow packs all `IsPackable` projects under `src/`

Registry entries: [novolis-registry](https://github.com/Novolis-Platform/novolis-registry) `packages/novolis-game-*.json`.

See [novolis-governance release-policy](https://github.com/Novolis-Platform/novolis-governance/blob/main/docs/release-policy.md).
