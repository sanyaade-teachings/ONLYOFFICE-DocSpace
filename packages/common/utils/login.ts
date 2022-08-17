import api from "../api";

export async function login(user: string, hash: string, session = true) {
  try {
    const response = await api.user.login(user, hash, session);

    if (!response || (!response.token && !response.tfa))
      throw response.error.message;

    if (response.tfa && response.confirmUrl) {
      const url = response.confirmUrl.replace(window.location.origin, "");
      return url;
    }

    api.client.setWithCredentialsStatus(true);

    // this.reset();

    // this.init();
    // const defaultPage = window["AscDesktopEditor"] !== undefined || IS_PERSONAL ? combineUrl(proxyURL, "/products/files/") : "/"
    // return this.settingsStore.defaultPage;
    return Promise.resolve(response);
  } catch (e) {
    return Promise.reject(e);
  }
}
