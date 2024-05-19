# xPlanner [API schema]

### Auth:

[POST] /auth/register (string email, string password) => (User user, string accessToken)
1. check is user exist. if not create new user
2. generate token with userId
3. save refresh token in cookie
4. return user + accessToken 

[POST] /auth/login (string email, string password) => (User user, string accessToken)
1. validate user
2. generate token with userId
3. save refresh token in cookie
4. return user + accessToken 

[POST] /auth/login/access-token () => (User user, string accessToken)
1. get refresh token from cookie
2. if token exist and it`s valid get userId
3. generate token with userId
4. save refresh token in cookie
5. return user + accessToken 

[POST] /auth/logout () => (bool result)
1. remove refresh token from cookie

### Timer

[GET] user/timer/today () => (PomodoroSession session)

[POST] user/timer () => (PomodoroSession session)

[PUT] user/timer/round/:id (int id, bool totalSec, bool isCompleted) => (PomodoroRound round)

[PUT] user/timer/:id (int id, bool isCompleted) => (PomodoroSession session)

[DELETE] user/timer/:id (int id) => (PomodoroSession session)

### Task

[GET] user/tasks () => (UsetTask[] tasks)

[POST] user/tasks (string name, bool isCompleted, string createdAt, string priority) => (UsetTask task)

[PUT] user/tasks/:id (int id, string name, bool isCompleted, string createdAt, string priority) => (UsetTask task)

[DELETE] user/tasks/:id (int id) => (UsetTask task)

### TimeBlock

[GET] user/time-blocks () => (TimeBlock[] timeBlocks)

[POST] user/time-blocks (string name, string color, int duration, int order) => (TimeBlock timeBlock)

[PUT] user/time-blocks/update-order (string[] ids) => (TimeBlock[] timeBlocks)

[PUT] user/time-blocks/:id (int id, string name, string color, int duration, int order) =>(TimeBlock timeBlock)

[DELETE] user/time-blocks/:id (int id) => (TimeBlock timeBlock)

### User
[GET] user/profile () => (string email, string name)

[POST] user/profile (string email, string name, string password, int workInterval, int breakInterval, int intervalsCount) => (User user, (string, string)[] statistics)
