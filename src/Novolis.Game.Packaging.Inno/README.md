# Novolis.Game.Packaging.Inno

Per-user Inno Setup script generation for Novolis games (same contract as `Novolis.Avalonia.Packaging.Inno`).

- `PrivilegesRequired=lowest` → `%LocalAppData%\Programs\…` (no admin)
- Publisher `Novolis`, URL `https://github.com/Novolis-Platform`
- Optional MIT `LicenseFile` and `SetupIconFile` / `icon.ico`

Avalonia desktop apps should use `Novolis.Avalonia.Packaging.Inno` instead.
