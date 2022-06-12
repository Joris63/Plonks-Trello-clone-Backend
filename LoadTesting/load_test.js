import http from "k6/http";
import { check, fail } from "k6";
import crypto from "k6/crypto";
import encoding from "k6/encoding";

const algToHash = {
  HS256: "sha256",
  HS384: "sha384",
  HS512: "sha512",
};

// sign jwt
function sign(data, hashAlg, secret) {
  let hasher = crypto.createHMAC(hashAlg, secret);
  hasher.update(data);

  // Some manual base64 rawurl encoding as `Hasher.digest(encodingType)`
  // doesn't support that encoding type yet.
  return hasher
    .digest("base64")
    .replace(/\//g, "_")
    .replace(/\+/g, "-")
    .replace(/=/g, "");
}

// encode jwt
function encode(payload, secret, algorithm) {
  algorithm = algorithm || "HS256";
  let header = encoding.b64encode(
    JSON.stringify({ typ: "JWT", alg: algorithm }),
    "rawurl"
  );
  payload = encoding.b64encode(JSON.stringify(payload), "rawurl");
  let sig = sign(header + "." + payload, algToHash[algorithm], secret);
  return [header, payload, sig].join(".");
}

// decode jwt
function decode(token, secret, algorithm) {
  let parts = token.split(".");
  let header = JSON.parse(encoding.b64decode(parts[0], "rawurl"));
  let payload = JSON.parse(encoding.b64decode(parts[1], "rawurl"));
  algorithm = algorithm || algToHash[header.alg];
  if (sign(parts[0] + "." + parts[1], algorithm, secret) != parts[2]) {
    throw Error("JWT signature verification failed");
  }
  return payload;
}

export let options = {
  insecureSkipTLSVerify: true,
  noConnectionReuse: false,
  stages: [
    { duration: "5m", target: 100 }, // ramp up to 100 users over a 5 minute period
    { duration: "10m", target: 100 }, // maintain 100 users count
    { duration: "5m", target: 0 }, // ramp down to 0 users
  ],
  thresholds: {
      http_req_duration: ['p(99)<150'] // 99% of requests must complete below 150ms
  }
};

export default () => {
  const userId = "an existing user id";

  const message = {
    id: userId,
    jti: "5cde3905-d34c-4e2f-8a0e-fe2adc935d09",
    aud: ["https://plonks.nl"],
    iss: "https://api.plonks.nl",
    exp: 1750036883,
  };

  const token = encode(message, "jwt secret");

  const params = {
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  const res = http.get(`https://api.plonks.nl/board/all/${userId}`, params);
  
  const checkOutput = check(res, { "status was 200": (r) => r.status == 200 });

  if (!checkOutput) {
    fail("unexpected response");
  }
};
