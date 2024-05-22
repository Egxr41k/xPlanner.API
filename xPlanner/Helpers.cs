namespace xPlanner
{
    public class Helpers
    {
        public static int GetUserIdFromContext(HttpContext context)
        {
            var userIdClaim = context.User.Claims
                .FirstOrDefault(claim => claim.Type == "userId");

            return Convert.ToInt32(userIdClaim?.Value);
        }
    }
}
