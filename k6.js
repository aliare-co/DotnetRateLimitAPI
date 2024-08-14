import http from "k6/http";
import { check, sleep } from "k6";

const userIds = [
    0, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22,
    23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41,
    42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 59, 60, 61, 62,
    63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 82, 84, 85, 86,
    91, 92, 93, 94, 95, 96, 97, 98, 99, 103, 104, 107, 108, 109, 110, 111, 112,
    114, 115, 116, 118, 120, 121, 122, 123, 124, 125, 128, 129, 130, 131, 132,
    134, 135, 136, 138, 139, 140, 141, 142, 143, 144, 154, 157, 160, 161, 163,
    164, 165, 166, 167, 168, 169, 170, 171, 172, 173, 175, 178, 181, 182, 183,
    184, 185, 187, 189, 190, 191, 192, 195, 196, 197, 199, 200, 201, 202, 203,
    204, 205, 206, 207, 208, 209, 210, 212, 213, 214, 215, 216, 217, 218, 219,
    220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, 232, 233, 234,
    235, 237, 238, 243, 244, 247, 250, 251, 257, 258, 259, 291, 293, 302, 303,
    310, 315, 316, 317, 330, 331, 333, 336, 337, 338, 340, 342, 345, 346, 371,
    373, 375, 376, 378, 380, 384, 388, 389, 391, 392,
];

//const baseUrl = "http://localhost:5051/database/new/";
const baseUrl = "http://localhost:8080/database/new/";

export let options = {
    scenarios: {
        test_scenario: {
            executor: "per-vu-iterations",
            vus: 1, //userIds.length,
            iterations: 1,
        },
    },
};

export default async () => {
    const requests = userIds.map(async id => {
        sleep(1/8);

        return http.asyncRequest('GET', `${baseUrl}${id}`, { timeout: "120s" });
    });

    const responses = await Promise.all(requests);

    responses.forEach(res => {
        check(res, {
            "200: new database created!": (r) => r.status === 200,
            "404: error": (r) => r.status === 404,
            "429: too many requests": (r) => r.status === 429,
            "503: service unavailable": (r) => r.status === 503,
        });
    });
}