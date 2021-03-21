using Photon.Realtime;

namespace Extensions
{
    public static class PhotonPlayerExtensions
    {
        public static string GetNicknameOrDefault(this Player player)
        {
            if (player == null)
            {
                return "anonymous";
            }
            if (string.IsNullOrEmpty(player.NickName))
            {
                return "player " + player.UserId;
            }

            return player.NickName;
        }
    }
}