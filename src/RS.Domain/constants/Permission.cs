using System;
using System.Collections.Generic;
using System.Text;

namespace RS.Domain.Constants;

public static class Permissions
{
    public static class Property
    {
        public const string Create = "Property.Create";
        public const string Read = "Property.Read";
        public const string Update = "Property.Update";
        public const string Delete = "Property.Delete";
        public const string List = "Property.List";
    }

    public static class User
    {
        public const string Create = "User.Create";
        public const string Read = "User.Read";
        public const string Update = "User.Update";
        public const string Delete = "User.Delete";
        public const string List = "User.List";
    }
}
