'use strict';
import {
  clientId, clientSecret, deviceId, API_DOMAIN, AUTH_LEVEL,
} from '../env';
// import { fetchProfile } from '../ProfileController';

async function fetchAuth(data) {
  let url = API_DOMAIN + `/v1/login`;
  console.log(url);
  console.log(data);
  let response = await fetch(url, {
    method: 'POST',
    headers: {
      "Accept": "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data)
  }).catch(console.error);
  response = await response.json();
  __DEV__ && console.log('AUTH response:');
  __DEV__ && console.log(response);
  return response;
}

const { getItemAsync, setItemAsync, deleteItemAsync } = Expo.SecureStore;

async function setAccessToken(tokenData) {
  let { accessToken, refreshToken, expTime, authLevel } = tokenData;
  setItemAsync('accessToken', accessToken);
  setItemAsync('refreshToken', refreshToken);
  setItemAsync('authLevel', authLevel);
  setItemAsync('expTime', expTime);
}

export async function fetchTravoramaLoginApi(email, countryCallCd, phoneNumber, password) {
  let data = { clientId, clientSecret, deviceId, email, countryCallCd, phoneNumber, password };

  if (!data.email) delete data.email;
  if (!data.countryCallCd) delete data.countryCallCd;
  if (!data.phoneNumber) delete data.phoneNumber;

  let response = await fetchAuth(data);

  switch (response.status) {
    case 200:
      response.authLevel = AUTH_LEVEL.User;
      setAccessToken(response);
      // await fetchProfile();
      break;
    case 400:
    case 500:
    default:
      console.log('LOGIN API error ' + response.status);
      console.log('LOGIN API request data:');
      console.log(data);
  }
  return response;
}

async function waitFetchingAuth() {
  let sleep = ms => {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  while (global.isFetchingAuth) {
    await sleep(10);
    // console.log('still waiting for global.isFetchingAuth')
  }
  return;
}

export async function getAuthAccess() {
  if (global.isFetchingAuth) {
    await waitFetchingAuth();
    let { accessToken, authLevel } = global;
    return { accessToken, authLevel };
  }
  global.isFetchingAuth = true;
  try {
    let [accessToken, refreshToken, expTime, authLevel] =
      await Promise.all([
        getItemAsync('accessToken'), getItemAsync('refreshToken'),
        getItemAsync('expTime'), getItemAsync('authLevel')
      ]);
    let data = { clientId, clientSecret, deviceId };

    // console.warn('sengaja ditutup dulu validasi expTimenya buat testing refreshToken')
    /**/if (new Date(expTime) > new Date()) { //// token not expired
      console.log(
        'session not expired, continue the request...'
      )
      //already logged in, go to next step
      global.isFetchingAuth = false;
      global.accessToken = accessToken;
      global.authLevel = authLevel;
      return { accessToken, authLevel };
    } //// if it's not, then token is expired or client doesnt have expTime
    else/**/ if (refreshToken) {
      console.log('....     prepare login by refreshToken')
      data.refreshToken = refreshToken;
    } else {
      console.log('....     login as guest')
      authLevel = AUTH_LEVEL.Guest;
    }

    let response = await fetchAuth(data);
    global.isFetchingAuth = false;

    switch (response.status) {
      case 200:
        response.authLevel = authLevel;
        setAccessToken(response);
        break;
      case 400:
      case 500:
      default:
        if (authLevel != AUTH_LEVEL.Guest) {
          await removeAccessToken();
          return getAuthAccess();
        } else {
          console.log('get auth as guest error');
        }
    }
    global.isFetchingAuth = false;
    global.accessToken = accessToken;
    global.authLevel = authLevel;
    return { accessToken, authLevel };
  } catch (error) {
    console.log('get auth access error');
    console.log(error);
  }
}

export async function checkUserLoggedIn() {
  let { authLevel } = await getAuthAccess();
  return (authLevel >= AUTH_LEVEL.User);
}

export async function removeAccessToken() {
  await Promise.all([
    deleteItemAsync('accessToken'), deleteItemAsync('refreshToken'),
    deleteItemAsync('expTime'), deleteItemAsync('authLevel'),
  ]);
  return;
}

export async function logout() {
  return await Promise.all([
    deleteItemAsync('isLoggedIn'), removeAccessToken(),
  ]);
}
