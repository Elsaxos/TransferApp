

```md
# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

## [2026-03-13]
### Added
- Production configuration template (`appsettings.Production.json`).
- Security headers and reverse-proxy forwarded headers.
- Expanded unit tests for admin, transfer, localization, and validation.
- Coverage collection for integration tests.

### Changed
- Admin dashboard uses ViewModels consistently.
- Contact form email target is now driven by configuration.
- Admin auth models consolidated into a single options model.

### Removed
- Duplicate admin model classes and unused admin hash artifacts.

## [2026-01-18]
### Added
- Admin navigation shown only inside `/Admin` area.
- Separate admin lists for **Inquiries** and **Reservations**.
- Delete flow for inquiries/reservations in admin panel.
- Cookie auth improvements and Logout support.

### Changed
- Transfer flow updated to support required Email field.
- UI layout adjustments for Contacts page to behave better on smaller screens.
- Route-based pricing logic (price loaded automatically from predefined routes).

### Fixed
- Routing issues causing 404 for Pages (Prices/About/Contacts) after Program.cs changes.
- Admin links inconsistencies leading to 404.
