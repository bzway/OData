namespace OpenData.Sites.FrontPage.Models
{
    public class TrackModel
    {
        public bool useWechat { get; set; }
        public string trackId { get; set; }
        public string appId { get; set; }
        public long timeStamp { get; set; }
        public string nonceStr { get; set; }
        public string signature { get; set; }
    }
}