namespace wdskills.DomainLayer.Domain
{
    public class UserEffect
    {
        public bool UserEffectIsBan { get; set; } = false;
        public bool UserEffectIsVip { get; set; } = false;
        public bool UserEffectIsMute { get; set; } = false;
        public decimal UserEffectAbleToPay { get; set; }
        public DateTime? UserEffectEndBan { get; set; }
        public DateTime? UserEffectEndVip { get; set; }
        public DateTime? UserEffectEndMute { get; set; }
    }
}