

```md
# Changelog

All notable changes to this project will be documented in this file.

## [Unreleased]

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
