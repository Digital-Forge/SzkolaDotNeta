namespace Application.Constans
{
    public static class Constans
    {
        public static class Role
        {
            public static class Name
            {
                public const string Administration = "Admin";
                public const string PickupPoint = "UpPoint";
            }

            public static class Id
            {
                public static readonly Guid Administration = Guid.Parse("6E18233D-845C-4558-8BA1-4EE8C9C0724A");
                public static readonly Guid PickupPoint = Guid.Parse("5C1D5993-16E1-4C65-B290-06B093EB189B");
            }
        }
    }
}
