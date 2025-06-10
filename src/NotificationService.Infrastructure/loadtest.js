import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  vus: 10,           // number of virtual users (concurrent requests)
  duration: '30s',   // test duration
};

const url = 'http://localhost:5037/api/notifications/';

const payload = JSON.stringify({
  type: 'email',
  email: {
    to: 'ahmed.eh01@gmail.com',
    subject: 'notification test',
    body: 'notification test for the emil service',
  },
});

const params = {
  headers: {
    'Content-Type': 'application/json',
  },
};

export default function () {
  const res = http.post(url, payload, params);

  check(res, {
    'is status 200': (r) => r.status === 200,
  });

  sleep(1); // pause for 1 second between iterations (adjust as needed)
}
