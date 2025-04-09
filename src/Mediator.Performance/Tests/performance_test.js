import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
  // Testing for sustained load with constant VUs over time.
  stages: [
    { duration: '1m', target: 100 },  // ramp-up to 100 users over 1 minute
    { duration: '3m', target: 100 },  // hold at 100 users for 3 minutes
    { duration: '1m', target: 0 },    // ramp-down to 0 users over 1 minute
  ],
};

export default function () {
    const url = 'http://localhost:8080/anothersimple'; //<---- change to your endpoint


  // Make an HTTP POST request to the API
  const res = http.get(url);

  // You can add basic checks here for response status and performance.
  check(res, {
    'status is 200': (r) => r.status === 200,    // Successful request
    'response time is less than 2000ms': (r) => r.timings.duration < 2000,  // Example threshold
  });
}
