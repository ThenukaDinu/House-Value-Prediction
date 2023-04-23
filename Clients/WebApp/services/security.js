import Oidc from 'oidc-client'

var mgr = new Oidc.UserManager({
  authority: 'https://localhost:44342',
  client_id: 'VueWebApp',
  redirect_uri: 'http://localhost:44344/auth/callback',
  response_type: 'id_token token',
  scope: 'openid profile all read write update delete',
  post_logout_redirect_uri: 'http://localhost:44344/',
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage })
})

// dev env only must remove on the production
Oidc.Log.logger = console
Oidc.Log.level = Oidc.Log.INFO

export default mgr
