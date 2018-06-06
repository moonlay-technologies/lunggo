import { getAuthAccess } from './Auth/AuthController';
export { AUTH_LEVEL } from './env';

const LOGGING = true;


//// fetch API
export async function fetchTravoramaApi(request) {
  try {
    let { path, method, data, requiredAuthLevel } = request;
    method = method || 'GET';
    let url = API_DOMAIN + (path || request);
    LOGGING && console.log('fetching ' + method + ' from ' + url + ' ...')
    if (!requiredAuthLevel)
      throw 'ERROR fetch: requiredAuthLevel needed!';
    console.log('///prepare auth')
    //// Get auth info and check if user authorized for the request

    const coba = await getAuthAccess();
    console.log('coba')
    console.log(coba)

    let { accessToken, authLevel } = await getAuthAccess();
    //// check if client have sufficent authLevel for request
    authLevel = parseInt(authLevel);
    requiredAuthLevel = parseInt(requiredAuthLevel);
    if (authLevel < requiredAuthLevel) {
      console.warn(`401 Not Authorized: Not enough auth level!`)
      return {
        status: 401, message: 'Not Authorized: Not enough auth level!',
        requiredAuthLevel,
      }
    }
    console.log('///done auth, prepare fetch actual api')
    //// Execute request
    let response = await fetch(url, {
      method,
      headers: {
        "Authorization": 'Bearer ' + accessToken,
        "Accept": "application/json",
        "Content-Type": "application/json"
      },
      body: JSON.stringify(request.data),
    }).catch(console.error);
    console.log('///done fetch,, response handling')
    if (response == null || response == ' ') {
      console.log('response null, please check your connection!');
      return { message: 'response null, please check your connection!' }
    }
    if (response.status == 401) {
      //await deleteItemAsync('expTime');
      //return fetchTravoramaApi(request);
      throw `error 401 forbidden (not yet handled)`
    } else if (response.error == "ERRGEN98") { //invalid JSON format
      // console.log(JSON.stringify(request.data));
      throw 'invalid JSON format :' + JSON.stringify(request.data);
    }
    response = await response.json();
    LOGGING && console.log('response from ' + url + ' :');
    LOGGING && console.log(response);
    if (response.status != 200) {
      console.log('status is not 200! \nresponse:');
      console.log(response);
      console.log('request.data:');
      console.log(request.data);
    }
    return response;
  } catch (err) {
    console.log('error while fetching this request :');
    console.log(request);
    console.log(err);
  }
}
