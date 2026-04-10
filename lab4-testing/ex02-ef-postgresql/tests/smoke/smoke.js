import http from 'k6/http';
import { sleep } from 'k6';
import { group } from 'k6';
import * as config from './config.js';

export const options = {
    vus: 100,
    duration: '30s',

    thresholds: {
        http_req_duration: ['p(95)<1000'],
        'http_req_duration{group:::Persons}': ['p(95)<1000'],
        'http_req_duration{group:::Persons Cached}': ['p(95)<1000']
    },
};

export default function () {
    group('Persons', function() {
        http.get(config.API_PERSONS_URL);
    });
    
    group('Persons Cached', function() {
        http.get(config.API_PERSONS_CACHED_URL);
    });
    
    sleep(1);
}
