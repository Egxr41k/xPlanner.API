# xPlanner
[API schema]

## Auth:

[POST] /auth/register (string email, string password)
1. check is user exist. if not create new user
2. generate token with userId
3. save refresh token in cookie
4. return user + accessToken 

[POST] /auth/login (string email, string password)
1. validate user
2. generate token with userId
3. save refresh token in cookie
4. return user + accessToken 

[POST] /auth/login/access-token
1. get refresh token from cookie
2. if token exist and it`s valid get userId
3. generate token with userId
4. save refresh token in cookie
5. return user + accessToken 

[POST] /auth/logout
1. remove  refresh token from cookie

## Timer

[GET] user/timer/today

[POST] user/timer

[PUT] user/timer/round/:id (int id, bool totalSec, bool isCompleted)

[PUT] user/timer/:id (int id, bool isCompleted)

[DELETE] user/timer/:id (int id)

## Task

[GET] user/tasks

[POST] user/tasks (string name, bool isCompleted, string createdAt, string priority)

[PUT] user/tasks/:id (int id, string name, bool isCompleted, string createdAt, string priority)

[DELETE] user/tasks/:id (int id)

## TimeBlock

[GET] user/time-blocks

[POST] user/time-blocks (string name, string color, int duration, int order)

[PUT] user/time-blocks/update-order (string[] ids)

[PUT] user/time-blocks/:id (int id, string name, string color, int duration, int order)

[DELETE] user/time-blocks/:id (int id)

## User
[GET] user/profile

[POST] user/profile (string email, string name, string password, int workInterval, int breakInterval, int intervalsCount)
