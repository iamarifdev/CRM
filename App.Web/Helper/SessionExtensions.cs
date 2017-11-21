using System.Web;

namespace App.Web.Helper
{
    public static class SessionExtensions
    {
        public static T Get<T>(this HttpSessionStateBase session, string key)
        {
            return (T)session[key];
        }

        public static void Set(this HttpSessionStateBase session, string key, object value)
        {
            session[key] = value;
        }

        public static void Clean(this HttpSessionStateBase session, string key)
        {
            session.Remove(key);
        }
    }
}