// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  AuthEnabled: true,
  ApiBaseURL: 'http://localhost:52000/api/',
  AccountApiBaseUrl: '',
  StsURL: 'https://login.microsoftonline.com/76ae1115-1efc-4af2-a536-e2b2443af1a0',
  Resource: 'http://localhost:52000/api',
  RedirectURI: 'http%3A%2F%2Flocalhost%3A4200%2Fauth',
  ClientId: '6af55e3f-43de-4d5e-9fa6-ca6ebb96afcb'
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
