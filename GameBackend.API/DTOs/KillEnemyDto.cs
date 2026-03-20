namespace GameBackend.API.DTOs
{
    public class KillEnemyDto
    {
        public string EnemyName { get; set; } = string.Empty;
        public int BaseXpReward { get; set; }
        public int BaseCoinReward { get; set; }
    }
}