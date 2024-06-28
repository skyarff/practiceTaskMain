namespace StockService.Repository.CookieRep
{
    public interface ICookieService
    {
        void SetCookie(string key, string value, int? expireTime);
        string GetCookie(string key);
        void RemoveCookie(string key);
    }
}
