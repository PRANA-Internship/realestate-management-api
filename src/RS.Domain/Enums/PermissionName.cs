namespace RS.Domain.Enums
{
    public static class PermissionName
    {
        public static string Of(Entity entity, Permission permission)
            => $"{entity}:{permission}";
    }
}
