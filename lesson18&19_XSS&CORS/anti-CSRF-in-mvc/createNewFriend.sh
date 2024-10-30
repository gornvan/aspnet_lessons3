curl 'https://localhost:7099/Friend/Create' \
  -H 'accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7' \
  -H 'content-type: application/x-www-form-urlencoded' \
  -H 'user-agent: curl' \
  -o 'result.txt' \
  --data-raw 'FriendName=Harry_FROM_CURL&FriendID=222&__Invariant=FriendID&Place=London' \
  -v
