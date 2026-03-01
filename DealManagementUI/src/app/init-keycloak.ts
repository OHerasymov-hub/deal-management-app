import { KeycloakService } from 'keycloak-angular'

export function initializeKeycloak(keycloak: KeycloakService) {
  return () =>
    keycloak.init({
      config: {
        url: 'http://localhost:8080', // Keycloak url Docker
        realm: 'dealmanagementapp',   // Realm
        clientId: 'deal-management-app' // Client ID
      },
      initOptions: {
        onLoad: 'check-sso', 
        silentCheckSsoRedirectUri: window.location.origin + '/assets/silent-check-sso.html'
      },
      enableBearerInterceptor: true, // Add the Authorization header to outgoing HTTP requests
      bearerExcludedUrls: ['/assets']
    });
}
