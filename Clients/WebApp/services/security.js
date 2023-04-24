import Oidc from 'oidc-client'

var mgr = new Oidc.UserManager({
  authority: import.meta.env.VITE_BASE_URL_IDENTITY_SERVER_URL,
  client_id: 'VueWebApp',
  redirect_uri: 'http://localhost:44344/auth/callback',
  response_type: 'id_token token',
  scope: 'openid profile all read write update delete',
  post_logout_redirect_uri: 'http://localhost:44344/',
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage })
})

const displayISLogs = String(import.meta.env.VITE_SHOW_IDENTITY_LOGS).toLowerCase() === 'true'
// logs on dev env only must remove on the production
if (displayISLogs) {
  Oidc.Log.logger = console
  Oidc.Log.level = Oidc.Log.INFO
}

export default mgr
