import http from 'k6/http';
import { sleep } from 'k6';

var n = 0;

export default function () {
  n = n + 1;
  http.post('http://localhost:5028/Notification/Send?message=Message'+n);
  sleep(1);
}