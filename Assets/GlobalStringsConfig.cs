public static class GlobalStringsConfig
{
    public static class Animations
    {
        public const string Idle = "idle";
        public const string Move = "move";
        public const string JumpFall = "jumpFall";
        public const string WallSlide = "wallSlide";
        public const string Dash = "dash";
        public const string BasicAttack = "basicAttack";
        public const string BasicAttackIndex = "basicAttackIndex";
        public const string yVelocity = "yVelocity";
    }

    public static class LayerMasks
    {
        public static int Default => 1 << 0;           // Layer 0
        public static int TransparentFX => 1 << 1;     // Layer 1
        public static int IgnoreRaycast => 1 << 2;     // Layer 2
        // Layer 3 is empty in Unity by default
        public static int Water => 1 << 4;             // Layer 4
        public static int UI => 1 << 5;                // Layer 5
        public static int Ground => 1 << 6;            // Layer 6
    }
}