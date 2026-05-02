# AuthSystem Implementation Steps

## Current Phase (Completed)
- Multi-project clean architecture scaffold
- Auth API endpoints: login, refresh, logout
- Signed JWT generation and JWT bearer validation
- Permission policies (`users.manage`, `roles.manage`) and authorization handler
- Refresh token lifecycle with revoke/rotate behavior
- Auditing and login tracking via persistence-backed in-memory stores

## Next Phase (Planned in code design)
- Replace in-memory stores with EF Core implementations:
  - `IRefreshTokenStore` -> `EfRefreshTokenStore`
  - `IAuditService` -> `EfAuditStore`
  - `ILoginTrackingService` -> `EfLoginTrackingStore`
- Replace placeholder `IdentityDbContext` with ASP.NET Identity-backed context
- Hash and verify passwords through Identity managers
- Add migrations and schema bootstrap
- Add email confirmation and password reset flow
- Add role management and user administration endpoints

## Contracts intentionally stable for upgrade
- `IAuthService`
- `ITokenService`
- `IRefreshTokenStore`
- `IPermissionReadService`
- `IAuditService`
- `ILoginTrackingService`
- `INotificationService`
